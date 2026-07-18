using LexCore.Api.DTOs;
using LexCore.Api.Models;
using LexCore.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexCore.Api.Services
{
    public class ContentService
    {
        private readonly IContentRepository _repository;

        public ContentService(IContentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContentItemResponse>> GetItemsAsync(string? language, string? status)
        {
            var items = await _repository.GetAllAsync(language, status);
            return items.Select(MapToResponse);
        }

        public async Task<ContentItemResponse?> GetItemByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item != null ? MapToResponse(item) : null;
        }

        public async Task<ContentItemResponse> CreateItemAsync(CreateContentItemRequest request)
        {
            var item = new ContentItem
            {
                Id = Guid.NewGuid(),
                ExternalId = request.ExternalId,
                Title = request.Title,
                Language = request.Language,
                Status = request.Status,
                Tags = request.Tags,
                Body = request.Body,
                PublishedAt = request.Status == "published" ? DateTime.UtcNow : null
            };

            await _repository.AddAsync(item);
            return MapToResponse(item);
        }

        public async Task<bool> UpdateItemAsync(Guid id, UpdateContentItemRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Title = request.Title;
            existing.Language = request.Language;
            existing.Status = request.Status;
            existing.Tags = request.Tags;
            existing.Body = request.Body;

            if (request.Status == "published" && existing.PublishedAt == null)
            {
                existing.PublishedAt = DateTime.UtcNow;
            }

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        private static ContentItemResponse MapToResponse(ContentItem item) =>
            new ContentItemResponse(
                item.Id,
                item.ExternalId,
                item.Title,
                item.Language,
                item.Status,
                item.Tags,
                item.PublishedAt,
                item.Body
            );
    }
}
