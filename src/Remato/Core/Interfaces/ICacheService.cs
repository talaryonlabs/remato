using System.Collections.Generic;
using Talaryon;

namespace Remato
{
    public interface ICacheService
    {
        ICacheServiceEntry<T> Key<T>(string key);
        ICacheServiceEntry<object> Key(string key);
        ITalaryonRunner RemoveMany(IEnumerable<string> keys);
    }
    
    public interface ICacheServiceEntry<T> :
        ITalaryonRunner<T>,
        ITalaryonExistable,
        ITalaryonDeletable
    {
        ITalaryonRunner Set(T value);
        ITalaryonRunner Refresh(T value);
    }
}