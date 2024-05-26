using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    /// <summary>
    /// 评论表
    /// </summary>
    public class Comment : BaseEntity
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        [Required]
        [StringLength(800)]
        public string Content { get; set; }
        /// <summary>
        /// 哪个用户评论的
        /// </summary>
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 哪个文章
        /// </summary>
        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public int likes { get; set; }
        public int dislikes { get; set; }

    }
}
