using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models.UserViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Please enter your email address")]
        [DataType(DataType.EmailAddress,ErrorMessage ="The format is not correct")]
        [Display(Name ="Email Addresss")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [StringLength(50,MinimumLength =6,ErrorMessage ="Must be 6 ~ 50 characters")]
        [Display(Name ="Password")]
        public string PassWord { get; set; }

        [Display(Name ="remember me")]
        public bool  RememberMe { get; set; }
    }
}