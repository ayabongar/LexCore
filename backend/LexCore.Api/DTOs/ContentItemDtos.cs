using System;
using System.Collections.Generic;

namespace LexCore.Api.DTOs
{
    public record CreateContentItemRequest(
        string ExternalId,
        string Title,
        string Language,
        string Status,
        List<string> Tags,
        string Body
    );

    public record UpdateContentItemRequest(
        string Title,
        string Language,
        string Status,
        List<string> Tags,
        string Body
    );

    public record ContentItemResponse(
        Guid Id,
        string ExternalId,
        string Title,
        string Language,
        string Status,
        List<string> Tags,
        DateTime? PublishedAt,
        string Body
    );
}
