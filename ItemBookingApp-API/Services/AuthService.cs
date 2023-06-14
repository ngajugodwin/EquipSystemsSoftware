using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Helpers;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Resources.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ItemBookingApp_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IGenericRepository _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public AuthService(ApplicationUserManager applicationUserManager,
                           SignInManager<AppUser> signInManager,
                           IMapper mapper,
                           IOptions<AppSettings> appSettings,
                           IGenericRepository genericRepository,
                           IUnitOfWork unitOfWork)
        {
            _applicationUserManager = applicationUserManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }
        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _applicationUserManager.FindByEmailAsync(email);

            if (user == null)
                return new AuthResponse("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                var newRefreshToken = CreateRefreshToken(_appSettings.ClientId, user.Id);

                var oldUserTokenFromRepo = await _genericRepository.ListAsync<Token>(t => t.UserId == user.Id);

                if (oldUserTokenFromRepo.Count() > 0)
                {
                    foreach (var token in oldUserTokenFromRepo)
                    {
                        _genericRepository.Remove<Token>(token);
                    }
                }

                await _genericRepository.AddAsync<Token>(newRefreshToken);
                await _unitOfWork.CompleteAsync();

                return new AuthResponse(
                        new TokenResource
                        {
                            Token = await GenerateAccessToken(user),
                            RefreshToken = newRefreshToken.Value,
                            Expiration = newRefreshToken.ExpiryTime,
                            User = _mapper.Map<UserResource>(user)
                        });
            }

            return new AuthResponse("Invalid email or password");
        }

        public async Task<AuthResponse> RefreshTokenAsync(string userRefreshToken)
        {
            try
            {
                var userRefreshTokenFromRepo = await _genericRepository
                    .FindAsync<Token>(t => t.ClientId == _appSettings.ClientId
                    && t.Value == userRefreshToken.ToString());

                var MSG = "You are not authorized";
                if (userRefreshTokenFromRepo == null)
                    return new AuthResponse(MSG);

                if (userRefreshTokenFromRepo.ExpiryTime < DateTime.Now)
                    return new AuthResponse(MSG); //refresh token is expired

                var userFromRepo = await _applicationUserManager.FindByIdAsync(userRefreshTokenFromRepo.UserId);

                if (userFromRepo == null)
                    return new AuthResponse(MSG);

                var newRefreshToken = CreateRefreshToken(userRefreshTokenFromRepo.ClientId, userRefreshTokenFromRepo.UserId);

                _genericRepository.Remove<Token>(userRefreshTokenFromRepo);

                await _genericRepository.AddAsync<Token>(newRefreshToken);
                await _unitOfWork.CompleteAsync();


                return new AuthResponse(
                       new TokenResource
                       {
                           Token = await GenerateAccessToken(userFromRepo),
                           RefreshToken = newRefreshToken.Value,
                           Expiration = newRefreshToken.ExpiryTime,
                           User = _mapper.Map<UserResource>(userFromRepo)
                       });

            }
            catch (Exception ex)
            {
                return new AuthResponse($"An error occured: {ex.Message}");
            }
        }

        private Token CreateRefreshToken(string clientId, long userId)
        {
            return new Token
            {
                ClientId = clientId,
                UserId = userId,
                Value = Guid.NewGuid().ToString("N"),
                CreatedAt = DateTime.Now,
                ExpiryTime = DateTime.Now.AddMinutes(90) // refresh token stays valid for 90 mins while generated access token stays for 60mins
            };
        }

        private async Task<string> GenerateAccessToken(AppUser user)
        {
            var tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            var organisationId = (user.OrganisationId > 0) ? user.OrganisationId.ToString() : string.Empty;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
//                new Claim(JwtRegisteredClaimNames.NameId)
            };

            var roles = await _applicationUserManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(tokenExpiryTime),
                SigningCredentials = creds,
                Audience = _appSettings.Audience,
                Issuer = _appSettings.Site
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _applicationUserManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
