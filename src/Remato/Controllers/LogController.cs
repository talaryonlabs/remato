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
    [ApiRoute("logs")]
    public class LogController : Controller
    {
        [HttpGet]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoLogList))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoLogList> List([FromQuery] RematoLogListArgs listArgs,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPost]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoLog))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoLog> Create([FromBody] RematoRequest<RematoLog> createRequest,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpGet("{logId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoLog))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoLog> View([FromRoute] string logId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPatch("{logId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoLog))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoLog> Update([FromRoute] string logId,
            [FromBody] RematoRequest<RematoLog> updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpDelete("{logId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoLog))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoLog> Delete([FromRoute] string logId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }
    }
}