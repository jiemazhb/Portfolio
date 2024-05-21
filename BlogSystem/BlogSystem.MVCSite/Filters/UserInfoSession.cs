using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Filters
{
    public class UserInfoSession: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //当cookie中有值，session中没有值的时候，将cookie中的值拷贝到session中
            if (filterContext.HttpContext.Request.Cookies["LoginName"] != null &&
                filterContext.HttpContext.Session["LoginName"] == null)
            {
                filterContext.HttpContext.Session["LoginName"] =
                    filterContext.HttpContext.Request.Cookies["LoginName"].Value;
                filterContext.HttpContext.Session["userId"] =
                    filterContext.HttpContext.Request.Cookies["userId"].Value;
            }
            //当cookie中有值，session中也有值的时候。将session中的值覆盖
            if (filterContext.HttpContext.Request.Cookies["LoginName"] != null &&
                filterContext.HttpContext.Session["LoginName"] != null)
            {
                filterContext.HttpContext.Session["LoginName"] =
                    filterContext.HttpContext.Request.Cookies["LoginName"].Value;
                filterContext.HttpContext.Session["userId"] =
                    filterContext.HttpContext.Request.Cookies["userId"].Value;
            }
        }
    }
}