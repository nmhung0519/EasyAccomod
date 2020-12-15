using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class SearchModel
    {
        public string KeyWord { get; set; }

        public int Type { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }
        public int WardId { get; set; }
    }
}