using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Repositories;
using ItemBookingApp_API.Domain.Services;
using ItemBookingApp_API.Domain.Services.Communication;
using ItemBookingApp_API.Resources.CustomerQueries;
using ItemBookingApp_API.Resources.Query;

namespace ItemBookingApp_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGenericRepository _genericRepository;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IGenericRepository genericRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _genericRepository = genericRepository;
        }

        public async Task<CategoryResponse> ActivateOrDeactivateCategory(int categoryId, bool categoryStatus)
        {
            var categoryFromRepo = await _genericRepository.FindAsync<Category>(x => x.Id == categoryId);

            if (categoryFromRepo == null)
                return new CategoryResponse("Category not found");

            var result = (categoryStatus) ? categoryFromRepo.Status = EntityStatus.Active : categoryFromRepo.Status = EntityStatus.Disabled;

            await _unitOfWork.CompleteAsync();
            return new CategoryResponse(categoryFromRepo);
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int categoryId)
        {
            var categoryFromRepo = await _genericRepository.FindAsync<Category>(x => x.Id == categoryId);

            if (categoryFromRepo == null)
                return new CategoryResponse("Category not found");

            return new CategoryResponse(categoryFromRepo);

        }

        public async Task<PagedList<Category>> CustomerListAsync(CustomerCategoryQuery customerCategoryQuery)
        {
            return await _categoryRepository.CustomerListAsync(customerCategoryQuery);
        }

        public async Task<PagedList<Category>> ListAsync(CategoryQuery categoryQuery)
        {
            return await _categoryRepository.ListAsync(categoryQuery);
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _categoryRepository.ListAsync();
        }

        public async Task<CategoryResponse> SaveAsync(Category category)
        {
            try
            {
                // validate if a category exist
                var existingCategory = await _genericRepository.FindAsync<Category>(c => c.Name == category.Name);

                if (existingCategory != null)
                    return new CategoryResponse("Category already exist");

                // add the category to the database
                await _genericRepository.AddAsync<Category>(category);
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(category); //return entity
            }
            catch (Exception ex)
            {
                //return exception message 
                return new CategoryResponse($"An error occurred while saving the category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse> UpdateAsync(int categoryId, Category category)
        {
            // find category based on Id
            var categoryFromRepo = await _genericRepository.FindAsync<Category>(c => c.Id == categoryId);

            if (categoryFromRepo == null)
                return new CategoryResponse("Category not found");

            // check if category of same name exist
            var isExist = await _categoryRepository.IsExist(category.Name);

            if (isExist)
                return new CategoryResponse("Category already exist");

            // update the category name
            categoryFromRepo.Name = category.Name;

            try
            {   // save changes to databse and return data
                await _unitOfWork.CompleteAsync();
                return new CategoryResponse(categoryFromRepo);
            }
            catch (Exception ex)
            {   // return an exception if this occurs
                return new CategoryResponse($"An error occured when updating category: {ex.Message}");
            }
        }
    }
}
