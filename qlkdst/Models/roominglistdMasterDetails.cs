using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Models
{
    public class roominglistdMasterDetails
    {
        public decimal id_roomlistd { get; set; }
        public Nullable<decimal> id_roomlist { get; set; }
        public string sophong { get; set; }
        public Nullable<decimal> id_dsk { get; set; }
        public string loaiphong { get; set; }

        public virtual roominglist roominglist { get; set; }
    }
}