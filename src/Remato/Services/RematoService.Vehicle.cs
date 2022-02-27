using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Remato.Shared;
using Talaryon;

namespace Remato.Services
{
    public partial class RematoService
    {
        private class VehicleItem : IRematoServiceVehicle, IVehicleParams, ITalaryonRunner<bool>, ITalaryonParams<VehicleEntity, IVehicleParams>
        {
            private readonly RematoService _rematoService;
            private readonly string _vehicleIdOrName;

            private VehicleEntity _createRequest;
            private Dictionary<string, object> _updateRequest;
            private bool _deleteRequest;

            public VehicleItem(RematoService rematoService, string vehicleIdOrName)
            {
                _rematoService = rematoService;
                _vehicleIdOrName = vehicleIdOrName;
            }

            VehicleEntity ITalaryonRunner<VehicleEntity>.Run() => (this as ITalaryonRunner<VehicleEntity>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<VehicleEntity> ITalaryonRunner<VehicleEntity>.RunAsync(CancellationToken cancellationToken)
            {
                var cachedItem = await _rematoService
                    .Cache
                    .Key<VehicleEntity>(RematoCaching.GetVehicleKey(_vehicleIdOrName))
                    .RunAsync(cancellationToken);
                
                var entityItem = cachedItem ?? await _rematoService
                    .Database
                    .First<VehicleEntity>()
                    .Where(filter => filter
                        .Is(nameof(VehicleEntity.Id))
                        .EqualTo(_vehicleIdOrName)
                        .Or()
                        .Is(nameof(VehicleEntity.Name))
                        .EqualTo(_vehicleIdOrName)
                    )
                    .RunAsync(cancellationToken);
                
                if (_createRequest is not null)
                {
                    if (entityItem is not null)
                        throw new VehicleAlreadyExistsError(entityItem);
                    
                    // ReSharper disable once HeuristicUnreachableCode
                    entityItem = await _rematoService
                        .Database
                        .Insert(_createRequest)
                        .RunAsync(cancellationToken);
                }
                
                if (entityItem is null)
                    throw new VehicleNotFoundError();
                
                if (_updateRequest is not null)
                {
                    if (_updateRequest.ContainsKey("name"))
                        entityItem.Name = (string) _updateRequest["name"];

                    if (_updateRequest.ContainsKey("state"))
                        entityItem.State = (RematoVehicleState) _updateRequest["state"];
                    
                    if (_updateRequest.ContainsKey("vin"))
                        entityItem.VIN = (string) _updateRequest["vin"];
                    
                    if (_updateRequest.ContainsKey("licensePlate"))
                        entityItem.LicensePlate = (string) _updateRequest["licensePlate"];
                    
                    if (_updateRequest.ContainsKey("startDate"))
                        entityItem.StartDate = (DateTime) _updateRequest["startDate"];
                    
                    if (_updateRequest.ContainsKey("endTime"))
                        entityItem.EndDate = (DateTime) _updateRequest["endTime"];
                    
                    await Task.WhenAll(
                        _rematoService
                            .Cache
                            .RemoveMany(new[]
                            {
                                RematoCaching.GetVehicleKey(entityItem.Id),
                                RematoCaching.GetVehicleKey(entityItem.Name)
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
                    await Task.WhenAll(
                        _rematoService
                            .Cache
                            .RemoveMany(new[]
                            {
                                RematoCaching.GetVehicleKey(entityItem.Id),
                                RematoCaching.GetVehicleKey(entityItem.Name)
                            })
                            .RunAsync(cancellationToken),
                        _rematoService
                            .Database
                            .Delete(entityItem)
                            .RunAsync(cancellationToken)
                    );
                }
                else if (cachedItem is null)
                {
                    await Task.WhenAll(
                        _rematoService
                            .Cache
                            .Key<VehicleEntity>(RematoCaching.GetVehicleKey(entityItem.Id))
                            .Set(entityItem)
                            .RunAsync(cancellationToken),
                        _rematoService
                            .Cache
                            .Key<VehicleEntity>(RematoCaching.GetVehicleKey(entityItem.Name))
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
                    await (this as ITalaryonRunner<VehicleEntity>).RunAsync(cancellationToken);
                }
                catch (VehicleNotFoundError)
                {
                    return false;
                }
                return true;
            }

            
            ITalaryonParams<VehicleEntity, IVehicleParams> ITalaryonCreatable<VehicleEntity, IVehicleParams>.Create()
            {
                _createRequest = new VehicleEntity()
                {
                    Id = TalaryonHelper.UUID()
                };
                return this;
            }

            ITalaryonParams<VehicleEntity, IVehicleParams> ITalaryonUpdatable<VehicleEntity, IVehicleParams>.Update()
            {
                _updateRequest = new Dictionary<string, object>();
                return this;
            }

            ITalaryonRunner<VehicleEntity> ITalaryonDeletable<VehicleEntity>.Delete(bool force)
            {
                _deleteRequest = true;
                return this;
            }

            ITalaryonRunner<VehicleEntity> ITalaryonParams<VehicleEntity, IVehicleParams>.With(Action<IVehicleParams> withParams)
            {
                withParams(this);
                return this;
            }

            IVehicleParams IVehicleParams.Id(string vehicleId)
            {
                // skipping - cannot modifiy id
                return this;
            }

            IVehicleParams IVehicleParams.Name(string name)
            {
                if (_createRequest is not null) _createRequest.Name = name;
                _updateRequest?.Add(nameof(name), name);
                return this;
            }

            IVehicleParams IVehicleParams.State(RematoVehicleState state)
            {
                if (_createRequest is not null) _createRequest.State = state;
                _updateRequest?.Add(nameof(state), state);
                return this;
            }

            IVehicleParams IVehicleParams.LicensePlate(string licensePlate)
            {
                if (_createRequest is not null) _createRequest.LicensePlate = licensePlate;
                _updateRequest?.Add(nameof(licensePlate), licensePlate);
                return this;
            }

            IVehicleParams IVehicleParams.VIN(string vin)
            {
                if (_createRequest is not null) _createRequest.VIN = vin;
                _updateRequest?.Add(nameof(vin), vin);
                return this;
            }

            IVehicleParams IVehicleParams.StartDate(DateTime startDate)
            {
                if (_createRequest is not null) _createRequest.StartDate = startDate;
                _updateRequest?.Add(nameof(startDate), startDate);
                return this;
            }

            IVehicleParams IVehicleParams.EndDate(DateTime endDate)
            {
                if (_createRequest is not null) _createRequest.EndDate = endDate;
                _updateRequest?.Add(nameof(endDate), endDate);
                return this;
            }
        }

        class VehicleList : IRematoServiceVehicles, IVehicleParams
        {
            private readonly RematoService _rematoService;
            private readonly VehicleEntity _entity;

            private int _take, _skip;
            private string _skipUntil;

            public VehicleList(RematoService rematoService)
            {
                _rematoService = rematoService;
                _entity = new VehicleEntity();
            }

            IEnumerable<VehicleEntity> ITalaryonRunner<IEnumerable<VehicleEntity>>.Run() =>
                (this as ITalaryonRunner<IEnumerable<VehicleEntity>>)
                .RunAsync()
                .RunSynchronouslyWithResult();

            async Task<IEnumerable<VehicleEntity>> ITalaryonRunner<IEnumerable<VehicleEntity>>.RunAsync(CancellationToken cancellationToken)
            {
                var users = await _rematoService
                    .Database
                    .Many<VehicleEntity>()
                    .Where(filter => filter
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
                                    .Is(nameof(VehicleEntity.Id))
                                    .Like(_entity.Id)
                                    .Or();
                            }

                            if (_entity.Name is not null)
                            {
                                pattern
                                    .Is(nameof(VehicleEntity.Name))
                                    .Like(_entity.Name)
                                    .Or();
                            }
                        })
                    )
                    .RunAsync(cancellationToken);

                return users
                    .Skip(_skip)
                    .SkipWhile(e => _skipUntil is not null && e.Id != _skipUntil)
                    .Take(_take)
                    .Select(v => (VehicleEntity) v);
            }

            ITalaryonRunner<int> ITalaryonCountable.Count() => _rematoService
                .Database
                .Count<UserEntity>();

            ITalaryonEnumerable<VehicleEntity, IVehicleParams> ITalaryonEnumerable<VehicleEntity, IVehicleParams>.Take(int count)
            {
                _take = count;
                return this;
            }

            ITalaryonEnumerable<VehicleEntity, IVehicleParams> ITalaryonEnumerable<VehicleEntity, IVehicleParams>.Skip(int count)
            {
                _skip = count;
                return this;
            }

            ITalaryonEnumerable<VehicleEntity, IVehicleParams> ITalaryonEnumerable<VehicleEntity, IVehicleParams>.SkipUntil(string cursor)
            {
                _skipUntil = cursor;
                return this;
            }

            ITalaryonEnumerable<VehicleEntity, IVehicleParams> ITalaryonEnumerable<VehicleEntity, IVehicleParams>.Where(Action<IVehicleParams> whereParams)
            {
                whereParams(this);
                return this;
            }

            IVehicleParams IVehicleParams.Id(string vehicleId)
            {
                _entity.Id = vehicleId;
                return this;
            }

            IVehicleParams IVehicleParams.Name(string name)
            {
                _entity.Name = name;
                return this;
            }

            IVehicleParams IVehicleParams.LicensePlate(string licensePlate)
            {
                _entity.LicensePlate = licensePlate;
                return this;
            }

            IVehicleParams IVehicleParams.VIN(string vin)
            {
                _entity.VIN = vin;
                return this;
            }

            IVehicleParams IVehicleParams.StartDate(DateTime startDate)
            {
                _entity.StartDate = startDate;
                return this;
            }

            IVehicleParams IVehicleParams.EndDate(DateTime endDate)
            {
                _entity.EndDate = endDate;
                return this;
            }
        }
    }
}