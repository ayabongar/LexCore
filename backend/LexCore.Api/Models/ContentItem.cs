using System;
using System.Collections.Generic;

namespace LexCore.Api.Models
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "en"; // "fr", "en"
        public string Status { get; set; } = "draft"; // "draft", "published", "archived"
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime? PublishedAt { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}
