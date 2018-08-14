using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankApp.Filters
{
    public class DebugFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            var message = $"[OnActionExecuting] - {controllerName}.{actionName}";
            Debug.WriteLine(message);

            base.OnActionExecuting(filterContext);
        }

        
		public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            var message = $"[OnActionExecuted] - {controllerName}.{actionName}";
            Debug.WriteLine(message);

            base.OnActionExecuted(filterContext);
        }
    }
}
 