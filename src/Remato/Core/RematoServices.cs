using System;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Remato.Data.Migrations;
using Talaryon.Data;
using Newtonsoft.Json;
using Remato.Security;
using Remato.Security.Authenticators;
using Remato.Services;
using Remato.Shared;
using Talaryon;

namespace Remato
{
    public static class RematoServices
    {
        public static IServiceCollection AddRematoCore(this IServiceCollection services, StoragrConfig config)
        {
            var mediaType = new RematoMediaType();
            var storagrConfig = config.Get<StoragrCoreConfig>();

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Listen(storagrConfig.Listen);
            });
            
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    mediaType.MediaType.Value
                });
            });
            services.AddMvcCore()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add(new RematoExceptionFilter(mediaType));
                    options.Filters.Add(new ProducesAttribute(mediaType.MediaType.Buffer));

                    foreach (var input in options.InputFormatters.OfType<NewtonsoftJsonInputFormatter>())
                    {
                        input.SupportedMediaTypes.Add(mediaType);
                    }

                    foreach (var output in options.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>())
                    {
                        output.SupportedMediaTypes.Add(mediaType);
                    }

                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";
                });

            return services
                .AddSingleton<IStoragrService, StoragrService>()
                .AddSingleton<ICacheService, CacheService>()
                .AddScoped<IBatchService, BatchService>();
        }

        public static IServiceCollection AddRematoSecurity(this IServiceCollection services, StoragrConfig config)
        {
            var tokenConfig = config.Get<TokenOptions>();

            services.AddAuthentication("_")
                .AddPolicyScheme("_", "AuthRouter", options =>
                {
                    options.ForwardDefaultSelector = ctx =>
                    {
                        if (ctx.Request.Headers.ContainsKey(HeaderNames.Authorization) && ((string)ctx.Request.Headers[HeaderNames.Authorization]).StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }
                        return BasicAuthenticationDefaults.AuthenticationScheme;
                    };
                })
                .AddBasic(options =>
                {

                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenConfig;
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = (context) =>
                        {
                            if (context.AuthenticateFailure is not SecurityTokenException) 
                                return Task.CompletedTask;
                            
                            context.HandleResponse();
                            
                            var error = context.AuthenticateFailure switch
                            {
                                SecurityTokenExpiredException e => new TokenExpiredError()
                                {
                                    ExpiredAt = e.Expires
                                },
                                _ => new UnauthorizedError(context.AuthenticateFailure.Message) 
                            };
                            var data = TalaryonHelper.SerializeObject(error);

                            context.Response.StatusCode = error.Code;
                            context.Response.ContentType = (new RematoMediaType()).MediaType.Value;
                            return context.Response.Body.WriteAsync(data, 0, data.Length);
                        }
                    };
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy(RematoConstants.ManagementPolicy, x =>
                {
                    x.RequireAuthenticatedUser();
                    x.RequireRole(RematoConstants.ManagementRole);
                });
            });

            services
                .AddConfig<TokenOptions>(config)
                .AddSingleton<ITokenService, TokenService>();

            services.AddAuthentication<DefaultAuthenticator>();

            return services;
        }

        public static IServiceCollection AddRematoCache(this IServiceCollection services,  StoragrConfig config)
        {
            var storagrConfig = config.Get<StoragrCoreConfig>();
            
            switch (storagrConfig.Cache)
            {
                case StoragrCacheType.Memory:
                    services.AddDistributedMemoryCache();
                    break;
                
                case StoragrCacheType.Redis:
                    var redisConfig = config.Get<RedisCacheConfig>();
                    
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.InstanceName = "redis";
                        options.Configuration = redisConfig.Host;
                    });
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return services;
        }

        public static IServiceCollection AddRematoDatabase(this IServiceCollection services, StoragrConfig config)
        {
            var storagrConfig = config.Get<StoragrCoreConfig>();

            services
                .AddFluentMigratorCore()
                .ConfigureRunner(options =>
                {
                    switch (storagrConfig.Backend)
                    {
                        case StoragrBackendType.Sqlite:
                            var sqliteConfig = config.Get<SqliteOptions>();
                            options
                                .AddSQLite()
                                .WithGlobalConnectionString($"Data Source={sqliteConfig.DataSource}");
                            break;

                        case StoragrBackendType.MySql:
                            var mysqlConfig = config.Get<MysqlOptions>();
                            options
                                .AddMySql5()
                                .WithGlobalConnectionString(
                                    $"server={mysqlConfig.Server};" +
                                    $"uid={mysqlConfig.User};" +
                                    $"pwd={mysqlConfig.Password};" +
                                    $"database={mysqlConfig.Database}"
                                );
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    options.ScanIn(typeof(Setup).Assembly).For.Migrations();
                });

            switch (storagrConfig.Backend)
            {
                case StoragrBackendType.Sqlite:
                    services
                        .AddConfig<SqliteOptions>(config)
                        .AddBackend<SqliteAdapter>();
                    break;
                
                case StoragrBackendType.MySql:
                    services
                        .AddConfig<MysqlOptions>(config)
                        .AddBackend<MysqlAdapter>();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return services;
        }
    }
}