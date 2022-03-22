using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Remato.Security.Tokens;
using Remato.Shared;
using Talaryon;

namespace Remato.Services
{
    public partial class RematoService
    {
        private class Authentication :
            IRematoServiceAuthorization,
            IRematoServiceAuthorizationAuthentication,
            ITalaryonRunner<IRematoServiceAuthorizationResult>,
            ITalaryonRunner<UserEntity>,
            ITalaryonRunner<string>
        {
            private readonly RematoService _rematoService;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private string[] _credentials;
            private string _token;

            public Authentication(RematoService rematoService, IHttpContextAccessor httpContextAccessor)
            {
                _rematoService = rematoService;
                _httpContextAccessor = httpContextAccessor;
            }

            /**
             * Authentication
             */
            IRematoServiceAuthorizationAuthentication IRematoServiceAuthorization.Authenticate() => this;

            ITalaryonRunner<IRematoServiceAuthorizationResult> IRematoServiceAuthorizationAuthentication.With(string username, string password)
            {
                _credentials = new[] {username, password};
                return this;
            }

            ITalaryonRunner<IRematoServiceAuthorizationResult> IRematoServiceAuthorizationAuthentication.With(string token)
            {
                _token = token;
                return this;
            }

            IRematoServiceAuthorizationResult ITalaryonRunner<IRematoServiceAuthorizationResult>.Run() =>
                (this as ITalaryonRunner<IRematoServiceAuthorizationResult>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<IRematoServiceAuthorizationResult> ITalaryonRunner<IRematoServiceAuthorizationResult>.RunAsync(
                CancellationToken cancellationToken)
            {
                // TODO throw exception on failure

                var result = await _rematoService.Authenticator.Authenticate(_credentials[0], _credentials[1], cancellationToken);
                if (result is null)
                {
                    throw new UnauthorizedError("Invalid username and/or password.");
                }

                var authName = _rematoService.Authenticator.Name;
                var user = await _rematoService
                    .Database
                    .First<UserEntity>()
                    .Where(filter => filter
                        .Is(nameof(UserEntity.AuthAdapter))
                        .EqualTo(authName)
                        .And()
                        .Is(nameof(UserEntity.AuthId))
                        .EqualTo(result.Id)
                    )
                    .RunAsync(cancellationToken);

                if (user is null)
                {
                    var count = await _rematoService
                        .Database
                        .Count<UserEntity>()
                        .RunAsync(cancellationToken);

                    await _rematoService
                        .Database
                        .Insert(user = new UserEntity()
                        {
                            Id = TalaryonHelper.UUID(),
                            IsEnabled = true,
                            IsAdmin = (count == 0), // first created user is admin
                            AuthAdapter = authName,
                            AuthId = result.Id,
                            Username = result.Username,
                        })
                        .RunAsync(cancellationToken);
                }
                else if (user.Username != result.Username)
                {
                    user.Username = result.Username;
                    await _rematoService
                        .Database
                        .Update(user)
                        .RunAsync(cancellationToken);
                }

                var token = new UserToken()
                {
                    UserId = user.Id,
                    Role = user.IsAdmin ? RematoConstants.ManagementRole : default
                };
                user.Token = _rematoService
                                 .TokenService
                                 .Generate(token)
                             ?? throw new Exception("Unable to generate token!");

                return new AuthenticationResult(user, user.Token);
            }

            /**
             * GetAuthenticatedUser
             */
            ITalaryonRunner<UserEntity> IRematoServiceAuthorization.GetAuthenticatedUser() => this;

            UserEntity ITalaryonRunner<UserEntity>.Run() => (this as ITalaryonRunner<UserEntity>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            Task<UserEntity> ITalaryonRunner<UserEntity>.RunAsync(CancellationToken cancellationToken)
            {
                var context = _httpContextAccessor.HttpContext ?? throw new NullReferenceException();
                var userId = context.User.FindFirst(RematoConstants.UniqueTokenId)?.Value;

                return (_rematoService as IRematoService)
                    .User(userId)
                    .RunAsync(cancellationToken);
            }

            /**
             * GetAuthenticatedToken
             */
            ITalaryonRunner<string> IRematoServiceAuthorization.GetAuthenticatedToken() => this;

            string ITalaryonRunner<string>.Run() => (this as ITalaryonRunner<string>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<string> ITalaryonRunner<string>.RunAsync(CancellationToken cancellationToken)
            {
                var context = _httpContextAccessor.HttpContext ?? throw new NullReferenceException();
                var token = await Task.Run(() => context.GetTokenAsync("Bearer", "access_token"), cancellationToken);
                token ??= await Task.Run(() => context.GetTokenAsync("Basic", "access_token"), cancellationToken);

                return token;
            }


        }

        private class AuthenticationResult : IRematoServiceAuthorizationResult
        {
            public AuthenticationResult(UserEntity user, string token)
            {
                User = user;
                Token = token;
            }

            public UserEntity User { get; }
            public string Token { get; }
        }
    }
}