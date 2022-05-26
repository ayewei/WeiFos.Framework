using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiFos.Res.UEditor;

namespace WeiFos.Res.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class UEController : Controller
    {
        private UEditorService _uEditor;
        public UEController(UEditorService uEditor)
        {
            this._uEditor = uEditor;
        }

        public void Do()
        {
            this._uEditor.DoAction(HttpContext);
        }
    }
}
