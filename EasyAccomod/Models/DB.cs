using System;
using System.Collections.Generic;
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
    }
}