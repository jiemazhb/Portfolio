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
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid Userid { get; set; }
        public string NickName { get; set; }
    }
}
