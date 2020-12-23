using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext() : base("name=Default") { }

        public DbSet<CityModel> Cities { get; set; }
        public DbSet<DistrictModel> Districts { get; set; }
        public DbSet<WardModel> Wards { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ViewPostModel> ViewPosts { get; set; }
    }

    [Table("district")]
    public class DistrictModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("districtname")]
        public string DistrictName { get; set; }

        [Column("cityid")]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public CityModel City { get; set; }
        public virtual ICollection<PostModel> Posts { get; set; } 
        public virtual ICollection<WardModel> Wards { get; set; }
    }

    [Table("ward")]
    public class WardModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("wardname")]
        public string WardName { get; set; }

        [Column("districtid")]
        public int DistrictId { get; set; }
        
        [ForeignKey("DistrictId")]
        public DistrictModel District { get; set; }

        public virtual ICollection<PostModel> Posts { get; set; }
    }

    [Table("city")]
    public class CityModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cityname")]
        public string CityName { get; set; }
        public virtual ICollection<PostModel> Posts { get; set; }
        public virtual ICollection<DistrictModel> Districts { get; set; }
    }

    [Table("post")]
    public class PostModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }
        [Column("cityid")]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public CityModel City { get; set; }

        [Column("districtid")]
        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public DistrictModel District { get; set; }

        [Column("wardid")]
        public int WardId { get; set; }

        [ForeignKey("WardId")]
        public WardModel Ward { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("posterid")]
        public int PosterId { get; set; }
        
        [ForeignKey("PosterId")]
        public AccountModel Poster { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("closed_room")]
        public int ClosedRoom { get; set; }

        [Column("water_heater")]
        public int WaterHeater { get; set; }

        [Column("private_kitchen")]
        public int PrivateKitchen { get; set; }

        [Column("air_conditioner")]
        public int AirConditioner { get; set; }

        [Column("balcony")]
        public int Balcony { get; set; }

        [Column("without_host")]
        public int WithoutHost { get; set; }

        [Column("electricity_price")]
        public int ElectricityPrice { get; set; }

        [Column("water_price")]
        public int WaterPrice { get; set; }


        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("contact_name")]
        public string ContactName { get; set; }

        [Column("contact_phone")]
        public string ContactPhone { get; set; }

        [Column("last_changed")]
        public DateTime LastChanged { get; set; }

        [Column("price")]
        public double Price { get; set; }

        [Column("price_unit")]
        public int PriceUnit { get; set; }

        [Column("area")]
        public int Area { get; set; }

        [Column("views")]
        public int Views { get; set; }

        [Column("approved")]
        public int Approved { get; set; }

        [Column("isshow")]
        public bool IsShow { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("sold")]
        public bool Sold { get; set; }
        public virtual ICollection<PostImageModel> Images { get; set; }
        public virtual ICollection<TicketModel> Tickets { get; set; }

        public string DisplayPrice()
        {
            if (Price < 0) return "Liên hệ";
            if (PriceUnit == 1) return Price + "/tháng";
            if (PriceUnit == 2) return Price + "/năm";
            return "Liên hệ";
        }

        public string GetElectricityPrice()
        {
            if (ElectricityPrice < 0) return "Giá dân";
            return ElectricityPrice + "/kWh";
        }

        public string GetWaterPrice()
        {
            if (WaterPrice < 0) return "Giá dân";
            return WaterPrice + "/số";
        }
    }

    [Table("ticket")]
    public class TicketModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public PostModel Post { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("time")]
        public int Time { get; set; }

        [Column("unit_time")]
        public int UnitTime { get; set; }

        [Column("approved")]
        public int Approved { get; set; }

        [Column("approver_id")]
        public int ApproverId { get; set; }

        [Column("approval_time")]
        public DateTime ApprovalTime { get; set; }

        public string GetTime()
        {
            if (UnitTime == 1) return Time + " " + "Tuần";
            if (UnitTime == 2) return Time + " " + "Tháng";
            if (UnitTime == 3) return Time + " " + "Quý";
            if (UnitTime == 4) return Time + " " + "Năm";
            return "";
        }
    }

    [Table("account")]
    public class AccountModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("fullname")]
        public string FullName { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("idcard")]
        public string IdCard { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("approverid")]
        public int ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public virtual AccountModel Approver { get; set; }

        [Column("approval_time")]
        public DateTime ApprovalTime { get; set; }

        [Column("wardid")]
        public int WardId { get; set; }

        [ForeignKey("WardId")]
        public WardModel Ward { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("approved")]
        public bool Approved { get; set; }

        public virtual ICollection<PostModel> Posts { get; set; }
        public virtual ICollection<AccountModel> Accounts { get; set; }
        public virtual ICollection<NotificationModel> Notifications { get; set; }
    }

    [Table("notification")]
    public class NotificationModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("targetid")]
        public int TargetId { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("createdtime")]
        public DateTime CreatedTime { get; set; }

        [Column("receiverid")]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public AccountModel Receiver { get; set; }

        [Column("seen")]
        public bool Seen { get; set; }
    }

    [Table("viewpost")]
    public class ViewPostModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        public int PostId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }
    }

    [Table("postimage")]
    public class PostImageModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual PostModel Post { get; set; }

        [Column("path")]
        public string Path { get; set; }
    }
}