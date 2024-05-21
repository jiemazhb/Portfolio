using BlogSystem.BLL;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.Shared;
using BlogSystem.MVCSite.Models.UserViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IArticleManager _articleManager;

        public HomeController(IUserManager userManager, IArticleManager articleManager)
        {
            _userManager = userManager;
            _articleManager = articleManager;
        }
        public ActionResult Index()
        {
            //var article = await _articleManager.GetArticleByLikeAsync();

            //return View(article);
            return View();

        }
        public ActionResult Register()
        {
            this.ViewBag.Title = "Sign Up";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(_userManager.GetUserByEmail(model.Email) != null)
                    {
                        this.ModelState.AddModelError("", "The email has already registered.");
                        return View();
                    }

                    string hashedPassword = HashPasswordWithSHA256(model.PassWord);

                    await _userManager.RegisterAsync(model.Email, hashedPassword, "null", model.NickName);
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {

                return View("Error", new ErrorViewModel { Message = e.Message, Source = e.Source,
                    Details = e.StackTrace});
            }

        }
        private string HashPasswordWithSHA256(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {

                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId;
                string nickName;
                string hashedPassword = HashPasswordWithSHA256(model.PassWord);
                bool data = _userManager.Login(model.Email, hashedPassword, out userId, out nickName);
                if (data)
                {   
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("LoginName")
                        {
                            Value = model.Email,
                            Expires = DateTime.Now.AddDays(7)
                        });
                        Response.Cookies.Add(new HttpCookie("userId")
                        {
                            Value = userId.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                        Response.Cookies.Add(new HttpCookie("NickName") 
                        {
                            Value = nickName,
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else   
                    {
                        Session["LoginName"] = model.Email;
                        Session["userId"] = userId;
                        Session["NickName"] = nickName;
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password was incorrect！");
                }
            }
            return View(model);
        }
        public ActionResult ExitLogin()
        {
            if (Session["LoginName"] != null || Session["userId"] != null || Session["NickName"] != null)
            {
                Session.Clear();
            }

            if (Request.Cookies["LoginName"] != null || Request.Cookies["userId"] !=null || Request.Cookies["NickName"] != null)
            {
                Response.Cookies["LoginName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["userId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["NickName"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult AboutMe()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
    }
}