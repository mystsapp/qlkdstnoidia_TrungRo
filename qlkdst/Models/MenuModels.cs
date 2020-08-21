using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Models
{
    public class MenuModels
    {
        public int menuid { get; set; }
        public string menunm { get; set; }
        public string menulink { get; set; }
        public Nullable<int> areaid { get; set; }
        public bool show_mk { get; set; }
        public string classcss { get; set; }
        public string role { get; set; }
        public string areaname { get; set; }

        public string areacss { get; set; }

        public string actionnm { get; set; }
        public string controllernm { get; set; }

        public string areamvc { get; set; }

        public int? thutu { get; set; }

        public int? tt { get; set; }
    }
}