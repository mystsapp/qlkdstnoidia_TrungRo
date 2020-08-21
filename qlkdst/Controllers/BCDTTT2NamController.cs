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
using System.Collections.Generic;
using qlkdstDB.EF;

namespace qlkdst.Controllers
{
    public class BCDTTT2NamController : BaseController
    {
        // GET: BCDTTT2Nam
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dttt2nam(string tuthang1, string denthang1, string nam1, string tuthang2, string denthang2, string nam2, string dlcn)
        {
            var dao = new baocaoDAO();
            DateTime d1 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-12-31");


            DateTime d3 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d4 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");

            DataSet dt = null;

            string sUserName = Session["userId"].ToString();
            string sRoles = Session["RoleName"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();

            if (nam1 == null || nam1 == "") nam1 = DateTime.Now.AddYears(-1).Year.ToString();
            if (nam2 == null || nam2 == "") nam2 = DateTime.Now.Year.ToString();

            if (tuthang1 != null && denthang1 != null && !tuthang1.Equals("") && !denthang1.Equals("") && !nam1.Equals("") && nam1 != null)
            {
                //lay dinh dang ngay
                d1 = DateTime.Parse(nam1 + "-" + tuthang1 + "-01");
                d2 = DateTime.Parse(nam1 + "-" + denthang1 + "-" + DungChung.LaySoNgayTrongThang(denthang1, int.Parse(nam1)));

            }

            if (tuthang2 != null && denthang2 != null && !tuthang2.Equals("") && !denthang2.Equals("") && !nam2.Equals("") && nam2 != null)
            {
                //lay dinh dang ngay
                d3 = DateTime.Parse(nam2 + "-" + tuthang2 + "-01");
                d4 = DateTime.Parse(nam2 + "-" + denthang2 + "-" + DungChung.LaySoNgayTrongThang(denthang2, int.Parse(nam2)));

            }

            List<SelectListItem> lst1 = DungChung.ListThang();
            ViewBag.tuthang1 = new SelectList(lst1, "value", "text", "01");
            ViewBag.denthang1 = new SelectList(lst1, "value", "text", "12");
            ViewBag.tuthang1val = tuthang1;
            ViewBag.denthang1val = denthang1;
            ViewBag.nam1 = nam1;
            ViewBag.tuthang2 = new SelectList(lst1, "value", "text", "01");
            ViewBag.denthang2 = new SelectList(lst1, "value", "text", "12");
            ViewBag.tuthang2val = tuthang2;
            ViewBag.denthang2val = denthang2;
            ViewBag.nam2 = nam2;

            if (d2 < d1)
            {
                SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

                //return RedirectToAction("ShowError", "BCDTPhongKDKD");
            }

            if (d4 < d3)
            {
                SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

                //return RedirectToAction("ShowError", "BCDTPhongKDKD");
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
            dt = dao.BCDoanhThuTheoTuyen2Nam(d1, d2, d3, d4, Session["username"].ToString(), sRoles, dlcn,sCongTyPre);


            return View(dt);
        }

        //public ActionResult dttt2nam(string tungay, string denngay, string tungay1, string denngay1)
        //{
        //    var dao = new baocaoDAO();
        //    DateTime d1 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-1-1");
        //    DateTime d2 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-12-31");


        //    DateTime d3 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
        //    DateTime d4 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");

        //    DataSet dt = null;

        //    string sUserName = Session["userId"].ToString();
        //    string sRoles = Session["RoleName"].ToString();
        //    string sChinhanh = Session["chinhanh"].ToString();


        //    if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
        //    {

        //        //lay dinh dang ngay
        //        d1 = DateTime.Parse(tungay);
        //        d2 = DateTime.Parse(denngay);

        //    }

        //    if (tungay1 != null && denngay1 != null && !tungay1.Equals("") && !denngay1.Equals(""))
        //    {

        //        //lay dinh dang ngay
        //        d3 = DateTime.Parse(tungay1);
        //        d4 = DateTime.Parse(denngay1);

        //    }


        //    ViewBag.tungay = d1.ToString("dd/MM/yyyy");
        //    ViewBag.denngay = d2.ToString("dd/MM/yyyy");
        //    ViewBag.tungay1 = d3.ToString("dd/MM/yyyy");
        //    ViewBag.denngay1 = d4.ToString("dd/MM/yyyy");

        //    if (d2 < d1)
        //    {
        //        SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
        //        ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

        //        //return RedirectToAction("ShowError", "BCDTPhongKDKD");
        //    }

        //    if (d4 < d3)
        //    {
        //        SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
        //        ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

        //        //return RedirectToAction("ShowError", "BCDTPhongKDKD");
        //    }

        //    //ViewBag.CongTyPre = sCongTyPre;//dung xuat excel
        //    dt = dao.BCDoanhThuTheoTuyen2Nam(d1, d2, d3, d4, sUserName, sRoles, sChinhanh);


        //    return View(dt);
        //}

        public ActionResult ShowError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Excel(string tuthang1, string denthang1, string nam1, string tuthang2, string denthang2, string nam2, string schinhanh)
        {

            //tungay = "01-07-2017";
            //denngay = "01-07-2018";
            //CongTy = "";
            //CongTyPre = "STA,STB,STH,STO,STS,STT";
            string Filename = "BCSosanhdoanhsocacnam__" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcel(tuthang1, denthang1, nam1, tuthang2, denthang2, nam2, schinhanh);
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
            return RedirectToAction("~/BCDTPhongKDKD/dtphongkdkd");
        }

        //[HttpGet]
        //public ActionResult Excel(string tungay, string denngay, string tungay1, string denngay1)
        //{

        //    //tungay = "01-07-2017";
        //    //denngay = "01-07-2018";
        //    //CongTy = "";
        //    //CongTyPre = "STA,STB,STH,STO,STS,STT";
        //    string Filename = "BCDoanhSoTheoTuyen2Nam_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

        //    // Gọi lại hàm để tạo file excel
        //    var stream = CreateExcel(tungay, denngay, tungay1, denngay1);
        //    // Tạo buffer memory stream để hứng file excel
        //    var buffer = stream as MemoryStream;
        //    // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
        //    // File name của Excel này là ExcelDemo
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Filename);
        //    // Lưu file excel của chúng ta như 1 mảng byte để trả về response
        //    Response.BinaryWrite(buffer.ToArray());
        //    // Send tất cả ouput bytes về phía clients
        //    Response.Flush();
        //    Response.End();

        //    // Redirect về luôn trang index <img draggable="false" class="emoji" alt="😀" src="https://s0.wp.com/wp-content/mu-plugins/wpcom-smileys/twemoji/2/svg/1f600.svg">
        //    return RedirectToAction("~/BCDTPhongKDKD/dtphongkdkd");
        //}

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, DateTime d3, DateTime d4,string schinhanh)
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


            DataSet ds = dao.BCDoanhThuTheoTuyen2Nam(d1, d2, d3, d4, Session["username"].ToString(), sRoles, schinhanh,sCongTyPre);

            return ds;
        }

        private Stream CreateExcel(string tuthang1, string denthang1, string nam1, string tuthang2, string denthang2, string nam2, string schinhanh, Stream stream = null)
        {
            var dao = new baocaoDAO();
            DateTime d1 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-12-31");


            DateTime d3 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d4 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");

            string sUserName = Session["userId"].ToString();
            string sRoles = Session["RoleName"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();

            if (nam1 == null || nam1 == "") nam1 = DateTime.Now.AddYears(-1).Year.ToString();
            if (nam2 == null || nam2 == "") nam2 = DateTime.Now.Year.ToString();

            if (tuthang1 != null && denthang1 != null && !tuthang1.Equals("") && !denthang1.Equals("") && !nam1.Equals("") && nam1 != null)
            {
                //lay dinh dang ngay
                d1 = DateTime.Parse(nam1 + "-" + tuthang1 + "-01");
                d2 = DateTime.Parse(nam1 + "-" + denthang1 + "-" + DungChung.LaySoNgayTrongThang(denthang1, int.Parse(nam1)));

            }

            if (tuthang2 != null && denthang2 != null && !tuthang2.Equals("") && !denthang2.Equals("") && !nam2.Equals("") && nam2 != null)
            {
                //lay dinh dang ngay
                d3 = DateTime.Parse(nam2 + "-" + tuthang2 + "-01");
                d4 = DateTime.Parse(nam2 + "-" + denthang2 + "-" + DungChung.LaySoNgayTrongThang(denthang2, int.Parse(nam2)));

            }

            DataSet list = GetDuLieuBC(d1, d2, d3, d4, schinhanh);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel 
                string sNam1 = "", sNam2 = "";
                sNam1 = d1.ToString("yyyy");
                sNam2 = d3.ToString("yyyy");
                excelPackage.Workbook.Properties.Title = "SO SÁNH DOANH SỐ THEO TUYẾN " + sNam1 + " & " + sNam2;

                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("bc");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet = FormatWorkSheet(list, workSheet, d1, d3);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        //private Stream CreateExcel(string tungay, string denngay, string tungay1, string denngay1, Stream stream = null)
        //{
        //    DateTime d1 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-1-1");
        //    DateTime d2 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-12-31");

        //    DateTime d3 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
        //    DateTime d4 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");


        //    if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
        //    {
        //        d1 = DateTime.Parse(tungay);
        //        d2 = DateTime.Parse(denngay);
        //    }

        //    if (tungay1 != null && denngay1 != null && !tungay1.Equals("") && !denngay1.Equals(""))
        //    {

        //        //lay dinh dang ngay
        //        d3 = DateTime.Parse(tungay1);
        //        d4 = DateTime.Parse(denngay1);

        //    }

        //    DataSet list = GetDuLieuBC(d1, d2, d3, d4);

        //    using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
        //    {
        //        // Tạo author cho file Excel
        //        excelPackage.Workbook.Properties.Author = "Trung";
        //        // Tạo title cho file Excel 
        //        string sNam1 = "", sNam2 = "";
        //        sNam1 = d1.ToString("yyyy");
        //        sNam2 = d3.ToString("yyyy");
        //        excelPackage.Workbook.Properties.Title = "DOANH SỐ KHÁCH ĐOÀN SO SÁNH " + sNam1 + " & " + sNam2;

        //        // thêm tí comments vào làm màu
        //        excelPackage.Workbook.Properties.Comments = "Comments";
        //        // Add Sheet vào file Excel
        //        excelPackage.Workbook.Worksheets.Add("bc");
        //        // Lấy Sheet bạn vừa mới tạo ra để thao tác
        //        var workSheet = excelPackage.Workbook.Worksheets[1];
        //        // Đổ data vào Excel file
        //        workSheet = FormatWorkSheet(list, workSheet, d1, d3);

        //        //BindingFormatForExcel(workSheet, list);
        //        excelPackage.Save();
        //        return excelPackage.Stream;
        //    }
        //}

        public ExcelWorksheet FormatWorkSheet(DataSet ds, ExcelWorksheet ew, DateTime d1, DateTime d2)
        {
            //DU LIEU = item.ItemArray.Length = 17 =  mahh,tenhh,mapb,tenpb, 13 cot ( 201707-201807)
            
            int iColReport = 5;//so cot bao bieu

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            string sNam1 = "", sNam2 = "";
            sNam1 = d1.ToString("yyyy");
            sNam2 = d2.ToString("yyyy");
            string sReportTitle = "SO SÁNH DOANH SỐ THEO TUYẾN " + sNam1 + " & " + sNam2;

            ewres.Cells[2, 1].Value = sReportTitle;
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].LoadFromText("STT");           
            ewres.Cells[4, 2].LoadFromText("Tuyến tham quan");           
            ewres.Cells[4, 3].LoadFromText("Doanh số năm " + sNam2);
             ewres.Cells[4, 4].LoadFromText("Doanh số năm " + sNam1);
            ewres.Cells[4, 5].LoadFromText("Tỷ lệ");

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
                //var chart = ewres.Drawings.AddChart("doanhThuTuyenChart", eChartType.ColumnClustered);
                //chart.Title.Text = sReportTitle;
                //chart.SetPosition(2, 0, 6, 0);
                //chart.SetSize(600, 800);
                //chart.Border.Fill.Color = System.Drawing.Color.Green;

                DataTable dt = ds.Tables[0];

                decimal dDoanhThu1 = 0, dDoanhThu2 = 0;
                decimal dTyle = 0;
                int iSTT = 1;

                decimal[] dTotal = new decimal[3];

                //lay distinct ten tuyen
                DataView view = new DataView(dt);
                DataTable distinctTuyen = view.ToTable(true, "tuyentq");

                int iRowIndex = 5;

                

                foreach (DataRow row in distinctTuyen.Rows)
                {
                  
                    //lay du lieu theo tuyen
                    DataRow[] rows1 = dt.Select("tuyentq='" + row["tuyentq"].ToString() + "' AND nam='" + sNam1 + "'");//nam1
                    DataRow[] rows2 = dt.Select("tuyentq='" + row["tuyentq"].ToString() + "' AND nam='" + sNam2 + "'");//nam1

                    ewres.Cells[iRowIndex, 1].Value = iSTT;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = row["tuyentq"];
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    if (rows2.Length > 0)
                    {
                        foreach (DataRow r2 in rows2)
                        {
                            dDoanhThu1 = dDoanhThu1+ @Decimal.Parse(r2["doanhthutt"].ToString() == "" ? "0" : r2["doanhthutt"].ToString());
                           // dTotal[0] = dTotal[0] + dDoanhThu1;
                        }
                        dTotal[0] = dTotal[0] + dDoanhThu1;
                    }
                    else
                    {
                        dDoanhThu1 = 0;
                    }

                    if (rows1.Length > 0)
                    {
                        foreach (DataRow r1 in rows1)
                        {
                            dDoanhThu2 = dDoanhThu2+ @Decimal.Parse(r1["doanhthutt"].ToString() == "" ? "0" : r1["doanhthutt"].ToString());
                            //dTotal[1] = dTotal[1] + dDoanhThu2;

                        }
                        dTotal[1] = dTotal[1] + dDoanhThu2;

                    }
                    else
                    {
                        dDoanhThu2 = 0;
                    }

                    if (dDoanhThu2 > 0)
                    {
                        dTyle = dDoanhThu1 / dDoanhThu2;
                    }
                    else
                    {
                        dTyle = 0;
                    }                   

                    ewres.Cells[iRowIndex, 3].Value = dDoanhThu1;
                    ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = dDoanhThu2;
                    ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 5].Value = dTyle;
                    ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "0%";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    //reset
                    dDoanhThu1 = 0;
                    dDoanhThu2 = 0;
                    dTyle = 0;

                    iRowIndex = iRowIndex + 1;
                    iSTT = iSTT + 1;
                }

                ////create the ranges for the chart
                //var rangeLabel = ewres.Cells[$"B{5}:B{iRowIndex-1}"];
                //var range1 = ewres.Cells[$"C{5}:C{iRowIndex - 1}"];
                //var range2 = ewres.Cells[$"D{5}:D{iRowIndex - 1}"];

                ////add the ranges to the chart
                //var series1 = chart.Series.Add(range1, rangeLabel);
                //var series2 = chart.Series.Add(range2, rangeLabel);
                //series1.Header = "Năm " + sNam2;
                //series2.Header = "Năm " + sNam1;

                ////position of the legend                    
                //chart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Bottom;

                if (dTotal[1] > 0)
                {
                    dTotal[2] = dTotal[0] / dTotal[1];
                }
                else
                {
                    dTotal[2] = 0;
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

                ewres.Cells[iRowIndex, 5].Value = dTotal[2];
                ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "0%";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);


            }



            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}