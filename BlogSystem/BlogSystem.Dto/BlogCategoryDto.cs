using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Dto
{
    public class BlogCategoryDto
    {
        public Guid Id { get; set; }
        [DisplayName("种类名称")]
        public string CategoryName { get; set; }
    }
}
