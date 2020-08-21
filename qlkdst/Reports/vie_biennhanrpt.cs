using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qlkdst.Reports
{
    public class vie_biennhanrpt
    {
        public decimal iddatcoc { get; set; }
        public System.DateTime ngaydatcoc { get; set; }
        public decimal idtour { get; set; }
        public string sobiennhan { get; set; }
        public string nguoilambn { get; set; }
        public string daily { get; set; }

        //tên người đóng tiền
        public string tenkhach { get; set; }
        public string diachi { get; set; }
        public string dienthoai { get; set; }
        public string noidung { get; set; }
        public decimal sotien { get; set; }
        public System.DateTime ngaytao { get; set; }
        public string nguoitao { get; set; }
        public System.DateTime ngaysua { get; set; }
        public string nguoisua { get; set; }
        public string hinhthucthanhtoan { get; set; }
        public string chungtugoc { get; set; }
        public string tenmay { get; set; }
        public string sgtcode { get; set; }
        public string loaitien { get; set; }
        public decimal tygia { get; set; }
        public string sotienbangchu { get; set; }

        //tên khách đoàn
        public string tenkh { get; set; }

        public string chinhanh { get; set; }
        public string diachicn { get; set; }
        public string dienthoaicn { get; set; }
        public string fax { get; set; }

        public int sokhach { get; set; }
    }
}