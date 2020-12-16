using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Models
{
    public class AdvancedSearchModel
    {
        [Display(Name = "Tỉnh/Thành phố")]
        [Required(ErrorMessage = "Tỉnh/Thành phố không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
        public int CityId { get; set; }

        [Display(Name = "Quận/Huyện")]
        [Required(ErrorMessage = "Quận/Huyện không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public int DistrictId { get; set; }

        [Display(Name = "Phường/Xã")]
        [Required(ErrorMessage = "Phường/Xã không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public int WardId { get; set; }

        [Display(Name = "Loại BĐS")]
        [Required(ErrorMessage = "Loại BĐS không được để trống")]
        [Range(1, 4, ErrorMessage = "Loại BĐS không đúng định dạng")]
        public int Type { get; set; }
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Diện tích phải là số")]
        public double FromArea { get; set; }
        [RegularExpression(@"^[0-9''.'\s]{1,40}$", ErrorMessage = "Diện tích phải là số")]
        public double ToArea { get; set; }

        [Display(Name = "Điều hoà")]
        public bool AirConditioner { get; set; }

        [Display(Name = "Nóng lạnh")]
        public bool WaterHeater { get; set; }

        [Display(Name = "Bếp riêng")]
        public bool PrivateKitchen { get; set; }

        [Display(Name = "Ban công")]
        public bool Balcony { get; set; }

        [Display(Name = "Vệ sinh khép kín")]
        public bool ClosedRoom { get; set; }

        [Display(Name = "Không chung chủ")]
        public bool WithoutHost { get; set; }

        public IEnumerable<SelectListItem> GetListType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Phòng trọ", Value = "1"},
                new SelectListItem { Text = "Chung cư mini", Value = "2"},
                new SelectListItem { Text = "Nhà nguyên căn", Value = "3"},
                new SelectListItem { Text = "Chung cư nguyên căn", Value = "4"}
            };
        }

    }
}