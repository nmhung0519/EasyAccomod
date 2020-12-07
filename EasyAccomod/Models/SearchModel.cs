using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class SearchModel
    {
        public string keyWord { get; set; }

        public int type { get; set; }

        public int cityId { get; set; }

        public int districtId { get; set; }
    }
}