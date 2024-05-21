using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models.ArticleViewModels
{
    public class CreateArticleViewModel
    {
        [Required(ErrorMessage ="need a subject")]
        [DisplayName("Subject")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content can not be empty")]
        [DisplayName("Content")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Select the catagory")]
        [DisplayName("Category")]
        public int Category { get; set; }

    }
}