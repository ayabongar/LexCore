using LexCore.Api.Models;
using LexCore.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexCore.Api.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IContentRepository repository)
        {
            var items = new List<ContentItem>
            {
                new ContentItem
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "EXT-001",
                    Title = "Introduction to Legal Tech",
                    Language = "en",
                    Status = "published",
                    Tags = new List<string> { "legal", "tech", "intro" },
                    PublishedAt = DateTime.UtcNow.AddDays(-10),
                    Body = "Legal technology refers to the use of technology and software to provide legal services..."
                },
                new ContentItem
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "EXT-002",
                    Title = "Introduction à la technologie juridique",
                    Language = "fr",
                    Status = "published",
                    Tags = new List<string> { "juridique", "technologie" },
                    PublishedAt = DateTime.UtcNow.AddDays(-5),
                    Body = "La technologie juridique fait référence à l'utilisation de la technologie et des logiciels pour fournir des services juridiques..."
                },
                new ContentItem
                {
                    Id = Guid.NewGuid(),
                    ExternalId = "EXT-003",
                    Title = "Drafting Future Laws",
                    Language = "en",
                    Status = "draft",
                    Tags = new List<string> { "future", "policy" },
                    PublishedAt = null,
                    Body = "This article explores the upcoming challenges in drafting laws for AI..."
                }
            };

            foreach (var item in items)
            {
                await repository.AddAsync(item);
            }
        }
    }
}
