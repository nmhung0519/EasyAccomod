using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class ChangePasswordModel
    {
        [Display(Name = "Mật khẩu cũ")]
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        [MinLength(6, ErrorMessage = "Độ dài mật khẩu tối thiểu là 6 ký tự")]
        [MaxLength(24, ErrorMessage = "Độ dài mật khẩu tối đa là 24 ký tự")]
        public string OldPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(6, ErrorMessage = "Độ dài mật khẩu tối thiểu là 6 ký tự")]
        [MaxLength(24, ErrorMessage = "Độ dài mật khẩu tối đa là 24 ký tự")]
        public string NewPassword { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không giống mật khẩu mới")]
        public string RetypeNewPassword { get; set; }


    }
}