using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Models
{
    public class CreatePostModel
    {
        [Display(Name = "Số nhà, đường/thôn")]
        [Required(ErrorMessage = "Số nhà, đường/thôn không được để trống")]
        public string Address { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn Tỉnh/Thành phố")]
        public int CityId { get; set; }

        [Display(Name = "Quận/Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn Quận/Huyện")]
        public int DistrictId { get; set; }

        [Display(Name = "Phường/Xã")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn Phường/Xã")]
        public int WardId { get; set; }

        //[Display(Name = "Tiêu đề")]
        //[MinLength(10, ErrorMessage = "Tiêu đề tối thiểu 10 ký tự")]
        //[MaxLength(255, ErrorMessage = "Độ dài tiêu đề tối đa 255 ký tự")]
        public string Title { get; set; }

        //[Display(Name = "Mô tả")]
        //[MinLength(50, ErrorMessage = "Mô tả tối thiểu 50 ký tự")]
        //[MaxLength(4000, ErrorMessage = "Độ dài mô tả tối đa 4000 ký tự")]
        public string Content { get; set; }

        [Display(Name = "Giá tiền(VND)")]
        public double Price { get; set; }
        [Display(Name = "Loại BĐS")]
        [Range(1,4, ErrorMessage = "Loại BĐS không đúng định dạng")]
        public int Type { get; set; }

        [Display(Name = "Số phòng")]
        public int NumOfRooms { get; set; }

        [Display(Name = "Diện tích(m2)")]
        [Required(ErrorMessage = "Giá phòng không được để trống")]
        public int Area { get; set; }

        [Display(Name = "Giá điện(VND/kWh)")]
        [Required(ErrorMessage = "Giá điện không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá điện phải lớn hơn 0")]
        public int ElectricityPrice { get; set; }

        [Display(Name = "Giá nước(VND)")]
        [Required(ErrorMessage = "Giá nước không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá nước phải lớn hơn 0")]
        public int WaterPrice { get; set; }

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

        [Display(Name = "Hình ảnh")]
        [LimitCount(3, 10, ErrorMessage = "Số lượng file ảnh tối thiểu là 3 và tối đa là 10")]
        public List<HttpPostedFileWrapper> UploadImages { get; set; }

        public IEnumerable<SelectListItem> GetListType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Phòng trọ"},
                new SelectListItem { Value = "2", Text = "Chung cư mini"},
                new SelectListItem { Value = "3", Text = "Nhà nguyên căn"},
                new SelectListItem { Value = "4", Text = "Chung cư nguyên căn"}
            };
        }
    }

    public class LimitCountAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public LimitCountAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list == null)
                return false;

            if (list.Count < _min || list.Count > _max)
                return false;

            return true;
        }
    }
}