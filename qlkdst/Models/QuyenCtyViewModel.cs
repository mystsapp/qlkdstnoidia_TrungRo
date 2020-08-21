using System;
using System.Collections.Generic;

namespace qlkdst.Models
{
    public class QuyenCtyViewModel
    {
        public int id_usrkhu { get; set; }
        public string idkhucn { get; set; }
        public string userId { get; set; }
        public Nullable<System.DateTime> ngaytao { get; set; }
        public string nguoitao { get; set; }
        public Nullable<System.DateTime> ngaysua { get; set; }
        public string nguoisua { get; set; }

        public IList<KhuCNViewModel> listKhuCN { get; set; }
    }

    public class KhuCNViewModel
    {
        public int? idkhucn { get; set; }
        public string tenkhucn { get; set; }
        public bool Checked { get; set; }
    }
}