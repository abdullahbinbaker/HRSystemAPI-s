using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class LoginRequest
    {
        public string EmailAddress { get; set; }
        public String Password { get; set; }
    }
}
