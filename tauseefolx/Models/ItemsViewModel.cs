using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tauseefolx.Models
{
    public class ItemsViewModel
    {
        public int Sellitemid { get; set; }
        public string Productname { get; set; }
        public string Productdesc { get; set; }
        public string Productprice { get; set; }

        public string Productimage { get; set; }

        public int userid { get; set; }

        public string category { get; set; }

        public string Status { get; set; }

        public string Sellername { get; set; }
        public string Boughtdate { get; set; }


    }
}