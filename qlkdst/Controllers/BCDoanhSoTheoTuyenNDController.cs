using qlkdst.Common;
using qlkdstDB.DAO;
using System;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml;
using System.Data;
using System.Collections.Generic;
using qlkdstDB.EF;

namespace qlkdst.Controllers
{
    public class BCDoanhSoTheoTuyenNDController : BaseController
    {
        // GET: BCDoanhSoTheoTuyenND
        public ActionResult Index(string tungay, string denngay, string dlcn,string loaitourid)
        {
            var dao = new baocaoDAO();
            // DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            //DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");
            string sMM = "", sYYYY = "0";
            sMM = DateTime.Now.ToString("MM");
            sYYYY = DateTime.Now.ToString("yyyy");
            DateTime d1 = DateTime.Parse(sYYYY + "-" + sMM + "-1");
            DateTime d2 = DateTime.Parse(sYYYY + "-" + sMM + "-" + DungChung.LaySoNgayTrongThang(sMM, int.Parse(sYYYY)));

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

            ViewBag.dlcn = new SelectList(DungChung.LayDSChiNhanhTheoUser(sUserName), "Value", "Text");
            List<chinhanh> lst = DungChung.LayChiNhanhTheoUser(sUserName);

            ViewBag.loaitourid = new SelectList(DungChung.LayDSLoaiTour(), "Value", "Text");
            ViewBag.LoaiTourSelected = loaitourid;

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

            if (loaitourid == null)
            {
                loaitourid = "";
            }

            dt = dao.BCDTTheoTuyenND(d1, d2, Session["username"].ToString(), sRoles, dlcn, sCongTyPre, loaitourid,"");


            return View(dt);
        }

        public ActionResult Excel(string tungay, string denngay, string schinhanh,string loaitourid)
        {
            string Filename = "BCDSTheoTuyen_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcel(tungay, denngay, schinhanh, loaitourid);
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
            return RedirectToAction("~/BCCPHH/Index");
        }

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, string schinhanh,string loaitourid)
        {
            var dao = new baocaoDAO();

            string sUserName = Session["userId"].ToString();
            string sRoles = Session["RoleName"].ToString();

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
            if (loaitourid == null) loaitourid = "";

            DataSet ds = dao.BCDTTheoTuyenND(d1, d2, Session["username"].ToString(), sRoles, schinhanh, sCongTyPre, loaitourid,"");

            return ds;
        }

        private Stream CreateExcel(string tungay, string denngay, string schinhanh,string loaitourid, Stream stream = null)
        {
            DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");


            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);
            }

            DataSet list = GetDuLieuBC(d1, d2, schinhanh, loaitourid);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel              
                excelPackage.Workbook.Properties.Title = "DOANH SỐ THEO NGÀY ĐI TOUR";

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

            int iColReport = 12;

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");
            Color colorThanhLy = System.Drawing.ColorTranslator.FromHtml("#7FFF00");
            Color colorChuaThanhLy = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            ewres.Cells[2, 1].Value = "DOANH SỐ THEO NGÀY ĐI TOUR " + d1.ToString("dd/MM/yyyy") + " - " + d2.ToString("dd/MM/yyyy");
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].LoadFromText("STT");
            ewres.Cells[4, 2].LoadFromText("Code đoàn");
            ewres.Cells[4, 3].LoadFromText("Ngày đi");
            ewres.Cells[4, 4].LoadFromText("Ngày về");
            ewres.Cells[4, 5].LoadFromText("Tuyến tham quan");
            ewres.Cells[4, 6].LoadFromText("Số khách");
            ewres.Cells[4, 7].LoadFromText("Doanh số");
            ewres.Cells[4, 8].LoadFromText("Sales");
            ewres.Cells[4, 9].LoadFromText("Tên công ty/Khách hàng");
            ewres.Cells[4, 10].LoadFromText("Loại tour");
            ewres.Cells[4, 11].LoadFromText("Nguồn tour");//add 11092019
            ewres.Cells[4, 12].LoadFromText("Ngành nghề");//add 18122019

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
                decimal dDoanhSo = 0, dSK = 0;
                int iSTT = 1;
                decimal dTotal = 0, dTotalSK = 0;
                int iRowIndex = 5;
                string sbatdau = "", sketthuc = "";
                foreach (DataRow row in dt.Rows)
                {
                    string sTrangThai = row["trangthai"].ToString();

                    sbatdau = row["batdau"].ToString() == "" ? "" : DateTime.Parse(row["batdau"].ToString()).ToString("dd/MM/yyyy");
                    sketthuc = row["ketthuc"].ToString() == "" ? "" : DateTime.Parse(row["ketthuc"].ToString()).ToString("dd/MM/yyyy");

                    dSK = @Decimal.Parse(@row["sokhachtt"].ToString() == "" ? "0" : @row["sokhachtt"].ToString());
                    dDoanhSo = @Decimal.Parse(@row["doanhthutt"].ToString() == "" ? "0" : @row["doanhthutt"].ToString());

                    dTotal = dTotal + dDoanhSo;
                    dTotalSK = dTotalSK + dSK;

                    ewres.Cells[iRowIndex, 1].Value = iSTT.ToString();
                    ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    ewres.Cells[iRowIndex, 2].Value = row["sgtcode"].ToString();
                    ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                    ewres.Cells[iRowIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    if (sTrangThai == "3")
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(colorThanhLy);
                    }
                    else if (sTrangThai == "2")
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    }                  
                    else
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.White);
                    }

                  

                    ewres.Cells[iRowIndex, 3].Value = sbatdau;
                    ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = sketthuc;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 5].Value = row["tuyentq"].ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 6].Value = dSK;
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 7].Value = dDoanhSo;
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 8].Value = row["nguoitao"].ToString();
                    ewres.Cells[iRowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                  

                    ewres.Cells[iRowIndex, 9].Value = row["tenkh"].ToString();
                    ewres.Cells[iRowIndex, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    if (row["loaitourid"] != null)
                    {
                        ewres.Cells[iRowIndex, 10].Value = row["loaitourid"].ToString().ToUpper();
                    }else
                    {
                        ewres.Cells[iRowIndex, 10].Value = row["loaitourid"];
                    }                
                    
                    ewres.Cells[iRowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 11].Value = row["nguontour"].ToString();
                    ewres.Cells[iRowIndex, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 12].Value = row["nganhnghe"].ToString();
                    ewres.Cells[iRowIndex, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 12, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    iRowIndex = iRowIndex + 1;
                    iSTT = iSTT + 1;
                }
                //dong total
                ewres.Cells[iRowIndex, 1].Value = "";
                ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 2].Value = "";
                ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 3].Value = "";
                ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 4].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 5].Value = "TỔNG CỘNG";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 6].Value = dTotalSK;
                ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 7].Value = dTotal;
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 8].Value ="";
                ewres.Cells[iRowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
              

                ewres.Cells[iRowIndex, 9].Value ="";
                ewres.Cells[iRowIndex, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 10].Value = "";
                ewres.Cells[iRowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 11].Value = "";                
                DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 12].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 12, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                //empty row
                iRowIndex = iRowIndex + 1;
                ewres.Cells[iRowIndex, 1].Value = "";
                ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ewres.Cells[iRowIndex, 1, iRowIndex, iColReport].Merge = true;

                DataTable dt1 = ds.Tables[1];
                DataTable dt2 = ds.Tables[2];
                DataTable dt3 = ds.Tables[3];

                //DONG DA THANH LY
                iRowIndex = iRowIndex + 1;
                if (dt1.Rows.Count > 0)
                {
                  

                    ewres.Cells[iRowIndex, 1].Value = "";
                    ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = "";
                    ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 3].Value = "";
                    ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 5].Value = "Các đoàn đã thanh lý:";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                    ewres.Cells[iRowIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ewres.Cells[iRowIndex, 5].Style.Fill.BackgroundColor.SetColor(colorThanhLy);


                    ewres.Cells[iRowIndex, 6].Value = dt1.Rows[0]["sokhachtt"];
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 7].Value = dt1.Rows[0]["doanhthutt"];
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 8].Value = "";
                    ewres.Cells[iRowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                 

                    ewres.Cells[iRowIndex, 9].Value = "";
                    ewres.Cells[iRowIndex, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 10].Value = "";
                    ewres.Cells[iRowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 11].Value = "";                    
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                }

                if (dt2.Rows.Count > 0)
                {
                    //DONG CHUA THANH LY
                    iRowIndex = iRowIndex + 1;

                    ewres.Cells[iRowIndex, 1].Value = "";
                    ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = "";
                    ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 3].Value = "";
                    ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 5].Value = "Các đoàn chưa thanh lý:";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                    ewres.Cells[iRowIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ewres.Cells[iRowIndex, 5].Style.Fill.BackgroundColor.SetColor(Color.Yellow);


                    ewres.Cells[iRowIndex, 6].Value = dt2.Rows[0]["sokhachtt"];
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 7].Value = dt2.Rows[0]["doanhthutt"];
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 8].Value = "";
                    ewres.Cells[iRowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                  

                    ewres.Cells[iRowIndex, 9].Value = "";
                    ewres.Cells[iRowIndex, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 10].Value = "";
                    ewres.Cells[iRowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 11].Value = "";
                    ewres.Cells[iRowIndex, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                }

                if (dt3.Rows.Count > 0)
                {
                    //DONG CHUA KY HOP DONG 
                    iRowIndex = iRowIndex + 1;

                    ewres.Cells[iRowIndex, 1].Value = "";
                    ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = "";
                    ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 3].Value = "";
                    ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 5].Value = "Các đoàn chưa ký hợp đồng:";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 6].Value = dt3.Rows[0]["sokhachtt"];
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 7].Value = dt3.Rows[0]["doanhthutt"];
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 8].Value = "";
                    ewres.Cells[iRowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                 

                    ewres.Cells[iRowIndex, 9].Value = "";
                    ewres.Cells[iRowIndex, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 10].Value = "";
                    ewres.Cells[iRowIndex, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 11].Value = "";
                    ewres.Cells[iRowIndex, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                }

            }

            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}