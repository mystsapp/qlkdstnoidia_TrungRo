using CrystalDecisions.CrystalReports.Engine;
//using Microsoft.Office.Interop.Excel;
using qlkdst.Common;
using qlkdst.Reports;
using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace qlkdst.Views.BienNhan
{
    public partial class WebFormInBN : System.Web.UI.Page
    {
        qlkdtrEntities db = new qlkdtrEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            ReportDocument rptDoc = new ReportDocument();
            dsBienNhanFrmIn ds = new dsBienNhanFrmIn();
            decimal dId = 0;
            if (Request["tid"] != null)
            {
                dId =decimal.Parse(Request["tid"].ToString());
            }

            //rptDoc.Load(Server.MapPath("~/Reports/rptBienNhan.rpt"));
            rptDoc.Load(Path.Combine(Server.MapPath("~/Reports"), "rptBienNhanOB.rpt"));
            biennhanDAO dao = new biennhanDAO();

            List<vie_biennhanrpt> objrpt = new List<vie_biennhanrpt>();
            List<vie_biennhan> obj = dao.GetVieBN(dId).ToList();

            foreach(vie_biennhan v in obj)
            {
                vie_biennhanrpt vi = new vie_biennhanrpt();
                vi.iddatcoc = v.iddatcoc;
                vi.ngaydatcoc = v.ngaydatcoc == null ? DateTime.Now:(DateTime)v.ngaydatcoc;
                vi.idtour = v.idtour == null ? 0 : (decimal)v.idtour;
                vi.sobiennhan = v.sobiennhan==null?"":v.sobiennhan;
                vi.nguoilambn = v.nguoilambn==null?"":v.nguoilambn;
                vi.daily = v.daily == null ? "" : v.daily;
                vi.tenkhach = v.tenkhach == null ? "" : v.tenkhach;
                vi.tenkh = v.tenkh == null ? "" : v.tenkh;
                vi.diachi = v.diachi == null ? "" : v.diachi;
                vi.dienthoai = v.dienthoai == null ? "" : v.dienthoai;
                vi.noidung = v.noidung == null ? "" : v.noidung;
                vi.sotien = v.sotien == null ? 0 : (decimal)v.sotien;
                vi.ngaytao = v.ngaytao == null ? DateTime.Now : (DateTime)v.ngaytao;
                vi.nguoitao = v.nguoitao == null ? "" : v.nguoitao;
                vi.ngaysua = v.ngaysua == null ? DateTime.Now : (DateTime)v.ngaysua;
                vi.nguoisua = v.nguoisua == null ? "" : v.nguoisua;
                vi.hinhthucthanhtoan = v.hinhthucthanhtoan == null ? "" : v.hinhthucthanhtoan;
                vi.chungtugoc = v.chungtugoc == null ? "" : v.chungtugoc;
                vi.tenmay = v.tenmay == null ? "" : v.tenmay;
                vi.loaitien = v.loaitien == null ? "" : v.loaitien;
                vi.tygia = v.tygia == null ? 0: (decimal)v.tygia;
                vi.sgtcode = v.sgtcode == null ? "" : v.sgtcode;
                vi.sotienbangchu = DungChung.So_chu(v.sotien==null?0:(decimal)v.sotien);
                //them 10062019
                vi.chinhanh = v.chinhanh;
                vi.diachicn = v.diachicn;
                vi.dienthoaicn = v.dienthoaicn;
                vi.fax = v.fax;
                //them 11062019
                vi.sokhach = v.sokhach == null ? 0 : (int)v.sokhach;

                objrpt.Add(vi);
            }


            rptDoc.SetDataSource(objrpt);
            //rptDoc.SetDataSource(ds);
            CrystalReportViewer1.ReportSource = rptDoc;
        }

        protected void CrystalReportViewer1_Init(object sender, EventArgs e)
        {
            
        }

        protected void CrystalReportViewer1_Init1(object sender, EventArgs e)
        {

        }
    }
}