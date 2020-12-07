using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
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
    }
}