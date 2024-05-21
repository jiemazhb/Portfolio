using BlogSystem.BLL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.ArticleViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
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
        /// <summary>
        /// orderType代表排序类型，1热度， 2，最多评论 3.发表日期
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public async Task<ActionResult> ArticleList(int pageindex = 1, int pageSize = 16, int orderType = 3)
        {
            
            //当前用户第n页数据
            PagedList<ArticleList> articleList = _articleManager.GetAllArticles(pageindex, pageSize);

            //articleList = GetBrief(articleList);

            //当前用户文章总数
            //var total = await _articleManager.GetTotalOfArticlesAsync(categoryName);
            //ViewData["Title"] = categoryName;
            return View(new PagedList<ArticleList>(articleList, pageindex, pageSize, 18));
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

        /// <summary>
        /// 如果函数中得参数是可空的，那么在使用的时候一定要.value才行。 举例：ArticleDetail(Guid? articleId)
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ActionResult> ArticleDetail(Guid? articleId)
        {
            if (articleId == null || !await _articleManager.IsExistAsync(articleId.Value))
            {
                return RedirectToAction(nameof(ArticleList));
            }
            else
            {
                ViewBag.comments = await _articleManager.GetCommentByArticleIdAsync(articleId.Value);
                var article = await _articleManager.GetOneArticleByIdAsync(articleId.Value);
                return View(article);
            }
        }

        public async Task<ActionResult> ArticleEdit(Guid? articleId)
        {
            if (articleId == null || !await _articleManager.IsExistAsync(articleId.Value))
            {
                return RedirectToAction(nameof(ArticleList));
            }
            else
            {
                var article = await _articleManager.GetOneArticleByIdAsync(articleId.Value);

                ViewBag.CategoryIds = GetCategories();

                var CurrentArticle = new EditArticleViewModel() {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    CategoriesId = article.CategoryId
                };

                return View(CurrentArticle);
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
            var tempStr = Session["LoginName"];
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
            var tempStr = Session["userId"];         
            if (tempStr == null)
            {
                return Json(new { result = "error" });
            }
            else
            {
                var userId = Guid.Parse(tempStr.ToString());
                await _articleManager.CreateCommentAsync(model.Id, userId, model.Content);
                return Json(new { result = "ok" });
            }
        }
        public async Task<ActionResult> GetTopFiveArticle()
        {
            var topArticle = await _articleManager.GetTopArticles();
            return PartialView("_GetTopFiveArticle", topArticle);
        }
        public async Task<JsonResult> GetOneArticleOfTopFive(Guid? articleId)
        {
            var article = await _articleManager.GetOneArticleByArticleIdAsync(articleId.Value);
            return Json(article, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchArticle(string searchStr, int currentPage = 1, int pageSize = 4) 
        {
            int amount;
            var articles = _articleManager.GetAllArticlesBySearchStr(searchStr, currentPage, pageSize,out amount);
            ViewBag.amount = amount;
            articles = GetBrief(articles);
            return View(articles);
        }
    }
}