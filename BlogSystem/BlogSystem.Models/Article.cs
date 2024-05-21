using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Models
{
    public class Article : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Column(TypeName ="ntext"), Required]
        public string Content { get; set; }
        /// <summary>
        /// 点赞
        /// </summary>
        public int GoodCount { get; set; }
        /// <summary>
        /// 反对
        /// </summary>
        public int BadCount { get; set; }
        public string LikeUserInfo { get; set; }
        public string DislikeUserInfo { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int category { get; set; } = 0;

    }
}
