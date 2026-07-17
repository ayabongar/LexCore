using LexCore.Api.DTOs;
using LexCore.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace LexCore.Api.Endpoints
{
    public static class ContentEndpoints
    {
        public static void MapContentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/content");

            group.MapGet("/", async (ContentService service, string? language, string? status) =>
            {
                var items = await service.GetItemsAsync(language, status);
                return Results.Ok(items);
            });

            group.MapGet("/{id:guid}", async (ContentService service, Guid id) =>
            {
                var item = await service.GetItemByIdAsync(id);
                return item is not null ? Results.Ok(item) : Results.NotFound();
            });

            group.MapPost("/", async (ContentService service, CreateContentItemRequest request) =>
            {
                var item = await service.CreateItemAsync(request);
                return Results.Created($"/api/content/{item.Id}", item);
            });

            group.MapPut("/{id:guid}", async (ContentService service, Guid id, UpdateContentItemRequest request) =>
            {
                var success = await service.UpdateItemAsync(id, request);
                return success ? Results.NoContent() : Results.NotFound();
            });

            group.MapDelete("/{id:guid}", async (ContentService service, Guid id) =>
            {
                var success = await service.DeleteItemAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
