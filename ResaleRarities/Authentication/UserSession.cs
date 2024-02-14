using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResaleRarities.Authentication
{
    public class UserSession
    {
        public string FullName { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
