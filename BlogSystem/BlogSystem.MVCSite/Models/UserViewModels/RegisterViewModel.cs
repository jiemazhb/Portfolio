using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogSystem.MVCSite.Models.UserViewModels
{
    public class RegisterViewModel
    {
        private string nickName;

        [Required(ErrorMessage ="Please, enter your email address")]
        [EmailAddress(ErrorMessage ="Email addresss not correct")]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Display(Name = "Name")]
        [StringLength(15)]
        [DataType(DataType.Text)]
        public string NickName 
        {
            get {
                if (nickName == null)
                {
                    return "User";
                }
                else
                {
                    return nickName;
                }

            }
            set {
                nickName = value;
            }
        }

        [Required(ErrorMessage ="Please enter your password")]
        [DataType(DataType.Password)]
        [StringLength(50,MinimumLength =6,ErrorMessage ="Must be between 6 and 50 characters")]
        [Display(Name = "Password")]
        public string PassWord { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [StringLength(50,MinimumLength =6, ErrorMessage = "Must be between 6 and 50 characters")]
        [Compare(nameof(PassWord), ErrorMessage ="Password not match")]
        [Display(Name = "Re-Password")]
        public string ConfirmPassWord { get; set; }
    }
}