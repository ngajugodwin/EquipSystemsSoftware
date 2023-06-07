using AutoMapper;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Mappings;
using ItemBookingApp_API.Persistence.Contexts;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Persistence.Seeders;
using ItemBookingApp_API.Services;
using ItemBookingApp_API.Services.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDbContext<ApplicationDbContext>(g => g.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


#region Repository and Services DI Registration

//Repo
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGenericRepository, GenericRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrganisationService, OrganisationService>();
//Services
#endregion



#region Automapper Configuration
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ItemMappingProfile());
    cfg.AddProfile(new CategoryMappingProfile());
    cfg.AddProfile(new AppUserMappingProfile());
    cfg.AddProfile(new OrganisationMappingProfile());
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion


builder.Services.AddIdentityServices(builder.Configuration, builder.Environment);

//cors start
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(PermissionSystemName.AccessSuperAdminArea, policy => policy.RequireRole("SuperAdmin"));
    opt.AddPolicy(PermissionSystemName.HasUserRole, policy => policy.RequireRole("User", "Admin", "Owner", "SuperAdmin"));
    opt.AddPolicy(PermissionSystemName.AccessOrganizationOwnerArea, policy => policy.RequireRole("Owner"));
    opt.AddPolicy(PermissionSystemName.AccessOrganisationAdminRole, policy => policy.RequireRole("Admin", "Owner"));
});

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});


builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200").Build();
    });
});
//cors ends
builder.Services.AddTransient<Seed>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


    //var userManager = app.Services.GetRequiredService<UserManager<AppUser>>();
    //var roleManager = app.Services.GetRequiredService<RoleManager<Role>>();
    //Seed seed = new Seed(userManager, roleManager);
    //seed.SeedUsers();
}





app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var roleManager = services.GetRequiredService<RoleManager<Role>>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    var seed = new Seed(userManager, roleManager);
    seed.SeedUsers();
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}




app.Run();


