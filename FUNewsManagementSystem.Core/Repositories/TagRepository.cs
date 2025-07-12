using FUNewsManagementSystem.Core.Data;
using FUNewsManagementSystem.Core.Interfaces;
using FUNewsManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.Core.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementContext _context;

        public TagRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tag>> SearchAsync(string name)
        {
            var query = _context.Tags.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(t => t.TagName.Contains(name));
            return await query.ToListAsync();
        }

        public async Task<bool> CanDeleteAsync(int tagId)
        {
            return !await _context.NewsArticleTags.AnyAsync(t => t.TagId == tagId);
        }
    }
}