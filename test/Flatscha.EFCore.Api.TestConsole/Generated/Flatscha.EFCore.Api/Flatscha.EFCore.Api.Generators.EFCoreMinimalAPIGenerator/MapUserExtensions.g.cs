using Flatscha.EFCore.Api.TestConsole.Models;
using Flatscha.EFCore.Api.TestConsole.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Flatscha.EFCore.Api.Generators
{
    public static class MapUserExtensions
    {
        public static IEndpointRouteBuilder MapUser(this IEndpointRouteBuilder endpoints)
        {
            var entityEndpoints = endpoints.MapGroup("User").WithTags("User");

            entityEndpoints.MapGet("/", async (TestContext context, CancellationToken cancellationToken) 
                => await context.Users.ToListAsync(cancellationToken));

            entityEndpoints.MapGet("/{id}", async (TestContext context, Guid id, CancellationToken cancellationToken) 
                => await context.Users.FindAsync(id, cancellationToken) is User entity ? Results.Ok(entity) : Results.NotFound("No User found"));

            entityEndpoints.MapPost("/Save", async (TestContext context, User entity, CancellationToken cancellationToken) =>
            {
                context.Users.Add(entity);
                await context.SaveChangesAsync(cancellationToken);
                return Results.Ok(entity);
            });

            entityEndpoints.MapPut("/Update/{id}", async (TestContext context, User entity, Guid id, CancellationToken cancellationToken) =>
            {
                var dbEntity = await context.Users.FindAsync(id, cancellationToken);

                if (dbEntity is null) { return Results.NotFound("No User found"); }
                dbEntity.Id = entity.Id;
                dbEntity.FirstName = entity.FirstName;
                dbEntity.LastName = entity.LastName;
                dbEntity.Title = entity.Title;
                dbEntity.UserName = entity.UserName;
                dbEntity.Password = entity.Password;
                dbEntity.LastLogin = entity.LastLogin;
                dbEntity.ObjectIsDeleted = entity.ObjectIsDeleted;
                dbEntity.ObjectCreatedDateTime = entity.ObjectCreatedDateTime;
                dbEntity.ObjectCreatedUserId = entity.ObjectCreatedUserId;
                dbEntity.ObjectChangedDateTime = entity.ObjectChangedDateTime;
                dbEntity.ObjectChangedUserId = entity.ObjectChangedUserId;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(dbEntity);
            });

            entityEndpoints.MapDelete("/Delete/{id}", async (TestContext context, Guid id, CancellationToken cancellationToken) =>
            {
                var dbEntity = await context.Users.FindAsync(id, cancellationToken);

                if (dbEntity is null) { return Results.NotFound("No User found"); }

                context.Users.Remove(dbEntity);
                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok();
            });

            return endpoints;
        }
    }
}