using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    [Table("city")]
    public class CityModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cityname")]
        public string CityName { get; set; }
    }
}