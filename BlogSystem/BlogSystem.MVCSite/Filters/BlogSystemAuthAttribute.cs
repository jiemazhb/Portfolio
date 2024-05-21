using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSystem.MVCSite.Filters
{
    public class BlogSystemAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            //如果cookie和session中都为空。就跳到登录界面
            if (!(filterContext.HttpContext.Session["LoginName"] != null ||
                filterContext.HttpContext.Request.Cookies["LoginName"] != null))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    { "controller","Home"},
                    { "action","Login"} 
                });
            }
        }
    }
}