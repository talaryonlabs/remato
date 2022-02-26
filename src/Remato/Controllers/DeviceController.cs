using System.Threading;
using System.Linq;
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
    [ApiRoute("devices")]
    public class DeviceController
    {
        private readonly IRematoService _rematoService;

        public DeviceController(IRematoService rematoService)
        {
            _rematoService = rematoService;
        }

        [HttpGet]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoDeviceList))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoDeviceList> List([FromQuery] RematoDeviceListArgs listArgs,
            CancellationToken cancellationToken)
        {
            var count = await _rematoService
                .Devices()
                .Count()
                .RunAsync(cancellationToken);
            if (count == 0)
                return new RematoDeviceList();


            var list = (
                await _rematoService
                    .Devices()
                    .Skip(listArgs.Skip)
                    .SkipUntil(listArgs.Cursor)
                    .Take(listArgs.Limit)
                    .Where(whereParams => whereParams
                        .Id(listArgs.Id)
                        .Name(listArgs.Name)
                        .IsActive(listArgs.IsActive)
                    )
                    .RunAsync(cancellationToken)
            ).ToList();

            return !list.Any()
                ? new RematoDeviceList()
                : new RematoDeviceList()
                {
                    Items = list.Select(v => (RematoDevice) v),
                    NextCursor = list.Last().Id,
                    TotalCount = count
                };
        }

        [HttpPost]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoDevice))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoDevice> Create([FromBody] RematoRequest<RematoDevice> createRequest,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpGet("{deviceId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoDevice))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoDevice> View([FromRoute] string deviceId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPatch("{deviceId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoDevice))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoDevice> Update([FromRoute] string deviceId,
            [FromBody] RematoRequest<RematoDevice> updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpDelete("{deviceId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoDevice))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoDevice> Delete([FromRoute] string deviceId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }
    }
}