using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Models
{
    public class ObjDmKhachHang
    {
        public string makh { get; set; }
        public string tengiaodich { get; set; }
    }

    public class ObjDsNN
    {
        public int idhuytour { get; set; }
        public string noidung { get; set; }

        public string[] lstnn { get; set; }
    }
}