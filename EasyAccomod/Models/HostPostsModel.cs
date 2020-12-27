using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class HostPostsModel
    {
        public List<PostModel> ShowingPost { get; set; }
        public List<PostModel> ApprovingPost { get; set; }

        public List<PostModel> SoldPost { get; set; }
        public List<PostModel> ExpiredPost { get; set; }
        public List<PostModel> RefusedPost { get; set; }
    }
}