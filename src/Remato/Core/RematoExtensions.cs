using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Talaryon.Data;

namespace Remato
{
    public static class RematoExtensions
    {
        public static IServiceCollection AddBackend<TBackend>(this IServiceCollection services)
            where TBackend : class, IDatabaseAdapter
        {
            return services
                .AddSingleton<TBackend>()
                .AddSingleton<IDatabaseAdapter>(x => x.GetRequiredService<TBackend>());
        }
        public static IServiceCollection AddBackend<TBackend, TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
            where TBackend : class, IDatabaseAdapter
            where TOptions : class, IOptions<TOptions>
        {
            return services
                .AddOptions()
                .Configure(configureOptions)
                .AddSingleton<TBackend>()
                .AddSingleton<IDatabaseAdapter>(x => x.GetRequiredService<TBackend>());
        }
        
        public static IServiceCollection AddAuthentication<TAuthentication>(this IServiceCollection services)
            where TAuthentication : class, IAuthenticationAdapter
        {
            return services
                .AddSingleton<TAuthentication>()
                .AddSingleton<IAuthenticationAdapter>(x => x.GetRequiredService<TAuthentication>());
        }
        public static IServiceCollection AddAuthentication<TAuthentication, TOptions>(this IServiceCollection services, Action<TOptions> configureOptions)
            where TAuthentication : class, IAuthenticationAdapter
            where TOptions : class, IOptions<TOptions>
        {
            return services
                .AddOptions()
                .Configure(configureOptions)
                .AddSingleton<TAuthentication>()
                .AddSingleton<IAuthenticationAdapter>(x => x.GetRequiredService<TAuthentication>());
        }
    }
}