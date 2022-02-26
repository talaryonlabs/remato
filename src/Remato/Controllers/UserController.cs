using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remato.Shared;

namespace Remato.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiRoute("users")]
    public class UserController
    {
        private readonly IRematoService _rematoService;

        public UserController(IRematoService rematoService)
        {
            _rematoService = rematoService;
        }

        [HttpGet]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUserList))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoUserList> List([FromQuery] RematoUserListArgs listArgs,
            CancellationToken cancellationToken)
        {
            var count = await _rematoService
                .Users()
                .Count()
                .RunAsync(cancellationToken);
            
            if (count == 0)
                return new RematoUserList();
            
            var list = (
                await _rematoService
                    .Users()
                    .Skip(listArgs.Skip)
                    .SkipUntil(listArgs.Cursor)
                    .Take(listArgs.Limit)
                    .Where(userParams => userParams
                        .Id(listArgs.Id)
                        .Username(listArgs.Username)
                        .IsAdmin(listArgs.IsAdmin)
                        .IsEnabled(listArgs.IsEnabled)
                    )
                    .RunAsync(cancellationToken)
            ).ToList();
            
            return !list.Any()
                ? new RematoUserList()
                : new RematoUserList()
                {
                    Items = list.Select(v => (RematoUser) v),
                    NextCursor = list.Last().Id,
                    TotalCount = count
                };
        }

        [HttpPost]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoUser> Create([FromBody] RematoRequest<RematoUser> createRequest,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoUser> View([FromRoute] string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPatch("{userId}")] 
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoUser> Update([FromRoute] string userId,
            [FromBody] RematoRequest<RematoUser> updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoUser> Delete([FromRoute] string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }
    }
}