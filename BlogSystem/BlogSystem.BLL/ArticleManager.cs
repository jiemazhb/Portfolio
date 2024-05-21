using BlogSystem.DAL;
using BlogSystem.Dto;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.BLL
{
    public class ArticleManager : IArticleManager
    {
        private readonly IArticleService _articleService;
        private readonly IArticleToCategoryService _articleToCategoryService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly ICommentService _commentService;

        public ArticleManager(IArticleService articleService, IArticleToCategoryService articleToCategoryService , IBlogCategoryService blogCategoryService, ICommentService commentService)
        {
            _articleService = articleService;
            _articleToCategoryService = articleToCategoryService;
            _blogCategoryService = blogCategoryService;
            _commentService = commentService;

        }

        public PagedList<ArticleList> GetAllArticles(int CurrentPage, int pageSize)
        {
            var res = _articleService.GetAll().Select(a => new ArticleList()
            {
                Title = a.Title,
                BadCount = a.BadCount,
                Category = a.category,
                CreateTime = a.CreateTime,
                GoodCount = a.GoodCount,
                Id = a.Id,
                UserId = a.UserId,
                UserName = a.User.NickName,
            }).OrderBy(a => a.CreateTime).ToPagedList(CurrentPage, pageSize);
            
            return res;    
        }
        public PagedList<ArticleDto> GetAllArticlesBySearchStr(string searchStr, int CurrentPage, int pageSize, out int amount)
        {
            amount = _articleService.GetAll().Where(m => m.Title.Contains(searchStr) == true || m.Content.Contains(searchStr)).Count();
            var articles = _articleToCategoryService.GetAll().Include(m => m.BlogCategory).Include(m => m.Article).Where(m => m.Article.Title.Contains(searchStr) == true || m.Article.Content.Contains(searchStr));
            var art = articles.Select(m => new ArticleDto()
            {
                Title = m.Article.Title,
                Content = m.Article.Content,
                BadCount = m.Article.BadCount,
                GoodCount = m.Article.GoodCount,
                CreateTime = m.Article.CreateTime,
                Id = m.Article.Id,
                ImagePath = m.Article.User.ImagePath,
                Email = m.Article.User.Email,
                NickName = m.Article.User.NickName
            }).OrderByDescending(m => m.CreateTime).ToList().ToPagedList(CurrentPage, pageSize);

            foreach (var article in art)
            {
                var category = _articleToCategoryService.GetAll().Include(m => m.BlogCategory).Where(m => m.ArticleId == article.Id).ToList();
                article.CategoryId = category.Select(m => m.BlogCategoryId).ToArray();
                article.CategoryNames = category.Select(m => m.BlogCategory.CategoryName).ToArray();
            }
            return art;
        }
        public async Task<ArticleDtoNew> GetOneArticleByArticleIdAsync(Guid articleId)
        {
            var article = await _articleToCategoryService.GetAll().Include(m => m.Article).Include(n => n.BlogCategory)
                .Where(a => a.Article.Id == articleId)
                .Select(b => new ArticleDtoNew()
                {
                    Id = b.ArticleId,
                    Title = b.Article.Title,
                    Content = b.Article.Content,
                    CreateTime = b.Article.CreateTime,
                    Email = b.Article.User.Email,
                    GoodCount = b.Article.GoodCount,
                    BadCount = b.Article.BadCount,
                    ImagePath = b.Article.User.ImagePath,
                    CategoryId = b.BlogCategoryId,
                    CategoryName = b.BlogCategory.CategoryName,
                    UserId = b.Article.UserId,
                    NickName = b.Article.User.NickName
                }).FirstAsync();

            return article;
        }
        public async Task<List<TopArticleDto>> GetTopArticles()
        {
            return await _articleToCategoryService.GetAll().Include(m => m.Article).Include(n => n.BlogCategory)
                .OrderByDescending(a => a.Article.GoodCount)
                .Select(b => new TopArticleDto() 
                { 
                    Id = b.ArticleId,
                    Title = b.Article.Title,
                    CategoryId = b.BlogCategoryId,
                    CategoryName = b.BlogCategory.CategoryName,
                    Userid = b.Article.UserId,
                    NickName = b.Article.User.NickName
                }).Take(5).ToListAsync();
        }
        /// <summary>
        /// get the article that most people like.
        /// </summary>
        public async Task<ArticleDtoNew> GetArticleByLikeAsync()
        {
            int? maxValue;
            try
            {
                maxValue = await _articleService.GetAll().MaxAsync(m => m.GoodCount);

                return await _articleToCategoryService.GetAll().Include(m => m.Article).Include(n => n.BlogCategory)
                    .Include(b => b.Article.User).Where(a => a.Article.GoodCount == maxValue)
                    .Select(c => new ArticleDtoNew()
                    {
                        Id = c.Article.Id,
                        Title = c.Article.Title,
                        Content = c.Article.Content,
                        CreateTime = c.Article.CreateTime,
                        Email = c.Article.User.Email,
                        GoodCount = c.Article.GoodCount,
                        BadCount = c.Article.BadCount,
                        ImagePath = c.Article.User.ImagePath,
                        CategoryId = c.BlogCategory.Id,
                        CategoryName = c.BlogCategory.CategoryName
                    }).FirstAsync();
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task BadCountAsync(Guid articleId)
        {
            var article = await _articleService.GetOneByIdAsync(articleId);
            article.BadCount++;
            await _articleService.EditAsync(article);

        }
        public async Task<int> BadCountAsync(Guid articleId, string email)
        {
            var article = await _articleService.GetOneByIdAsync(articleId);
            if (article.DislikeUserInfo == null || article.DislikeUserInfo.Contains(email)  == false)
            {
                article.BadCount++;
                article.DislikeUserInfo = article.DislikeUserInfo +email+"-";
                await _articleService.EditAsync(article);
                return article.BadCount;
            }
            else
            {
                return article.BadCount;
            }
        }
        public async Task CreateArticleAsync(string title, string content, int category, Guid userId)
        {
            var article = new Article()
            {
                Title = title,
                Content = content,
                category = category,
                UserId = userId
            };

            await _articleService.CreateAsync(article);

            //foreach (var item in categoryId)
            //{
            //    await _articleToCategoryService.CreateAsync(new ArticleToCategory()
            //    {
            //        ArticleId = article.Id,
            //        BlogCategoryId = item
            //    },false);
            //}
            //await _articleToCategoryService.SaveAsync();
        }

        public async Task CreateCategory(string name, Guid userId)
        {
            await _blogCategoryService.CreateAsync(new BlogCategory()
            {
                CategoryName = name,
                UserId = userId
            });
        }

        public async Task CreateCommentAsync(Guid articleId, Guid userId, string Content)
        {
            await _commentService.CreateAsync(new Comment()
            {
                ArticleId = articleId,
                UserId = userId,
                Content = Content
            });
        }
        public async Task<List<CommentDto>> GetCommentByArticleIdAsync(Guid articleId)
        {
            return await _commentService.GetAllByOrder(false).Where(c => c.ArticleId == articleId)
            .Include(c => c.User)
            .Select(c => new CommentDto()
            {
                Id = c.Id,
                UserId = c.UserId,
                Email = c.User.Email,
                ArticleId = c.ArticleId,
                Content = c.Content,
                CreateTime = c.CreateTime,
                NickName = c.User.NickName
            }).ToListAsync();
        }

        public async Task EditArticle(Guid ArticleId, string newTitle, string newContent, Guid[] newCategoryId)
        {
            var article = await _articleService.GetOneByIdAsync(ArticleId);
            article.Title = newTitle;
            article.Content = newContent;
            await _articleService.EditAsync(article);

            //删除原有类别
            var ArticleCategoriesId = _articleToCategoryService.GetAll().Where(a => a.ArticleId == ArticleId);
            foreach (var id in ArticleCategoriesId)
            {
                await _articleToCategoryService.RemovedAsync(id, false);
            }
            //添加新类别
            foreach (var newId in newCategoryId)
            {
                var newCategory = new ArticleToCategory();
                newCategory.BlogCategoryId = newId;
                newCategory.ArticleId = ArticleId;
                await _articleToCategoryService.CreateAsync(newCategory, false);
            }
            await _articleToCategoryService.SaveAsync();
        }

        public async Task EditCategory(Guid categoryId, string newCategoryName)
        {
            var category = await _blogCategoryService.GetOneByIdAsync(categoryId);
            category.CategoryName = newCategoryName;

            await _blogCategoryService.EditAsync(category);
        }
        public Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ArticleDto>> GetAllArticlesByEmail(string email)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// PagedList是一个分页插件的类型
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<ArticleDto> GetAllArticlesByUserId(Guid userId, int CurrentPage, int pageSize)
        {
            //得到当前用户所有的文章
            var result = _articleService.GetAllByPageOrder(pageSize, CurrentPage, false).Include(ar => ar.User).Where(a => a.UserId == userId).Select(article => new ArticleDto()
            {
                Title = article.Title,
                Content = article.Content,
                BadCount = article.BadCount,
                GoodCount = article.GoodCount,
                CreateTime = article.CreateTime,
                Id = article.Id,
                ImagePath = article.User.ImagePath,
                Email = article.User.Email,
                NickName = article.User.NickName
            }).ToPagedList(CurrentPage, pageSize);
            //得到每一个文章所属的类别
            foreach (var item in result)
            {
                var cates = _articleToCategoryService.GetAll().Include(c => c.BlogCategory).Where(atc => atc.ArticleId == item.Id).ToList();
                item.CategoryId = cates.Select(ac => ac.BlogCategoryId).ToArray();
                item.CategoryNames = cates.Select(ac => ac.BlogCategory.CategoryName).ToArray();
            }
            return result;
        }
        public PagedList<ArticleDto> GetAllArticlesByUserId(string categoryName, Guid userId, int CurrentPage, int pageSize)
        {
            var articles = _articleToCategoryService.GetAll().Include(m => m.BlogCategory).Include(m => m.Article).Where(m => m.Article.UserId == userId && m.BlogCategory.CategoryName == categoryName);
            var art = articles.Select(m => new ArticleDto()
            {
                Title = m.Article.Title,
                Content = m.Article.Content,
                BadCount = m.Article.BadCount,
                GoodCount = m.Article.GoodCount,
                CreateTime = m.Article.CreateTime,
                Id = m.Article.Id,
                ImagePath = m.Article.User.ImagePath, 
                Email = m.Article.User.Email,
                NickName = m.Article.User.NickName
            }).OrderByDescending(m=>m.CreateTime).ToList().ToPagedList(CurrentPage, pageSize);

            foreach (var article in art)
            {
                var category = _articleToCategoryService.GetAll().Include(m => m.BlogCategory).Where(m => m.ArticleId == article.Id).ToList();
                article.CategoryId = category.Select(m => m.BlogCategoryId).ToArray();
                article.CategoryNames = category.Select(m => m.BlogCategory.CategoryName).ToArray();
            }
            return art;
        }
        public List<BlogCategoryDto> GetAllCategories()
        { 
                return _blogCategoryService.GetAllByOrder().Select(cate => new BlogCategoryDto()
                {
                    Id = cate.Id,
                    CategoryName = cate.CategoryName
                }).ToList();        
        }
        public List<BlogCategoryDto> GetAllCategories(Guid userId)
        {
                return _blogCategoryService.GetAllByOrder().Where(c => c.UserId == userId).Select(cate => new BlogCategoryDto() 
                { 
                    Id = cate.Id,
                    CategoryName = cate.CategoryName
                }).ToList();                          
        }
        public async Task<ArticleDto> GetOneArticleByIdAsync(Guid articleId)
        {
            var result = await _articleService.GetAll().Include(a => a.User).Where(a => a.Id == articleId).Select(article => new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                GoodCount = article.GoodCount,
                BadCount = article.BadCount,
                CreateTime = article.CreateTime,
                Email = article.User.Email,
                ImagePath = article.User.ImagePath,
                NickName = article.User.NickName
            }).FirstAsync();

            var categoriesList = await _articleToCategoryService.GetAll().Include(a => a.BlogCategory).Where(atc => atc.ArticleId == result.Id).ToListAsync();
            result.CategoryId = categoriesList.Select(ato => ato.BlogCategoryId).ToArray();
            result.CategoryNames = categoriesList.Select(ato => ato.BlogCategory.CategoryName).ToArray();

            return result;
        }

        public async Task GoodCountAsync(Guid articleId)
        {
            var article = await _articleService.GetOneByIdAsync(articleId);
            article.GoodCount++;
            await _articleService.EditAsync(article);
        }
        public async Task<int> GoodCountAsync(Guid articleId, string userEmail)
        {
            var article = await _articleService.GetOneByIdAsync(articleId);         
            if (article.LikeUserInfo == null || article.LikeUserInfo.Contains(userEmail) == false) 
            {
                article.GoodCount++;
                article.LikeUserInfo = article.LikeUserInfo + userEmail+"-";
                await _articleService.EditAsync(article);
                return article.GoodCount;
            }
            else
            {
                return article.GoodCount;
            }
        }

        public async Task<bool> IsExistAsync(Guid articleId)
        {
            return await _articleService.GetAll().AnyAsync(a => a.Id == articleId);
        }
        public async Task<BlogCategoryDto> GetOneCategoryByIdAsync(Guid categoryId)
        {
            var result = await _blogCategoryService.GetAll().Where(c => c.Id == categoryId).Select(c => new BlogCategoryDto()
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            }).FirstAsync();
            return result;
        }
        public async Task<bool> IsCategoryExsitAsync(Guid categoryId)
        {
            var result = await _blogCategoryService.GetAll().AnyAsync(c => c.Id == categoryId);
            return result;
        }
        public async Task<BlogCategory> GetCategoryByArticleIdAsync(Guid articleId)
        {
            var articleTocategory = await _articleToCategoryService.GetAll().FirstAsync(m=>m.ArticleId==articleId);
            var catogory = await _blogCategoryService.GetOneByIdAsync(articleTocategory.BlogCategoryId);

            return catogory;
        }
        public async Task RemoveArticleAsync(Guid ArticleId)
        {
            var articleToCategory = await _articleToCategoryService.GetAll().FirstAsync(m => m.ArticleId == ArticleId);
            await _articleToCategoryService.RemovedAsync(articleToCategory.Id);
            await _articleService.RemovedAsync(ArticleId);
        }

        public async Task RemoveCategory(Guid categoryId)
        {
            await _blogCategoryService.RemovedAsync(categoryId);
        }
        public async Task<int> GetTotalOfArticlesAsync(Guid userId)
        {
            var totalOfArticle = await _articleService.GetAll().CountAsync(a => a.UserId == userId);
            return totalOfArticle;
        }
        public async Task<int> GetTotalOfArticlesAsync(string categoryName , Guid userId)
        {
            var totalArticle = await _articleToCategoryService.GetAll().Include(arT => arT.BlogCategory).Include(m => m.Article).CountAsync(a => a.BlogCategory.CategoryName == categoryName && a.Article.UserId == userId);
            return totalArticle;
        }
        public async Task<int> GetTotalOfArticlesAsync(string categoryName)
        {
            var totalArticle = await _articleToCategoryService.GetAll().Include(m => m.BlogCategory).CountAsync(m => m.BlogCategory.CategoryName == categoryName);
            return totalArticle;
        }

        public PagedList<ArticleDto> GetAllArticlesByCategoryName(string categoryName, int CurrentPage, int pageSize, int orderType)
        {
            PagedList<ArticleDto> articles;
            var tempData = _articleToCategoryService.GetAll().Include(m => m.BlogCategory).Include(m => m.Article).Where(m => m.BlogCategory.CategoryName == categoryName);
            var art = tempData.Select(m => new ArticleDto()
            {
                Title = m.Article.Title,
                Content = m.Article.Content,
                BadCount = m.Article.BadCount,
                GoodCount = m.Article.GoodCount,
                CreateTime = m.Article.CreateTime,
                Id = m.Article.Id,
                ImagePath = m.Article.User.ImagePath,
                Email = m.Article.User.Email
            });
            if (orderType == 1)
            {
                articles = art.OrderByDescending(m => m.GoodCount).ToList().ToPagedList(CurrentPage, pageSize);
            }
            else if (orderType == 2)
            {
                //未完，待续
                articles = art.OrderByDescending(m => m.CreateTime).ToList().ToPagedList(CurrentPage, pageSize);
            }
            else
            {
                articles = art.OrderByDescending(m => m.CreateTime).ToList().ToPagedList(CurrentPage, pageSize);
            }

            foreach (var article in articles)
            {
                var category = _articleToCategoryService.GetAll().Include(m => m.BlogCategory).Where(m => m.ArticleId == article.Id).ToList();
                article.CategoryId = category.Select(m => m.BlogCategoryId).ToArray();
                article.CategoryNames = category.Select(m => m.BlogCategory.CategoryName).ToArray();
            }
            return articles;
        }
        /// <summary>
        /// 过滤文本中特殊字符
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public string HtmlTagFilter(string htmlString)
        {
            //删除一些脚本标记
            htmlString = Regex.Replace(htmlString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除html标签
            htmlString = Regex.Replace(htmlString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"-->", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlString = Regex.Replace(htmlString, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            htmlString = htmlString.Replace("<", "");
            htmlString = htmlString.Replace(">", "");
            htmlString = htmlString.Replace("\r\n", "");
            htmlString = HttpContext.Current.Server.HtmlEncode(htmlString).Trim();
            return htmlString;
        }
        /// <summary>
        /// 截取文章的部分内容作为摘要
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string InterceptString(string inputString, int len)
        {
            var ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
    }
}
