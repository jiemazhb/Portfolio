using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models.ArticleViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="请填入文章类别名称")]
        [StringLength(200,MinimumLength =2,ErrorMessage ="类别名称长度错误")]
        [Display(Name ="类别名称")]
        public string CategoryName { get; set; }
    }
}