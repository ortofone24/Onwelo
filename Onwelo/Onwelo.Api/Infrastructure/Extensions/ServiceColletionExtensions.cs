using Onwelo.Api.Data;
using Onwelo.Api.Features.Votes;

namespace Onwelo.Api.Infrastructure.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddDatabaseFromExtensions(this IServiceCollection services)
            => services.AddSingleton<DapperContext>();

        public static IServiceCollection AddIdentityServiceFromExtensions(this IServiceCollection services)
            => services.AddTransient<IVoteService, VoteService>();
    }
}
