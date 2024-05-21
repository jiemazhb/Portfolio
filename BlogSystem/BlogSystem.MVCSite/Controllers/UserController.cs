using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult GetCurrentUser()
        {
            return View();
        }
    }
}