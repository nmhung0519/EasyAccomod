using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
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
    }
}