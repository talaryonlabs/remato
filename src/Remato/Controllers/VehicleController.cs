using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remato.Shared;

namespace Remato.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiRoute("vehicles")]
    public class VehicleController : Controller
    {
        private readonly IRematoService _rematoService;

        public VehicleController(IRematoService rematoService)
        {
            _rematoService = rematoService;
        }

        [HttpGet]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoVehicleList))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoVehicleList> List([FromQuery] RematoVehicleListArgs listArgs,
            CancellationToken cancellationToken)
        {
            var count = await _rematoService
                .Vehicles()
                .Count()
                .RunAsync(cancellationToken);
            if (count == 0)
                return new RematoVehicleList();

            var list = (
                await _rematoService
                    .Vehicles()
                    .Skip(listArgs.Skip)
                    .SkipUntil(listArgs.Cursor)
                    .Take(listArgs.Limit)
                    .Where(whereParams => whereParams
                        .Id(listArgs.Id)
                        .Name(listArgs.Name)
                    )
                    .RunAsync(cancellationToken)
            ).ToList();

            return !list.Any()
                ? new RematoVehicleList()
                : new RematoVehicleList()
                {
                    Items = list.Select(v => (RematoVehicle) v),
                    NextCursor = list.Last().Id,
                    TotalCount = count
                };
        }

        [HttpPost]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoVehicle))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoVehicle> Create([FromBody] RematoRequest<RematoVehicle> createRequest,
            CancellationToken cancellationToken) =>
            await _rematoService
                .Vehicle((string)createRequest.Items["name"])
                .Create()
                .With(createParams =>
                {
                    if (createRequest.Items.ContainsKey("state"))
                        createParams.State((string)createRequest.Items["state"]);
                    
                    // TODO create vehicle
                    // createParams.StartDate((DateTime)createRequest.Items["startDate"]);
                })
                .RunAsync(cancellationToken);

        [HttpGet("{vehicleId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoVehicle))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoVehicle> View([FromRoute] string vehicleId, CancellationToken cancellationToken) =>
            await _rematoService
                .Vehicle(vehicleId)
                .RunAsync(cancellationToken);

        [HttpPatch("{vehicleId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoVehicle))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoVehicle> Update([FromRoute] string vehicleId,
            [FromBody] RematoRequest<RematoVehicle> updateRequest, CancellationToken cancellationToken) =>
            await _rematoService
                .Vehicle(vehicleId)
                .Update()
                .With(updateParams =>
                {
                    // TODO update vehicle
                    if (updateRequest.Items.ContainsKey("name"))
                        updateParams.Name((string)updateRequest.Items["name"]);
                })
                .RunAsync(cancellationToken);

        [HttpDelete("{vehicleId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoVehicle))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoVehicle> Delete([FromRoute] string vehicleId, CancellationToken cancellationToken) =>
            await _rematoService
                .Vehicle(vehicleId)
                .Delete()
                .RunAsync(cancellationToken);
    }
}