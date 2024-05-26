using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dto
{
    public class AddComment
    {
        public Guid Id { get; set; }
        public Guid ArticleID { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public int Likes { get; set; }
        public int dislikes { get; set; }
    }
}
