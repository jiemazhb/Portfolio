using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dto
{
    public class ArticleList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public int GoodCount { get; set; }
        public int BadCount { get; set; }
        public int Category { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
