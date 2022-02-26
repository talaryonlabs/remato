﻿using System;
using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public class TokenExpiredError : UnauthorizedError
    {
        [DataMember(Name = "expired_at")] public DateTime ExpiredAt { get; set; }
        
        public TokenExpiredError() 
            : base("Token is expired! Login again to retrive a new token.")
        {
        }
    }
}