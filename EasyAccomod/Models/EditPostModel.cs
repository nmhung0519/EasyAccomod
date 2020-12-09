using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class EditPostModel
    {
        [Display(Name = "Loại bất động sản")]
        [Required(ErrorMessage = "Chọn loại bất động sản")]
        [Range(1, 4)]
        public int type { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        [Required(ErrorMessage = "Chọn Tỉnh/Thành phố")]
        [Range(1, int.MaxValue, ErrorMessage = "Tỉnh/Thành phố không đúng định dạng")]
        public int cityId { get; set; }

        [Display(Name = "Quận/Huyện")]
        [Required(ErrorMessage = "Chọn Quận/Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Quận/Huyện không đúng định dạng")]
        public int districtId { get; set; }

        [Display(Name = "Phường/Xã")]
        [Required(ErrorMessage = "Chọn Phường/Xã")]
        [Range(1, int.MaxValue, ErrorMessage = "Phường/Xã không đúng định dạng")]
        public int wardId { get; set; }

        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string title { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string content { get; set; }

        [Display(Name = "Địa chỉ: Số nhà, Tầng, Toà nhà, Đường, Thôn, Xóm,...")]
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string address { get; set; }

        [Display(Name = "Khép kín")]
        [Required(ErrorMessage = "Trường Khép kín không được để trống")]
        [Range(-1, 1)]
        public int closedRoom { get; set; }

        [Display(Name = "Bình nóng lạnh")]
        [Required(ErrorMessage = "Trường bình nóng lạnh không được để trống")]
        [Range(-1, 1)]
        public int waterHeater { get; set; }

        [Display(Name = "Nhà bếp riêng")]
        [Required(ErrorMessage = "Trường nhà bếp riêng không được để trống")]
        [Range(-1, 1)]
        public int privateKitchen { get; set; }

        [Display(Name = "Điều hoà")]
        [Required(ErrorMessage = "Trường điều hoà không được để trống")]
        [Range(-1, int.MaxValue)]
        public int airConditioner { get; set; }

        [Display(Name = "Ban công")]
        [Required(ErrorMessage = "Trường ban công không được để trống")]
        [Range(-1, 1)]
        public int balcony { get; set; }

        [Display(Name = "Giá điện")]
        public double electricityPrice { get; set; }

        [Display(Name = "Đơn vị tính")]
        public char electricUnit { get; set; }

        [Display(Name = "Giá nước")]
        public double waterPrice { get; set; }

        [Display(Name = "Đơn vị tính")]
        public char waterUnit { get; set; }

        [Display(Name = "Người đăng:")]
        [Required(ErrorMessage = "Người đăng không được để trống")]
        public string contactName { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string contactPhone { get; set; }

        [Display(Name = "Số ngày hiển thị")]
        [Required(ErrorMessage = "Số ngày hiển thị không được để trống")]
        [Range(7, int.MaxValue, ErrorMessage = "Số ngày hiển thị tối thiểu là 7")]
        public int numberOfDateShow { get; set; }

        [Display(Name = "Giá thuê")]
        public double price { get; set; }

        [Display(Name = "Đơn vị tính")]
        public char priceUnit { get; set; }

    }
}