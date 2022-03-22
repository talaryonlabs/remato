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
    [ApiRoute("trainees")]
    public class TraineeController : Controller
    {
        private readonly IRematoService _rematoService;

        public TraineeController(IRematoService rematoService)
        {
            _rematoService = rematoService;
        }

        [HttpGet]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoTraineeList))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoTraineeList> List([FromQuery] RematoTraineeListArgs listArgs,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpPost]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoTrainee))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoTrainee> Create([FromBody] RematoRequest<RematoTrainee> createRequest,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpGet("{traineeId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoTrainee))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoTrainee> View([FromRoute] string traineeId, CancellationToken cancellationToken) =>
            await _rematoService
                .Trainee(traineeId)
                .RunAsync(cancellationToken);

        [HttpPatch("{traineeId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoTrainee))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        public async Task<RematoTrainee> Update([FromRoute] string traineeId,
            [FromBody] RematoRequest<RematoTrainee> updateRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedError();
        }

        [HttpDelete("{traineeId}")]
        [Authorize(Policy = RematoConstants.ManagementPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RematoTrainee))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(InternalServerError))]
        [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(NotImplementedError))]
        public async Task<RematoTrainee> Delete([FromRoute] string traineeId, CancellationToken cancellationToken) =>
            await _rematoService
                .Trainee(traineeId)
                .Delete()
                .RunAsync(cancellationToken);
    }
}