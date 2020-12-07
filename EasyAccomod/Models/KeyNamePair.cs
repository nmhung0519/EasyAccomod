using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAccomod.Models
{
    public class KeyNamePair
    {
        public int id { get; set; }

        public string name { get; set; }

        public KeyNamePair() { }

        public KeyNamePair(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}