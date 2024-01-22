using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Core.DTOs.Login
{
    public  class GetAccessTokenDTO
    {
        public string RefreshToken { get; set;}
    }
}
