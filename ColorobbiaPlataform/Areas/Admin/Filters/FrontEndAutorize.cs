using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ColorobbiaPlataform.Areas.Admin.Filters
{
    public class FrontEndAutorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            object usuario = actionContext.HttpContext.Session["usuario"];
            if (usuario == null)
            {
                actionContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary(
                        new
                        {
                            area = "Admin",
                            controller = "Login",
                            action = "Index"
                        })
                    );
            }            
        }
    }
}