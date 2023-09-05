using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Flatscha.EFCore.Api.Generators
{
    public static class MinimalAPITestContextExtensions
    {
        public static IEndpointRouteBuilder MapTestContext(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapUser();

            return endpoints;
        }
    }
}