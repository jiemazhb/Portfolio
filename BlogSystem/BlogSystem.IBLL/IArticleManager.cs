using BlogSystem.Dto;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace BlogSystem.IBLL
{
    public interface IArticleManager
    {
        List<TopArticleDto> GetArticleByDateAsync();
        Task StatisticsArticleDataAsync(WebSiteStatistic webSiteStatistic);
        PagedList<ArticleDto> GetAllArticles(int CurrentPage, int pageSize, out int amount);
        Task CreateArticleAsync(string title, string content, int category, Guid userId);

        Task CreateCategory(string name, Guid userId);

        List<BlogCategoryDto> GetAllCategories(Guid userId);
        List<BlogCategoryDto> GetAllCategories();

        PagedList<ArticleDto> GetAllArticlesByUserId(Guid userId, int CurrentPage, int pageSize);
        PagedList<ArticleDto> GetAllArticlesByUserId(string categoryName, Guid userId, int CurrentPage, int pageSize);
        PagedList<ArticleDto> GetAllArticlesByCategoryName(string categoryName, int CurrentPage, int pageSize, int orderType);
        PagedList<ArticleDto> GetAllArticlesBySearchStr(string searchStr, int CurrentPage, int pageSize, out int amount);

        Task<List<ArticleDto>> GetAllArticlesByEmail(string email);

        Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId);

        Task RemoveCategory(Guid categoryId);

        Task EditCategory(Guid categoryId, string newCategoryName);

        Task RemoveArticleAsync(Guid ArticleId);

        Task EditArticle(Guid ArticleId, string newTitle, string newContent, Guid[] newCategoryId);
        /// <summary>
        /// 判断文章是否存在
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<bool> IsExistAsync(Guid articleId);
        /// <summary>
        /// 根据文章编号查询对应文章明细
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<ArticleDtoNew> GetOneArticleByIdAsync(Guid articleId);
        Task<ArticleDtoNew> GetOneArticleByArticleIdAsync(Guid articleId);
        /// <summary>
        /// 得到当前用户总的文章数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetTotalOfArticlesAsync(Guid userId);
        Task<int> GetTotalOfArticlesAsync();
        Task<int> GetTotalOfArticlesAsync(string categoryName);
        Task<int> GetTotalOfArticlesAsync(string categoryName, Guid userId);
        /// <summary>
        ///  点赞功能
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task GoodCountAsync(Guid articleId);
        Task<int> GoodCountAsync(Guid articleId, string userEmail);
        /// <summary>
        /// 反对功能
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task BadCountAsync(Guid articleId);
        Task<int> BadCountAsync(Guid articleId, string email);
        /// <summary>
        /// 创建评论功能实现
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        Task CreateCommentAsync(Guid articleId, string content, Guid userId);
        Task<List<CommentDto>> GetCommentByArticleIdAsync(Guid articleId);
        Task<bool> IsCategoryExsitAsync(Guid articleId);
        Task<BlogCategoryDto> GetOneCategoryByIdAsync(Guid categoryId);
        Task<BlogCategory> GetCategoryByArticleIdAsync(Guid articleId);
        string HtmlTagFilter(string htmlString);
        string InterceptString(string inputString, int len);
        Task<ArticleDtoNew> GetArticleByLikeAsync();
        Task<List<TopArticleDto>> GetTopArticles();
    }
}
