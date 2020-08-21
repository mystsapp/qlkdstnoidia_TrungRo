using qlkdst.Common;
using qlkdstDB.DAO;
using System;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml;
using System.Data;
using OfficeOpenXml.Drawing.Chart;
using qlkdstDB.EF;
using System.Collections.Generic;

namespace qlkdst.Controllers
{
    public class BCTheoNhomThiTruongController : BaseController
    {
        // GET: BCTheoNhomThiTruong
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BCNN(string tungay, string denngay, string dlcn)
        {
            var dao = new baocaoDAO();
            DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");

            DataSet dt = null;

            string sUserName = Session["userId"].ToString();
            string sRoles = Session["RoleName"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();

            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {

                //lay dinh dang ngay
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);

            }

            ViewBag.tungay = d1.ToString("dd/MM/yyyy");
            ViewBag.denngay = d2.ToString("dd/MM/yyyy");

            if (d2 < d1)
            {
                SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

                return RedirectToAction("ShowError", "BaoCao");
            }

            List<chinhanh> lst = DungChung.LayChiNhanhTheoUser(sUserName);
            ViewBag.dlcn = new SelectList(DungChung.LayDSChiNhanhTheoUser(sUserName), "Value", "Text");

            string sCongTyPre = "";//ds chi nhanh co quyen
            foreach (chinhanh c in lst)
            {
                sCongTyPre = sCongTyPre + "," + c.chinhanh1;
            }

            //bo dau , dau tien
            if (sCongTyPre.Length > 0)
                sCongTyPre = sCongTyPre.Substring(1, sCongTyPre.Length - 1);

            ViewBag.chinhanhSelected = dlcn;

            if (dlcn == null)
            {
                dlcn = "";
            }

            dt = dao.BCTheoNhomTT(d1, d2, Session["username"].ToString(), sRoles, dlcn, sCongTyPre);


            return View(dt);
        }

        [HttpGet]
        public ActionResult Excel(string tungay, string denngay, string schinhanh)
        {

            //tungay = "01-07-2017";
            //denngay = "01-07-2018";
            //CongTy = "";
            //CongTyPre = "STA,STB,STH,STO,STS,STT";
            string Filename = "BCDoanhThuTheoNhomTT_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcel(tungay, denngay, schinhanh);
            // Tạo buffer memory stream để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Filename);
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();

            // Redirect về luôn trang index <img draggable="false" class="emoji" alt="😀" src="https://s0.wp.com/wp-content/mu-plugins/wpcom-smileys/twemoji/2/svg/1f600.svg">
            return RedirectToAction("~/BCNXTTheoThang/Index");
        }

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, string sUserName, string sRoles, string schinhanh)
        {
            var dao = new baocaoDAO();

            List<chinhanh> lst = DungChung.LayChiNhanhTheoUser(sUserName);

            string sCongTyPre = "";//ds chi nhanh co quyen
            foreach (chinhanh c in lst)
            {
                sCongTyPre = sCongTyPre + "," + c.chinhanh1;
            }

            //bo dau , dau tien
            if (sCongTyPre.Length > 0)
                sCongTyPre = sCongTyPre.Substring(1, sCongTyPre.Length - 1);

            if (schinhanh == null) schinhanh = "";

            DataSet ds = dao.BCTheoNhomTT(d1, d2, Session["username"].ToString(), sRoles, schinhanh, sCongTyPre);
            return ds;
        }

        private Stream CreateExcel(string tungay, string denngay, string schinhanh, Stream stream = null)
        {
            DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");


            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);
            }

            string sUserName = Session["userId"].ToString();
            string sRoles = Session["RoleName"].ToString();

            DataSet list = GetDuLieuBC(d1, d2, sUserName, sRoles, schinhanh);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel              
                excelPackage.Workbook.Properties.Title = "BÁO CÁO DOANH THU THEO TỪNG NHÓM THỊ TRƯỜNG";

                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("bc");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet = FormatWorkSheet(list, workSheet, d1, d2);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }



        public ExcelWorksheet FormatWorkSheet(DataSet ds, ExcelWorksheet ew, DateTime d1, DateTime d2)
        {
            int iColReport = 4;//so cot bao bieu

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            string sNam1 = "";
            sNam1 = d1.ToString("yyyy");


            ewres.Cells[2, 1].Value = "BÁO CÁO DOANH SỐ THEO TỪNG NHÓM THỊ TRƯỜNG TỪ NGÀY " + d1.ToString("dd/MM/yyyy") + " ĐẾN NGÀY " + d2.ToString("dd/MM/yyyy");
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, 15].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].LoadFromText("STT");
            ewres.Cells[4, 2].LoadFromText("Nhóm thị trường");
            ewres.Cells[4, 3].LoadFromText("Số khách");
            ewres.Cells[4, 4].LoadFromText("Doanh số");

            //create header
            // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            using (var range = ewres.Cells[4, 1, 4, iColReport])
            {
                // Canh giữa cho các text
                range.Style.WrapText = false;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại                    
                range.Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
                // Set Border
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                // Set màu ch Border
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Top.Color.SetColor(Color.Black);
                range.Style.Border.Right.Color.SetColor(Color.Black);
                range.Style.Border.Bottom.Color.SetColor(Color.Black);
            }
            //END HEADER

            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                decimal dSK1 = 0, dDoanhSo = 0;
                int iSTT = 1;

                decimal[] dTotal = new decimal[2];
                int iRowIndex = 5;
                foreach (DataRow row in dt.Rows)
                {
                    dSK1 = @Decimal.Parse(row["sokhachtt"].ToString() == "" ? "0" : row["sokhachtt"].ToString());
                    dDoanhSo = @Decimal.Parse(row["doanhthutt"].ToString() == "" ? "0" : row["doanhthutt"].ToString());
                    dTotal[0] = dTotal[0] + dSK1;
                    dTotal[1] = dTotal[1] + dDoanhSo;

                    ewres.Cells[iRowIndex, 1].Value = iSTT;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = row["nganhnghe"];
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    ewres.Cells[iRowIndex, 3].Value = dSK1;
                    ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = dDoanhSo;
                    ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    iRowIndex = iRowIndex + 1;
                    iSTT = iSTT + 1;
                }


                //add total row
                ewres.Cells[iRowIndex, 1].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                ewres.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 3].Value = dTotal[0];
                ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 4].Value = dTotal[1];
                ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);


            }

            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}