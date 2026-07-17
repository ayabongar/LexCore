using LexCore.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexCore.Api.Repositories
{
    public interface IContentRepository
    {
        Task<IEnumerable<ContentItem>> GetAllAsync(string? language = null, string? status = null);
        Task<ContentItem?> GetByIdAsync(Guid id);
        Task AddAsync(ContentItem item);
        Task UpdateAsync(ContentItem item);
        Task DeleteAsync(Guid id);
    }
}
