using LexCore.Api.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexCore.Api.Repositories
{
    public class InMemoryContentRepository : IContentRepository
    {
        private readonly ConcurrentDictionary<Guid, ContentItem> _items = new();

        public Task<IEnumerable<ContentItem>> GetAllAsync(string? language = null, string? status = null)
        {
            var query = _items.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(i => i.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult(query.OrderByDescending(i => i.PublishedAt ?? DateTime.MinValue).AsEnumerable());
        }

        public Task<ContentItem?> GetByIdAsync(Guid id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task AddAsync(ContentItem item)
        {
            if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
            _items[item.Id] = item;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ContentItem item)
        {
            _items[item.Id] = item;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _items.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
