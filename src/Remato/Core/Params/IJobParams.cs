using System;
using Remato.Shared;

namespace Remato
{
    public interface IJobParams
    {
        IJobParams Id(string vehicleId);
        IJobParams Title(string name);
        IJobParams Type(string type) => Type(Enum.Parse<RematoJobType>(type, true));
        IJobParams Type(RematoJobType type);
        IJobParams Date(DateTime dateTime);
        IJobParams IsDone(bool isDone);
    }
}