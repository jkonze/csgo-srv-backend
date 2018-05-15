using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace SrvBackend.Models
{
    public class ServerAdmin
    {
        private readonly ClaimsIdentity _identity;

        public ServerAdmin(IIdentity identity)
        {
            _identity = identity as ClaimsIdentity;
        }

        public string Password => _identity?.FindFirst("Password").Value;

        public IPAddress ServerIp => IPAddress.Parse(_identity.FindFirst("ServerIp").Value);

        public ushort ServerPort => Convert.ToUInt16(_identity?.FindFirst("ServerPort").Value);
    }
}