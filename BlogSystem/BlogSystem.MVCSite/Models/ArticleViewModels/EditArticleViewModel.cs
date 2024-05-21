using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models.ArticleViewModels
{
    public class EditArticleViewModel
    {
        public Guid Id { get; set; }
        [Display(Name ="标题")]
        [Required(ErrorMessage ="标题不能为空")]        
        public string Title { get; set; }
        [Display(Name = "内容")]
        [Required(ErrorMessage ="请输入您要修改的内容")]
        public string Content { get; set; }
        [Display(Name = "文章类别")]
        [Required(ErrorMessage ="请选择文章类别")]
        public Guid[] CategoriesId { get; set; }
    }
}