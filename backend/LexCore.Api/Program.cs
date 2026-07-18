using LexCore.Api.Endpoints;
using LexCore.Api.Repositories;
using LexCore.Api.Services;
using LexCore.Api.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Dependencies
builder.Services.AddSingleton<IContentRepository, InMemoryContentRepository>();
builder.Services.AddScoped<ContentService>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Map Endpoints
app.MapContentEndpoints();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<IContentRepository>();
    await DataSeeder.SeedAsync(repo);
}

app.Run();
