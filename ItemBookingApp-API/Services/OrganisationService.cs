using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Persistence.Repositories;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository _genericRepository;
        private readonly IOrganisationRepository _organisationRepository;

        public OrganisationService(IUnitOfWork unitOfWork, 
            IGenericRepository genericRepository,
            IOrganisationRepository organisationRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _organisationRepository = organisationRepository;
        }

        public async Task<OrganisationResponse> ActivateOrDeactivateOrganisation(int organisationId, bool organisationStatus)
        {
            var organisationFromRepo = await _genericRepository.FindAsync<Organisation>(x => x.Id == organisationId);

            if (organisationFromRepo == null)
                return new OrganisationResponse("Organisation not found");

            var result = (organisationStatus) ? organisationFromRepo.Status == EntityStatus.Active : organisationFromRepo.Status == EntityStatus.Disabled;

            await _unitOfWork.CompleteAsync();
            return new OrganisationResponse(organisationFromRepo);
        }

        public async Task<OrganisationResponse> SaveAsync(Organisation organisation)
        {
            try
            {
                var existingOrganisation = await _genericRepository.FindAsync<Organisation>(o => o.Name == organisation.Name);

                if (existingOrganisation != null)
                    return new OrganisationResponse("Organisation already exist");

                //send notification to user about pending approval
                organisation.Status = EntityStatus.Pending;
                                
                await _genericRepository.AddAsync<Organisation>(organisation);
                await _unitOfWork.CompleteAsync();

                return new OrganisationResponse(organisation);
            }
            catch (Exception ex)
            {
                // Do some logging
                return new OrganisationResponse($"An error occurred while creating the organisation: {ex.Message}");
            }
        }

        public Task<PagedList<Organisation>> ListAsync(OrganisationQuery organisationQuery)
        {
            return _organisationRepository.ListAsync(organisationQuery);
        }

        public async Task<IEnumerable<Organisation>> ListAsync()
        {
            return await _organisationRepository.ListAsync();
        }

        public async Task<OrganisationResponse> UpdateAsync(int organisationId, Organisation organisation)
        {
            var organisationFromRepo = await _genericRepository.FindAsync<Organisation>(c => c.Id == organisationId);

            if (organisationFromRepo == null)
                return new OrganisationResponse("Organisation not found");

            var isExist = await _organisationRepository.IsExist(organisation.Name);

            if (isExist)
                return new OrganisationResponse("Organisation already exist");

            organisationFromRepo.Name = organisation.Name;

            try
            {
                await _unitOfWork.CompleteAsync();
                return new OrganisationResponse(organisationFromRepo);
            }
            catch (Exception ex)
            {
                return new OrganisationResponse($"An error occured when updating organisation: {ex.Message}");
            }
        }
      
    }
}
