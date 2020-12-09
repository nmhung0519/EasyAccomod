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
    }

    [Table("district")]
    public class DistrictModel
    {
        [Key]
        [Column]
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

        [Column("electricity_price")]
        public string ElectricityPrice { get; set; }

        [Column("water_price")]
        public string WaterPrice { get; set; }

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
        public char PriceUnit { get; set; }

        [Column("rent_time")]
        public string RentTime { get; set; }

        [Column("date_per_time")]
        public string DatePerTime { get; set; }

        [Column("deposit")]
        public string Deposit { get; set; }

        public virtual ICollection<TicketModel> Tickets { get; set; }
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

        [Column("approver_id")]
        public int ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public AccountModel Approver { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }
    }

    [Table("account")]
    public class AccountModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Column("idcard")]
        public string IdCard { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("approverid")]
        public int ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public AccountModel Approver { get; set; }

        [Column("approval_time")]
        public DateTime ApprovalTime { get; set; }

        public virtual ICollection<PostModel> Posts { get; set; }
        public virtual ICollection<AccountModel> Accounts { get; set; }
    }
}