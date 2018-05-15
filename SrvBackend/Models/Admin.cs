using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SrvBackend.Models
{
    public class Admin
    {
        public Admin(IEnumerable<Claim> userClaims)
        {
            AdminId = userClaims.First(c => c.Type == "user_id").Value;
        }

        public string AdminId { get; set; }
    }
}