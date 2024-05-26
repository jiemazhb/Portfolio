using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.Models;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.ArticleViewModels;
using HtmlAgilityPack;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleManager _articleManager;
        public ArticleController(IArticleManager articleManager)
        {
            _articleManager = articleManager;
        }
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [BlogSystemAuth]
        public ActionResult CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(Session["userId"].ToString());
                _articleManager.CreateCategory(model.CategoryName, userId);
                return RedirectToAction(nameof(CategoryList));
            }
            return View(model);
        }

        public ActionResult CategoryList()
        {
            Guid userId = Guid.Parse(Session["userId"].ToString());
            var categoryList = _articleManager.GetAllCategories(userId);
            return View(categoryList);
        }
        public async Task<ActionResult> CategoryEdit(Guid? categoryId)
        {
            var boolVal = await _articleManager.IsCategoryExsitAsync(categoryId.Value);
            if (categoryId == null || !boolVal)
            {
                return RedirectToAction(nameof(CategoryList));
            }
            else
            {
                var category = await _articleManager.GetOneCategoryByIdAsync(categoryId.Value);
                return View(new CreateCategoryViewModel() { Id = category.Id, CategoryName = category.CategoryName });
            }
        }
        [HttpPost]
        public async Task<ActionResult> CategoryEdit(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _articleManager.EditCategory(model.Id, model.CategoryName);
                return RedirectToAction(nameof(CategoryList));
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ActionResult> CategoryDelete(Guid? categoryId)
        {
            if (categoryId == null || !await _articleManager.IsCategoryExsitAsync(categoryId.Value))
            {
                return RedirectToAction(nameof(CategoryList));
            }
            else
            {
                await _articleManager.RemoveCategory(categoryId.Value);
                return RedirectToAction(nameof(CategoryList));
            }
        }
        [BlogSystemAuth]
        public ActionResult CreateArticle()
        {
            return View();
        }

        private List<BlogCategoryDto> GetCategories()
        {
            Guid userId = Guid.Parse(Session["userId"].ToString());
            return _articleManager.GetAllCategories(userId);
        }

        [HttpPost]
        [BlogSystemAuth]
        [UserInfoSession]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //对输入的内容不做校验, 主要针对ckEditor中的内容
        public async Task<ActionResult> CreateArticle(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId = Guid.Parse(Session["userId"].ToString());
                await _articleManager.CreateArticleAsync(model.Title, model.Content, model.Category, userId);
                //var category = await _articleManager.GetOneCategoryByIdAsync(model.Category);
                //return RedirectToAction(nameof(ArticleList), new { categoryName = category.CategoryName });
                return RedirectToAction("ArticleList");
            }
            //ModelState.AddModelError("", "添加失败");
            return View(model);
        }
        //[HttpPost]
        //public async Task<ActionResult> UploadImage()
        //{
        //    if (Request.Files.Count > 0)
        //    {
        //        var file = Request.Files[0]; // 获取上传的文件
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);
        //            var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
        //            file.SaveAs(path);

        //            return Json(new { success = true, message = "File uploaded successfully", filePath = "/UploadedImages/" + fileName });
        //        }
        //    }

        //    return Json(new { success = false, message = "No file uploaded" });
        //}
        public async Task<ActionResult> ArticleListContainer(string searchStr, int pageindex = 1, int pageSize = 16)
        {
            try
            {
                int amount;
                ViewBag.SearchStr = searchStr;
                if (searchStr == null || searchStr == "")
                {
                    PagedList<ArticleDto> articleList =  _articleManager.GetAllArticles(pageindex, pageSize, out amount);

                    return View("ArticleListContainer", new PagedList<ArticleDto>(articleList, pageindex, pageSize, amount));
                }
                else
                {
                    pageSize = 4;
                    PagedList<ArticleDto> articles = _articleManager.GetAllArticlesBySearchStr(searchStr, pageindex, pageSize, out amount);

                    foreach (var article in articles)
                    {
                        var doc = new HtmlDocument();
                        doc.LoadHtml(article.Content);
                        var imageNode = doc.DocumentNode.SelectSingleNode("//img");
                        if (imageNode != null)
                        {
                            article.ImagePath = imageNode.Attributes["src"].Value;
                        }
                        else
                        {
                            article.ImagePath = "/Images/unnamed.png";
                        }
                    }
                    ViewBag.amount = amount;

                    return View("ArticleListContainer", new PagedList<ArticleDto>(articles, pageindex, pageSize, amount));
                }
            }
            catch (Exception)
            {
                return View("Error");
            }

        }

        private PagedList<ArticleDto> GetBrief(PagedList<ArticleDto> articleList)
        {
            string tempString;
            foreach (var article in articleList)
            {
                tempString = _articleManager.HtmlTagFilter(article.Content);
                tempString = _articleManager.InterceptString(tempString, 300);
                article.Content = tempString;
            }

            return articleList;
        }
        [ChildActionOnly]
        public ActionResult RecentPosts()
        {
            List<TopArticleDto> recentPosts = _articleManager.GetArticleByDateAsync();

            foreach (var article in recentPosts)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(article.Content);
                var imageNode = doc.DocumentNode.SelectSingleNode("//img");
                if (imageNode != null)
                {
                    article.picPath = imageNode.Attributes["src"].Value;
                }
                else
                {
                    article.picPath = "/Images/unnamed.png"; // 默认图片如果没有找到图片
                }
            }
            return PartialView("_RecentPosts", recentPosts);
        }
        public List<string> ExtractImageUrls(string htmlContent)
        {

            var urls = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var images = doc.DocumentNode.SelectNodes("//img[@src]");
            if (images != null)
            {
                foreach (var img in images)
                {
                    string src = img.GetAttributeValue("src", string.Empty);
                    if (!string.IsNullOrEmpty(src))
                    {
                        urls.Add(src);
                    }
                }
            }
            return urls;
        }

        public async Task<ActionResult> ArticleDetail(Guid? Id)
        {
            if (Id == null || !await _articleManager.IsExistAsync(Id.Value))
            {
                return RedirectToAction(nameof(ArticleList));
            }
            else
            {
                ViewBag.comments = await _articleManager.GetCommentByArticleIdAsync(Id.Value);
                var article = await _articleManager.GetOneArticleByIdAsync(Id.Value);
                return View(article);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ArticleEdit(EditArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _articleManager.EditArticle(model.Id, model.Title, model.Content, model.CategoriesId);
                return RedirectToAction(nameof(ArticleList));
            }
            ViewBag.CategoryIds = GetCategories();
            return View(model);
        }
        public async Task<ActionResult> DeleteArticleAsync(Guid? articleId)
        {
            var category = await _articleManager.GetCategoryByArticleIdAsync(articleId.Value);

            await _articleManager.RemoveArticleAsync(articleId.Value);
            return RedirectToAction(nameof(ArticleList), (Object)category.CategoryName);
        }
        [HttpPost]
        [UserInfoSession]
        public async Task<ActionResult> GoodCount(Guid id)
        {

            var tempStr = Session["userId"];
            if (tempStr == null)
            {
                return Json(new { result = "error" });
            }
            else
            {
                var email = tempStr.ToString();
                var likeNumber = await _articleManager.GoodCountAsync(id, email);
                return Json(likeNumber);
            }
        }
        [HttpPost]
        [UserInfoSession]
        public async Task<ActionResult> BadCount(Guid articleid)
        {
            var tempStr = Session["LoginName"];
            if (tempStr == null)
            {
                return Json(new { result = "error" });
            }
            else
            {
                var email = tempStr.ToString();
                var dislikeNumber = await _articleManager.BadCountAsync(articleid, email);
                return Json(dislikeNumber);
            }
        }
        [UserInfoSession]
        public async Task<JsonResult> AddComment(CreateCommentViewModel model)
        {
            var userId = Session["userId"];
            if (userId == null)
            {
                return Json(new { result = "error" });
            }
            else
            {
                var uId = Guid.Parse(userId.ToString());
                await _articleManager.CreateCommentAsync(model.ArticleId, model.Content, uId);
                return Json(new { result = "ok" });
            }
        }

        public async Task<ActionResult> GetTopFourArticle()
        {
            var topArticle = await _articleManager.GetTopArticles();

            foreach (var article in topArticle)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(article.Content);
                var imageNode = doc.DocumentNode.SelectSingleNode("//img");
                if (imageNode != null)
                {
                    article.picPath = imageNode.Attributes["src"].Value;
                }
                else
                {
                    article.picPath = "/Images/unnamed.png"; // 默认图片如果没有找到图片
                }
            }
                return PartialView("_GetTopFourArticle", topArticle);
        }
        public async Task<JsonResult> GetOneArticleOfTopFive(Guid? articleId)
        {
            var article = await _articleManager.GetOneArticleByArticleIdAsync(articleId.Value);
            return Json(article, JsonRequestBehavior.AllowGet);
        }

    }
}