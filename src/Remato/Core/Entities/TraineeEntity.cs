using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableTrainee)]
    public class TraineeEntity
    {
        [ExplicitKey] public string Id { get; set; }

        public static implicit operator RematoTrainee([NotNull] TraineeEntity entity) => new()
        {
            TraineeId = entity.Id,
        };
        
        public static implicit operator TraineeEntity([NotNull] RematoTrainee trainee) => new()
        {
            Id = trainee.TraineeId
        };
    }
}