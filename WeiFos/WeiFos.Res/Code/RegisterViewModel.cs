using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeiFos.Res.Code
{
    public class RegisterViewModel
    {
        // other properties omitted
        public IFormFile AvatarImage { get; set; }
    }
}
