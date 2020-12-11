﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class SignUpModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MinLength(6, ErrorMessage = "Độ dài tên đăng nhập tối thiểu là 6 ký tự")]
        [MaxLength(24, ErrorMessage = "Độ dài tên đăng nhập tối đa là 24 ký tự")]
        public string usernname { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Độ dài mật khẩu tối thiểu là 6 ký tự")]
        [MaxLength(24, ErrorMessage = "Độ dài mật khẩu tối đa là 24 ký tự")]
        public string password { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("password", ErrorMessage = "Mật khẩu nhập lại không giống mật khẩu")]
        public string repassword { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string fullname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng định dạng")]
        public string email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Sô điện thoại không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string phone { get; set; }

        [Display(Name = "Cắn cước công dân")]
        [Required(ErrorMessage = "Căn cước công dân không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Căn cước công dân không đúng định dạng")]
        public string idCard { get; set; }

        [Display(Name = "Tỉnh/Thành Phố")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn Tỉnh/Thành phố")]
        public int cityId { get; set; }

        [Display(Name = "Quận/Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn Quận/Huyện")]
        public int districtId { get; set; }

        [Display(Name = "Phường/Xã")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn Phường/Xã")]
        public int wardId { get; set; }

        [Display(Name = "Số nhà")]
        [Required(ErrorMessage = "Số nhà không được để trống")]
        public string address { get; set; }


    }
}