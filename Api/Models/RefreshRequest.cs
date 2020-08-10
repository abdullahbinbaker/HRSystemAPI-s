using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class RefreshRequest
    {
        public string JWTToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
