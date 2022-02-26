using System;
using Microsoft.AspNetCore.Mvc;

namespace Remato.Shared
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryMemberAttribute : FromQueryAttribute
    {
        public QueryMemberAttribute(string memberName)
        {
            Name = memberName;
        }
    }
}