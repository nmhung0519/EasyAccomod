using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class SignInModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MinLength(6, ErrorMessage = "Độ dài tên đăng nhập tối thiểu là 6 ký tự")]
        [MaxLength(24, ErrorMessage = "Độ dài tên đăng nhập tối đa là 24 ký tự")]
        public string username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Độ dài mật khẩu tối thiểu là 6 ký tự")]
        [MaxLength(24, ErrorMessage = "Độ dài mật khẩu tối đa là 24 ký tự")]
        public string password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool rememberMe { get; set; }
    }
}