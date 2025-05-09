﻿using BlogSystem.BLL;
using BlogSystem.Dto;
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
        public async Task<ActionResult> Index()
        {
            WebSiteStatistic webSiteStatistic = new WebSiteStatistic();

            Task userStatsTask = _userManager.StatisticsUserDataAsync(webSiteStatistic);
            Task articleStatsTask = _articleManager.StatisticsArticleDataAsync(webSiteStatistic);

            await Task.WhenAll(userStatsTask, articleStatsTask);

            return View(webSiteStatistic);

        }
        //public async Task<ActionResult> GetSiteStatistics()
        //{
        //    WebSiteStatistic webSiteStatistic = new WebSiteStatistic();

        //    Task userStatsTask = _userManager.StatisticsUserDataAsync(webSiteStatistic);
        //    Task articleStatsTask = _articleManager.StatisticsArticleDataAsync(webSiteStatistic);

        //    await Task.WhenAll(userStatsTask, articleStatsTask);

        //    return Json(webSiteStatistic, JsonRequestBehavior.AllowGet);
        //}
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
                        return View(model);
                    }

                    string filePath = "/Images/avatar/human.png";

                    if (model.IconPath != null && model.IconPath.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(model.IconPath.FileName);
                        string savePath = Path.Combine(Server.MapPath("/Images/avatar"), fileName);
                        model.IconPath.SaveAs(savePath);

                        filePath = "/Images/avatar/" + fileName;
                    }

                    string hashedPassword = HashPasswordWithSHA256(model.PassWord);

                    await _userManager.RegisterAsync(model.Email, hashedPassword, filePath, model.NickName);
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
                ClearCookieSession();

                Guid userId;
                string nickName;
                string avatarPath;
                string hashedPassword = HashPasswordWithSHA256(model.PassWord);
                bool data = _userManager.Login(model.Email, hashedPassword, out userId, out nickName, out avatarPath);
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
                        Response.Cookies.Add(new HttpCookie("AvatarPath")
                        {
                            Value = avatarPath,
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else   
                    {
                        Session["LoginName"] = model.Email;
                        Session["userId"] = userId;
                        Session["NickName"] = nickName;
                        Session["AvatarPath"] = avatarPath;
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
        public void ClearCookieSession()
        {
            if (Session["LoginName"] != null || Session["userId"] != null || Session["NickName"] != null || Session["AvatarPath"] != null)
            {
                Session.Clear();
            }

            if (Request.Cookies["LoginName"] != null || Request.Cookies["userId"] != null || Request.Cookies["NickName"] != null || Response.Cookies["AvatarPath"] != null)
            {
                Response.Cookies["LoginName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["userId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["NickName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["AvatarPath"].Expires = DateTime.Now.AddDays(-1);
            }
        }
        public ActionResult ExitLogin()
        {
            ClearCookieSession();
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