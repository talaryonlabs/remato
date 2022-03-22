using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remato.Shared;

namespace Remato.Controllers
{
    // [Authorize(Policy = RematoConstants.ManagementPolicy)]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiRoute("users")]
    public class UserController : Controller
    {
        private readonly IRematoService _rematoService;

        public UserController(IRematoService rematoService)
        {
            _rematoService = rematoService;
        }

        [HttpGet]
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
                        .Username(listArgs.Mail)
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoUser> Create([FromBody] RematoRequest<RematoUser> createRequest,
            CancellationToken cancellationToken) =>
            await _rematoService
                .User((string)createRequest.Items["username"])
                .Create()
                .With(createParams => createParams
                    .IsAdmin(false)
                    .IsEnabled(true)
                    .Name((string)createRequest.Items["name"])
                    .Password((string)createRequest.Items["password"])
                )
                .RunAsync(cancellationToken);

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoUser> View([FromRoute] string userId, CancellationToken cancellationToken) =>
            await _rematoService
                .User(userId)
                .RunAsync(cancellationToken);

        [HttpPatch("{userId}")] 
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoUser> Update([FromRoute] string userId,
            [FromBody] RematoRequest<RematoUser> updateRequest, CancellationToken cancellationToken)
        {
            return await _rematoService
                .User(userId)
                .Update()
                .With(updateParams =>
                {
                    if (updateRequest.Items.ContainsKey("username"))
                        updateParams.Username((string)updateRequest.Items["username"]);
                    
                    if (updateRequest.Items.ContainsKey("is_admin"))
                        updateParams.IsAdmin((bool)updateRequest.Items["is_admin"]);
                    
                    if (updateRequest.Items.ContainsKey("is_enabled"))
                        updateParams.IsEnabled((bool)updateRequest.Items["is_enabled"]);
                    
                    if (updateRequest.Items.ContainsKey("password"))
                        updateParams.Password((string)updateRequest.Items["password"]);
                })
                .RunAsync(cancellationToken);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoUser> Delete([FromRoute] string userId, CancellationToken cancellationToken) =>
            await _rematoService
                .User(userId)
                .Delete()
                .RunAsync(cancellationToken);
    }
}