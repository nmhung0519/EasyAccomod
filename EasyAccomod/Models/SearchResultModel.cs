using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class SearchResultModel
    {
        public SearchModel SearchInfo { get; set; }
        public List<PostModel> Result { get; set; }
        public SearchResultModel(SearchModel searchInfo, List<PostModel> result)
        {
            SearchInfo = searchInfo;
            Result = result;
        }
    }
}