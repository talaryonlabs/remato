using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Remato.Shared
{
    public class RematoHelper
    {
        [Pure]
        public static string ToQueryString<T>(T data)
        {
            return string.Join("&", typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(v => v.CanRead)
                .Select(v =>
                {
                    var attr = v.GetCustomAttributes<QueryMemberAttribute>().FirstOrDefault();
                    var name = attr is not null ? attr.Name : v.Name;
                    var value = v.GetValue(data) ?? "";

                    return $"{name.ToLower()}={HttpUtility.UrlEncode(value.ToString())}";
                }));
        }
    }
}