using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.Models
{
    /// <summary>
    /// 多对多关系表
    /// </summary>
    public class ArticleToCategory : BaseEntity
    {
        [ForeignKey(nameof(BlogCategory))]
        public Guid BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
