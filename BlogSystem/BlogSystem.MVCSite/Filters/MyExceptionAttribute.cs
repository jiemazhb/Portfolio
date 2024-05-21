using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebGrease;

namespace BlogSystem.MVCSite.Filters
{
    public class MyExceptionAttribute:HandleErrorAttribute
    {
        /// <summary>
        /// 用Nlog框架记录日志未完成。待续
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {

            if (!filterContext.ExceptionHandled)
            {
                //    controllerName, actionName, param, ip, filterContext.Exception.Message);
                //从新定向到某个页面
                //filterContext.Result = new RedirectResult("~/ErrorPage.html");
                filterContext.HttpContext.Response.Redirect("~/ErrorPage.html");
                filterContext.ExceptionHandled = true;

                //var controllerName = (string)filterContext.RouteData.Values["controller"];
                //var actionName = (string)filterContext.RouteData.Values["action"];
                //var param = Common.GetPostParas();
                //var ip = HttpContext.Current.Request.UserHostAddress;
                //LogManager.GetLogger("LogExceptionAttribute").Error("Location：{0}/{1} Param：{2}UserIP：{3} Exception：{4}",
                //filterContext.Result = new JsonResult()
                //{
                //    Data = new ReturnModel_Common { success = false, code = ReturnCode_Interface.服务端抛错, msg = filterContext.Exception.Message },
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};
            }
            //if (filterContext.Result is JsonResult)
            //{
            //    filterContext.ExceptionHandled = true;
            //}
            //else
            //{
            //    base.OnException(filterContext);
            //}
        }
    }
}