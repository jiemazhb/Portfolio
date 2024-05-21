using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        [DisplayName("标题")]
        public string Title { get; set; }
        [DisplayName("内容")]
        public string Content { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("用户名")]
        public string Email { get; set; }
        [DisplayName("点赞数")]
        public int GoodCount { get; set; }
        [DisplayName("反对数")]
        public int BadCount { get; set; }
        [DisplayName("头像")]
        public string ImagePath { get; set; }
        [DisplayName("昵称")]
        public string NickName { get; set; }
        [DisplayName("文章类别")]
        public string[] CategoryNames { get; set; }
        public Guid[] CategoryId { get; set; }
    }
}
