using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using WeiFos.Res.UEditor.Handlers;

namespace WeiFos.Res.UEditor
{
    public class UEditorService
    {
        private UEditorActionCollection actionList;

        //已过时
        //public UEditorService(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, UEditorActionCollection actions)
        //{
        //    Config.WebRootPath = env.WebRootPath;
        //    actionList = actions;
        //}

        public UEditorService(IWebHostEnvironment env, UEditorActionCollection actions)
        {
            Config.WebRootPath = env.WebRootPath;
            actionList = actions;
        }

        public void DoAction(HttpContext context)
        {
            var action = context.Request.Query["action"];
            if (actionList.ContainsKey(action))
                actionList[action].Invoke(context);
            else
                new NotSupportedHandler(context).Process();
        }
    }
}
