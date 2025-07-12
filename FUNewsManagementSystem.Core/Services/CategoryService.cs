using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            if (await CanDeleteAsync(id))
            {
                await _categoryRepository.DeleteAsync(id);
            }
            else
            {
                throw new InvalidOperationException("Danh mục đang được sử dụng bởi bài viết.");
            }
        }

        public async Task<List<Category>> SearchAsync(string name, int? status)
        {
            return await _categoryRepository.SearchAsync(name, status);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return !await _categoryRepository.HasArticlesAsync(id);
        }
    }
}
