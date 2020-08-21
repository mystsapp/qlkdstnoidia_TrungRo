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
    public class BCDTSKGTBQController : BaseController
    {
        // GET: BCDTSKGTBQ
        public ActionResult dtskgiatourbq(string tuthang1, string denthang1, string nam1, string tuthang2, string denthang2, string nam2, string dlcn)
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

            List<SelectListItem> lstTh = DungChung.ListThang();
            ViewBag.tuthang1 = new SelectList(lstTh, "value", "text", "01");
            ViewBag.denthang1 = new SelectList(lstTh, "value", "text", "12");
            ViewBag.tuthang1val = tuthang1;
            ViewBag.denthang1val = denthang1;
            ViewBag.nam1 = nam1;
            ViewBag.tuthang2 = new SelectList(lstTh, "value", "text", "01");
            ViewBag.denthang2 = new SelectList(lstTh, "value", "text", "12");
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
            dt = dao.BCDTSKGiaTourBQ(d1, d2, d3, d4, Session["username"].ToString(), sRoles, dlcn, sCongTyPre);


            return View(dt);
        }

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
            string Filename = "BCDTSKGiaTour_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

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
            return RedirectToAction("~/BCDTSKTheoTuyen/dtsktheotuyen");
        }

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, DateTime d3, DateTime d4, string schinhanh)
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

            DataSet ds = dao.BCDTSKGiaTourBQ(d1, d2, d3, d4, Session["username"].ToString(), sRoles, schinhanh, sCongTyPre);

            return ds;
        }

        private Stream CreateExcel(string tuthang1, string denthang1, string nam1, string tuthang2, string denthang2, string nam2, string schinhanh, Stream stream = null)
        {
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
                excelPackage.Workbook.Properties.Title = "SO SÁNH DOANH THU, SỐ KHÁCH VÀ GIÁ TOUR BÌNH QUÂN GIỮA CÁC NĂM " + sNam1 + " & " + sNam2;

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

        public ExcelWorksheet FormatWorkSheet(DataSet ds, ExcelWorksheet ew, DateTime d1, DateTime d2)
        {
            //DU LIEU = item.ItemArray.Length = 17 =  mahh,tenhh,mapb,tenpb, 13 cot ( 201707-201807)

            int iColReport = 8;//so cot bao bieu

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
            string sReportTitle = "SO SÁNH DOANH THU, SỐ KHÁCH VÀ GIÁ TOUR BÌNH QUÂN GIỮA CÁC NĂM " + sNam1 + " & " + sNam2;

            ewres.Cells[2, 1].Value = sReportTitle;
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].LoadFromText("STT");
            ewres.Cells[4, 2].LoadFromText("Tuyến");
            ewres.Cells[4, 3].LoadFromText("Số khách năm " + sNam1);
            ewres.Cells[4, 4].LoadFromText("Doanh thu tuyến " + sNam1);
            ewres.Cells[4, 5].LoadFromText("Giá tour bình quân" + sNam1);
            ewres.Cells[4, 6].LoadFromText("Số khách năm " + sNam2);
            ewres.Cells[4, 7].LoadFromText("Doanh thu " + sNam2);
            ewres.Cells[4, 8].LoadFromText("Giá tour bình quân" + sNam2);


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
                int iSTT = 1;
                decimal[] dTotal = new decimal[2];
                decimal[] dTotalSK = new decimal[2];

                //lay distinct theo khu
                DataView view = new DataView(dt);
                view.Sort = "tenkhu";
                DataTable distinctKhu = view.ToTable(true, "tenkhu", "tuyentq");
                DataTable distinctTuyen = view.ToTable(true, "tuyentq");

                int iRowIndex = 5;

                // foreach (DataRow row in distinctKhu.Rows)
                foreach (DataRow row in distinctTuyen.Rows)
                {
                    DataRow[] rows = dt.Select("tuyentq='" + row["tuyentq"].ToString() + "'");

                    ewres.Cells[iRowIndex, 1].Value = iSTT;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = row["tuyentq"].ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    DataRow[] rowss = dt.Select("tuyentq='" + row["tuyentq"].ToString() + "' AND nam='" + sNam1 + "'");
                    DataRow[] rowsss = dt.Select("tuyentq='" + row["tuyentq"].ToString() + "' AND nam='" + sNam2 + "'");

                    // foreach (DataRow rSS in rowss)
                    // {
                    decimal dSK = 0, dDS = 0, dGia = 0;


                    if (rowss.Length > 0)
                    {
                        foreach (DataRow r in rowss)
                        {
                            dSK = dSK + Decimal.Parse(r["sokhachtt"].ToString() == "" ? "0" : r["sokhachtt"].ToString());
                            dDS = dDS + Decimal.Parse(r["doanhthutt"].ToString() == "" ? "0" : r["doanhthutt"].ToString());
                            dGia = dGia + Decimal.Parse(r["giabinhquan"].ToString() == "" ? "0" : r["giabinhquan"].ToString());

                        }


                        //dSK = Decimal.Parse(rowss[0]["sokhachtt"].ToString() == "" ? "0" : rowss[0]["sokhachtt"].ToString());
                        //dDS = Decimal.Parse(rowss[0]["doanhthutt"].ToString() == "" ? "0" : rowss[0]["doanhthutt"].ToString());
                        //dGia = Decimal.Parse(rowss[0]["giabinhquan"].ToString() == "" ? "0" : rowss[0]["giabinhquan"].ToString());

                        dTotal[0] = dTotal[0] + dDS;
                        dTotalSK[0] = dTotalSK[0] + dSK;
                    }
                    else
                    {
                        dSK = 0;
                        dDS = 0;
                        dGia = 0;

                        dTotal[0] = dTotal[0] + dDS;
                        dTotalSK[0] = dTotalSK[0] + dSK;
                    }


                    ewres.Cells[iRowIndex, 3].Value = dSK;
                    ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 4].Value = dDS;
                    ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 5].Value = dGia;
                    ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    decimal dSK1 = 0, dDS1 = 0, dGia1 = 0;

                    if (rowsss.Length > 0)
                    {
                        foreach (DataRow r in rowsss)
                        {
                            dSK1 = dSK1 + Decimal.Parse(r["sokhachtt"].ToString() == "" ? "0" : r["sokhachtt"].ToString());
                            dDS1 = dDS1 + Decimal.Parse(r["doanhthutt"].ToString() == "" ? "0" : r["doanhthutt"].ToString());
                            dGia1 = dGia1 + Decimal.Parse(r["giabinhquan"].ToString() == "" ? "0" : r["giabinhquan"].ToString());
                        }

                        //dSK1 = Decimal.Parse(rowsss[0]["sokhachtt"].ToString() == "" ? "0" : rowsss[0]["sokhachtt"].ToString());
                        //dDS1 = Decimal.Parse(rowsss[0]["doanhthutt"].ToString() == "" ? "0" : rowsss[0]["doanhthutt"].ToString());
                        //dGia1 = Decimal.Parse(rowsss[0]["giabinhquan"].ToString() == "" ? "0" : rowsss[0]["giabinhquan"].ToString());

                        dTotal[1] = dTotal[1] + dDS1;
                        dTotalSK[1] = dTotalSK[1] + dSK1;
                    }
                    else
                    {
                        dSK1 = 0;
                        dDS1 = 0;
                        dGia1 = 0;
                        dTotal[1] = dTotal[1] + dDS1;
                        dTotalSK[1] = dTotalSK[1] + dSK1;
                    }

                    ewres.Cells[iRowIndex, 6].Value = dSK1;
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 7].Value = dDS1;
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 8].Value = dGia1;
                    ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    iSTT = iSTT + 1;
                    iRowIndex = iRowIndex + 1;

                }//end row

                //add total row
                decimal dBQ = 0, dBQ1 = 0;

                dBQ = dTotalSK[0] == 0 ? 0 : dTotal[0] / dTotalSK[0];
                dBQ1 = dTotalSK[1] == 0 ? 0 : dTotal[1] / dTotalSK[1];


                ewres.Cells[iRowIndex, 1].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                ewres.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 3].Value = dTotalSK[0];
                ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 4].Value = dTotal[0];
                ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 5].Value = dBQ;
                ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 6].Value = dTotalSK[1];
                ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 7].Value = dTotal[1];
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 8].Value = dBQ1;
                ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);


            }


            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}