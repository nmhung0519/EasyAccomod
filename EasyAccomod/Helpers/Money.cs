using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAccomod.Helpers
{
    public static class Money
    {
        public static string ToVND(this int amount)
        {
            string tmp = amount.ToString();
            string result = "";
            while (tmp.Length > 3)
            {
                result = "." + tmp.Substring(tmp.Length - 3, 3) + result;
                tmp = tmp.Substring(0, tmp.Length - 3);
            }
            result = tmp + result + "đ";
            return result;
        }
    }
}