using BlogSystem.BLL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            this._userManager = userManager;
        }

        public ActionResult GetCurrentUser()
        {
            return View();
        }
        public async Task<ActionResult> GetMostPopularUser() 
        {
            List<Followers> userWithFolloer = await _userManager.GetMostFolloerUser();
            return PartialView("_Interest", userWithFolloer);
        }
    }
}