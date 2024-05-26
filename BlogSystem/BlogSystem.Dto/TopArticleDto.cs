using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dto
{
    public class TopArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Category { get; set; }
        public int CategoryName { get; set; }
        public Guid Userid { get; set; }
        public string NickName { get; set; }
        public string picPath { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public int Likes { get; set; }
    }
}
