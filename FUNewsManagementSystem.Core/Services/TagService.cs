using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;

namespace FUNewsManagementSystem.Core.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Tag tag)
        {
            await _tagRepository.AddAsync(tag);
        }

        public async Task UpdateAsync(Tag tag)
        {
            await _tagRepository.UpdateAsync(tag);
        }

        public async Task DeleteAsync(int id)
        {
            if (await CanDeleteAsync(id))
            {
                await _tagRepository.DeleteAsync(id);
            }
            else
            {
                throw new InvalidOperationException("Thẻ đang được sử dụng bởi bài viết.");
            }
        }

        public async Task<List<Tag>> SearchAsync(string name)
        {
            return await _tagRepository.SearchAsync(name);
        }

        public async Task<bool> CanDeleteAsync(int tagId)
        {
            return await _tagRepository.CanDeleteAsync(tagId);
        }
    }
}
