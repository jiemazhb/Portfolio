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
        PagedList<ArticleDto> GetAllArticles(int CurrentPage, int pageSize, out int amount);
        /// <summary>
        /// /创建文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateArticleAsync(string title, string content, int category, Guid userId);
        /// <summary>
        /// 创建种类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateCategory(string name, Guid userId);
        /// <summary>
        /// 得到某一用户的所有类别
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<BlogCategoryDto> GetAllCategories(Guid userId);
        List<BlogCategoryDto> GetAllCategories();
        /// <summary>
        /// 根据用户查找所有文章
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        PagedList<ArticleDto> GetAllArticlesByUserId(Guid userId, int CurrentPage, int pageSize);
        PagedList<ArticleDto> GetAllArticlesByUserId(string categoryName, Guid userId, int CurrentPage, int pageSize);
        PagedList<ArticleDto> GetAllArticlesByCategoryName(string categoryName, int CurrentPage, int pageSize, int orderType);
        PagedList<ArticleDto> GetAllArticlesBySearchStr(string searchStr, int CurrentPage, int pageSize, out int amount);
        /// <summary>
        /// 根据邮箱查找所有文章
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<List<ArticleDto>> GetAllArticlesByEmail(string email);
        /// <summary>
        /// 根据类别查询所有文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<ArticleDto>> GetAllArticlesByCategoryId(Guid categoryId);
        /// <summary>
        /// 根据名称删除类别
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task RemoveCategory(Guid categoryId);
        /// <summary>
        /// 修改类别
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="newCategoryName"></param>
        /// <returns></returns>
        Task EditCategory(Guid categoryId, string newCategoryName);
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="ArticleId"></param>
        /// <returns></returns>
        Task RemoveArticleAsync(Guid ArticleId);
        /// <summary>
        /// 编辑文章
        /// </summary>
        /// <param name="ArticleId"></param>
        /// <param name="newTitle"></param>
        /// <param name="newContent"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
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
