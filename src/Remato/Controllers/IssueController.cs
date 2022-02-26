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
    [ApiRoute("issues")]
    public class IssueController
    {
        [HttpGet]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoIssueList))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoIssueList> List([FromQuery] RematoIssueListArgs listArgs,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPost]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoIssue))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoIssue> Create([FromBody] RematoRequest<RematoIssue> createRequest,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpGet("{issueId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoIssue))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoIssue> View([FromRoute] string issueId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPatch("{issueId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoIssue))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoIssue> Update([FromRoute] string issueId,
            [FromBody] RematoRequest<RematoIssue> updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpDelete("{issueId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoIssue))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoIssue> Delete([FromRoute] string issueId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }
    }
}