using MinhaMinimalApiDapper.Repository;
using MinhaMinimalApiDapper.Request;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("Default");

    return new NpgsqlConnection(connectionString);
});

builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

app.MapGet("/users/{id:guid}", async (Guid id, UserRepository repo) =>
{
    var user = await repo.GetByIdAsync(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
});

app.MapPost("/users", async (CreateUserRequest request, UserRepository repo) =>
{
    var id = await repo.CreateAsync(request);
    return Results.Created($"/users/{id}", new { id });
});

app.Run();