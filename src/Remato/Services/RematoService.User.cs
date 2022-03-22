using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Remato.Shared;
using Talaryon;

namespace Remato.Services
{
    public partial class RematoService
    {
        private class UserItem : IRematoUserService, IUserParams, ITalaryonRunner<bool>,
            ITalaryonParams<UserEntity, IUserParams>
        {
            private readonly RematoService _rematoService;
            private readonly string _userIdOrName;

            private UserEntity _createRequest;
            private Dictionary<string, object> _updateRequest;
            private bool _deleteRequest;

            public UserItem(RematoService rematoService, string userIdOrName)
            {
                _rematoService = rematoService;
                _userIdOrName = userIdOrName;
            }

            UserEntity ITalaryonRunner<UserEntity>.Run() => (this as ITalaryonRunner<UserEntity>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<UserEntity> ITalaryonRunner<UserEntity>.RunAsync(CancellationToken cancellationToken)
            {
                var cachedItem = await _rematoService
                    .Cache
                    .Key<UserEntity>(RematoCaching.GetKey(RematoCachingName.User, _userIdOrName))
                    .RunAsync(cancellationToken);

                var entityItem = cachedItem ?? await _rematoService
                    .Database
                    .First<UserEntity>()
                    .Where(filter => filter
                        .Is(nameof(UserEntity.IsDeleted))
                        .EqualTo(false)
                        .And()
                        .Clamp(filter2 => filter2
                            .Is(nameof(UserEntity.Id))
                            .EqualTo(_userIdOrName)
                            .Or()
                            .Is(nameof(UserEntity.Username))
                            .EqualTo(_userIdOrName)
                        )
                    )
                    .RunAsync(cancellationToken);

                if (_createRequest is not null)
                {
                    if (entityItem is not null)
                        throw new UserAlreadyExistsError(entityItem);

                    entityItem.Password = new PasswordHasher<UserEntity>()
                        .HashPassword(entityItem, entityItem.Password);
                    
                    // ReSharper disable once HeuristicUnreachableCode
                    entityItem = await _rematoService
                        .Database
                        .Insert(_createRequest)
                        .RunAsync(cancellationToken);
                }

                if (entityItem is null)
                    throw new UserNotFoundError();

                if (_updateRequest is not null)
                {
                    entityItem.ChangedAt = DateTime.Now;
                    entityItem.Username = (string)(_updateRequest.GetValueOrDefault("username") ?? entityItem.Username);
                    entityItem.Name = (string)(_updateRequest.GetValueOrDefault("name") ?? entityItem.Name);
                    entityItem.Mail = (string)(_updateRequest.GetValueOrDefault("mail") ?? entityItem.Mail);

                    if (_updateRequest.ContainsKey("password"))
                        entityItem.Password = new PasswordHasher<UserEntity>()
                            .HashPassword(entityItem, (string)_updateRequest["password"]);
                    
                    if (_updateRequest.ContainsKey("is_enabled"))
                        entityItem.IsEnabled = (bool)_updateRequest["is_enabled"];
                    
                    if (_updateRequest.ContainsKey("is_admin"))
                        entityItem.IsAdmin = (bool)_updateRequest["is_admin"];

                    await Task.WhenAll(
                        _rematoService
                            .Cache
                            .RemoveMany(new[]
                            {
                                RematoCaching.GetKey(RematoCachingName.User, entityItem.Id),
                                RematoCaching.GetKey(RematoCachingName.User, entityItem.Username)
                            })
                            .RunAsync(cancellationToken),
                        _rematoService
                            .Database
                            .Update(entityItem)
                            .RunAsync(cancellationToken)
                    );
                }

                if (_deleteRequest)
                {
                    entityItem.IsDeleted = true;
                    entityItem.ChangedAt = DateTime.Now;

                    await Task.WhenAll(
                        _rematoService
                            .Cache
                            .RemoveMany(new[]
                            {
                                RematoCaching.GetKey(RematoCachingName.User, entityItem.Id),
                                RematoCaching.GetKey(RematoCachingName.User, entityItem.Username)
                            })
                            .RunAsync(cancellationToken),
                        _rematoService
                            .Database
                            .Update(entityItem)
                            .RunAsync(cancellationToken)
                    );
                }
                else if (cachedItem is null)
                {
                    await Task.WhenAll(
                        _rematoService
                            .Cache
                            .Key<UserEntity>(RematoCaching.GetKey(RematoCachingName.User, entityItem.Id))
                            .Set(entityItem)
                            .RunAsync(cancellationToken),
                        _rematoService
                            .Cache
                            .Key<UserEntity>(RematoCaching.GetKey(RematoCachingName.User, entityItem.Username))
                            .Set(entityItem)
                            .RunAsync(cancellationToken)
                    );
                }

                return entityItem;
            }

            ITalaryonRunner<bool> ITalaryonExistable.Exists() => this;

            bool ITalaryonRunner<bool>.Run() => (this as ITalaryonRunner<bool>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<bool> ITalaryonRunner<bool>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    await (this as ITalaryonRunner<UserEntity>).RunAsync(cancellationToken);
                }
                catch (UserNotFoundError)
                {
                    return false;
                }

                return true;
            }


            ITalaryonParams<UserEntity, IUserParams> ITalaryonCreatable<UserEntity, IUserParams>.Create()
            {
                _createRequest = new UserEntity()
                {
                    Id = TalaryonHelper.UUID(),
                    Username = _userIdOrName,
                    CreatedAt = DateTime.Now,
                    ChangedAt = DateTime.Now
                };
                return this;
            }

            ITalaryonParams<UserEntity, IUserParams> ITalaryonUpdatable<UserEntity, IUserParams>.Update()
            {
                _updateRequest = new Dictionary<string, object>();
                return this;
            }

            ITalaryonRunner<UserEntity> ITalaryonDeletable<UserEntity>.Delete(bool force)
            {
                _deleteRequest = true;
                return this;
            }

            ITalaryonRunner<UserEntity> ITalaryonParams<UserEntity, IUserParams>.With(Action<IUserParams> withParams)
            {
                withParams(this);
                return this;
            }

            IUserParams IUserParams.Id(string userId)
            {
                // skipping - cannot modifiy id
                return this;
            }

            IUserParams IUserParams.Username(string name)
            {
                if (_createRequest is not null) _createRequest.Name = name;
                _updateRequest?.Add(nameof(name), name);
                return this;
            }

            IUserParams IUserParams.Password(string password)
            {
                if (_createRequest is not null) _createRequest.Password = password;
                _updateRequest?.Add(nameof(password), password);
                return this;
            }

            IUserParams IUserParams.Mail(string mail)
            {
                if (_createRequest is not null) _createRequest.Mail = mail;
                _updateRequest?.Add(nameof(mail), mail);
                return this;
            }

            IUserParams IUserParams.Name(string name)
            {
                if (_createRequest is not null) _createRequest.Name = name;
                _updateRequest?.Add(nameof(name), name);
                return this;
            }

            IUserParams IUserParams.IsEnabled(bool isEnabled)
            {
                if (_createRequest is not null) _createRequest.IsEnabled = isEnabled;
                _updateRequest?.Add(nameof(isEnabled), isEnabled);
                return this;
            }

            IUserParams IUserParams.IsAdmin(bool isAdmin)
            {
                if (_createRequest is not null) _createRequest.IsAdmin = isAdmin;
                _updateRequest?.Add(nameof(isAdmin), isAdmin);
                return this;
            }
        }

        class UserList : IRematoUsersService, IUserParams
        {
            private readonly RematoService _rematoService;
            private readonly UserEntity _entity;

            private int _take, _skip;
            private string _skipUntil;

            public UserList(RematoService rematoService)
            {
                _rematoService = rematoService;
                _entity = new UserEntity();
            }

            IEnumerable<UserEntity> ITalaryonRunner<IEnumerable<UserEntity>>.Run() =>
                (this as ITalaryonRunner<IEnumerable<UserEntity>>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<IEnumerable<UserEntity>> ITalaryonRunner<IEnumerable<UserEntity>>.RunAsync(
                CancellationToken cancellationToken)
            {
                var users = await _rematoService
                    .Database
                    .Many<UserEntity>()
                    .Where(filter => filter
                        .Is(nameof(UserEntity.IsDeleted))
                        .EqualTo(false)
                        .And()
                        // .Is(nameof(UserEntity.IsEnabled))
                        // .EqualTo(_entity.IsEnabled)
                        // .And()
                        // .Is(nameof(UserEntity.IsAdmin))
                        // .EqualTo(_entity.IsEnabled)
                        // .And()
                        .Clamp(pattern =>
                        {
                            if (_entity.Id is not null)
                            {
                                pattern
                                    .Is(nameof(UserEntity.Id))
                                    .Like(_entity.Id)
                                    .Or();
                            }

                            if (_entity.Username is not null)
                            {
                                pattern
                                    .Is(nameof(UserEntity.Username))
                                    .Like(_entity.Username)
                                    .Or();
                            }
                        })
                    )
                    .RunAsync(cancellationToken);

                return users
                    .Skip(_skip)
                    .SkipWhile(e => _skipUntil is not null && e.Id != _skipUntil)
                    .Take(_take)
                    .Select(v => (UserEntity)v);
            }

            ITalaryonRunner<int> ITalaryonCountable.Count() => _rematoService
                .Database
                .Count<UserEntity>();

            ITalaryonEnumerable<UserEntity, IUserParams> ITalaryonEnumerable<UserEntity, IUserParams>.Take(int count)
            {
                _take = count;
                return this;
            }

            ITalaryonEnumerable<UserEntity, IUserParams> ITalaryonEnumerable<UserEntity, IUserParams>.Skip(int count)
            {
                _skip = count;
                return this;
            }

            ITalaryonEnumerable<UserEntity, IUserParams> ITalaryonEnumerable<UserEntity, IUserParams>.SkipUntil(
                string cursor)
            {
                _skipUntil = cursor;
                return this;
            }

            ITalaryonEnumerable<UserEntity, IUserParams> ITalaryonEnumerable<UserEntity, IUserParams>.Where(
                Action<IUserParams> whereParams)
            {
                whereParams(this);
                return this;
            }

            IUserParams IUserParams.Id(string userId)
            {
                _entity.Id = userId;
                return this;
            }

            IUserParams IUserParams.Username(string username)
            {
                _entity.Username = username;
                return this;
            }

            IUserParams IUserParams.Password(string password)
            {
                _entity.Password = password;
                return this;
            }

            IUserParams IUserParams.Mail(string mail)
            {
                _entity.Mail = mail;
                return this;
            }

            IUserParams IUserParams.Name(string name)
            {
                _entity.Name = name;
                return this;
            }

            IUserParams IUserParams.IsEnabled(bool isEnabled)
            {
                _entity.IsEnabled = isEnabled;
                return this;
            }

            IUserParams IUserParams.IsAdmin(bool isAdmin)
            {
                _entity.IsAdmin = isAdmin;
                return this;
            }
        }
    }
}