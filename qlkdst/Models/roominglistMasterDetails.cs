using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Models
{
    public class roominglistMasterDetails
    {
        public decimal id_roomlist { get; set; }
        public Nullable<decimal> idtour { get; set; }
        public string tenkhachsan { get; set; }
        public Nullable<System.DateTime> ngaycheckin { get; set; }
        public Nullable<System.DateTime> ngaycheckout { get; set; }
        
        public virtual ICollection<roominglistd> lst { get; set; }
    }
}