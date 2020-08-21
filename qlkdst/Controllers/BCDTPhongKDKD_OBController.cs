using qlkdst.Common;
using qlkdstDB.DAO;
using System;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml;
using System.Data;

namespace qlkdst.Controllers
{
    public class BCDTPhongKDKD_OBController : BaseController
    {
        // GET: BCDTPhongKDKD_OB
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult dtphongkdkdob(string tungay, string denngay, string tungay1, string denngay1)
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


            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {

                //lay dinh dang ngay
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);

            }

            if (tungay1 != null && denngay1 != null && !tungay1.Equals("") && !denngay1.Equals(""))
            {

                //lay dinh dang ngay
                d3 = DateTime.Parse(tungay1);
                d4 = DateTime.Parse(denngay1);

            }


            ViewBag.tungay = d1.ToString("dd/MM/yyyy");
            ViewBag.denngay = d2.ToString("dd/MM/yyyy");
            ViewBag.tungay1 = d3.ToString("dd/MM/yyyy");
            ViewBag.denngay1 = d4.ToString("dd/MM/yyyy");

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

            //ViewBag.CongTyPre = sCongTyPre;//dung xuat excel
            dt = dao.BCDoanhThuTheoPhongKDKD(d1, d2, d3, d4, sUserName, sRoles, sChinhanh);


            return View(dt);
        }

        public ActionResult ShowError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Excel(string tungay, string denngay, string tungay1, string denngay1)
        {

            //tungay = "01-07-2017";
            //denngay = "01-07-2018";
            //CongTy = "";
            //CongTyPre = "STA,STB,STH,STO,STS,STT";
            string Filename = "BCDoanhThuTheoPhongKDKD_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcel(tungay, denngay, tungay1, denngay1);
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

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, DateTime d3, DateTime d4)
        {
            var dao = new baocaoDAO();

            string sUserName = Session["userId"].ToString();
            string sRoles = Session["RoleName"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();

            DataSet ds = dao.BCDoanhThuTheoPhongKDKD(d1, d2, d3, d4, sUserName, sRoles, sChinhanh);

            return ds;
        }

        private Stream CreateExcel(string tungay, string denngay, string tungay1, string denngay1, Stream stream = null)
        {
            DateTime d1 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.AddDays(-365).ToString("yyyy") + "-12-31");

            DateTime d3 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d4 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");


            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);
            }

            if (tungay1 != null && denngay1 != null && !tungay1.Equals("") && !denngay1.Equals(""))
            {

                //lay dinh dang ngay
                d3 = DateTime.Parse(tungay1);
                d4 = DateTime.Parse(denngay1);

            }

            DataSet list = GetDuLieuBC(d1, d2, d3, d4);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel              
                excelPackage.Workbook.Properties.Title = "BÁO CÁO DOANH THU THEO PHÒNG KINH DOANH KHÁCH ĐOÀN OUTBOUND";

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

            int iColReport = 10;//so cot bao bieu

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            ewres.Cells[2, 1].Value = "BÁO CÁO DOANH THU PHÒNG KINH DOANH KHÁCH ĐOÀN OUTBOUND";
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            string sNam1 = "", sNam2 = "";
            sNam1 = d1.ToString("yyyy");
            sNam2 = d2.ToString("yyyy");

            ewres.Cells[4, 1].LoadFromText("STT");
            //ewres.Cells[4, 1, 5, 1].Merge = true;
            ewres.Cells[4, 2].LoadFromText("Tháng");
            //ewres.Cells[4, 2, 5, 2].Merge = true;
            ewres.Cells[4, 3].LoadFromText("Số khách năm " + sNam1);
            ewres.Cells[4, 4].LoadFromText("Doanh số năm " + sNam1);
            ewres.Cells[4, 5].LoadFromText("Doanh thu năm " + sNam1);
            ewres.Cells[4, 6].LoadFromText("Số khách năm " + sNam2);
            ewres.Cells[4, 7].LoadFromText("Doanh số năm " + sNam2);
            ewres.Cells[4, 8].LoadFromText("Doanh thu năm " + sNam2);
            ewres.Cells[4, 9].LoadFromText("Tỉ lệ SK");
            ewres.Cells[4, 10].LoadFromText("Tỉ lệ DT");

            //ewres.Cells[5, 3].LoadFromText("A");
            //ewres.Cells[5, 4].LoadFromText("B");
            //ewres.Cells[5, 5].LoadFromText("B");
            //ewres.Cells[5, 6].LoadFromText("C");
            //ewres.Cells[5, 7].LoadFromText("D");
            //ewres.Cells[5, 8].LoadFromText("D");
            //ewres.Cells[5, 9].LoadFromText("C/A");
            //ewres.Cells[5, 10].LoadFromText("C/B");


            //create header
            // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            using (var range = ewres.Cells[4, 1, 5, iColReport])
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

                DataTable dt1 = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];

                decimal[] dTotal = new decimal[6];
                int iRowIndex = 6;


                for (int i = 1; i <= 12; i++)
                {
                    decimal dSK = 0, dDS = 0, dDT = 0;
                    decimal dSK1 = 0, dDS1 = 0, dDT1 = 0;

                    ewres.Cells[iRowIndex, 1].Value = i.ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = "Tháng " + i.ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    DataRow[] rs = dt1.Select("thang='" + i + "' AND nam='" + sNam1 + "'");
                    DataRow[] rs2 = dt2.Select("thang='" + i + "' AND nam='" + sNam2 + "'");

                    for (int i1 = 0; i1 < rs.Length; i1++)
                    {
                        dSK = dSK + @Decimal.Parse(@rs[i1]["sokhach"].ToString() == "" ? "0" : @rs[i1]["sokhach"].ToString());
                        dDS = dDS + @Decimal.Parse(@rs[i1]["doanhso"].ToString() == "" ? "0" : @rs[i1]["doanhso"].ToString());
                        dDT = dDS;
                        dTotal[0] += dSK;
                        dTotal[1] += dDS;
                        dTotal[2] += dDT;
                    }

                    ewres.Cells[iRowIndex, 3].Value = dSK; ;
                    ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                    ewres.Cells[iRowIndex, 4].Value = dDS;
                    ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                    ewres.Cells[iRowIndex, 5].Value = dDT;
                    ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    for (int i2 = 0; i2 < rs2.Length; i2++)
                    {
                        dSK1 = dSK1 + @Decimal.Parse(@rs2[i2]["sokhach"].ToString() == "" ? "0" : @rs2[i2]["sokhach"].ToString());
                        dDS1 = dDS1 + @Decimal.Parse(@rs2[i2]["doanhso"].ToString() == "" ? "0" : @rs2[i2]["doanhso"].ToString());
                        dDT1 = dDS1;

                        dTotal[3] += dSK1;
                        dTotal[4] += dDS1;
                        dTotal[5] += dDT1;
                    }


                    ewres.Cells[iRowIndex, 6].Value = dSK1;
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                    ewres.Cells[iRowIndex, 7].Value = dDS1;
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                    ewres.Cells[iRowIndex, 8].Value = dDT1;
                    ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    decimal dTyleCA = 0, dTyleDB = 0;

                    if (dSK > 0)
                    {
                        dTyleCA = dSK1 / dSK;
                    }
                    else
                    {
                        dTyleCA = 0;
                    }

                    if (dDT > 0)
                    {
                        dTyleDB = dDT1 / dDT;
                    }
                    else
                    {
                        dTyleDB = 0;
                    }

                    ewres.Cells[iRowIndex, 9].Value = dTyleCA;
                    ewres.Cells[iRowIndex, 9].Style.Numberformat.Format = "0%";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 10].Value = dTyleDB;
                    ewres.Cells[iRowIndex, 10].Style.Numberformat.Format = "0%";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    iRowIndex = iRowIndex + 1;
                }

                //add total row
                ewres.Cells[iRowIndex, 1].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                ewres.Cells[iRowIndex, 2].Value = "Tổng Cộng";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 3].Value = dTotal[0];
                ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 4].Value = dTotal[1];
                ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 5].Value = dTotal[2];
                ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 6].Value = dTotal[3];
                ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 7].Value = dTotal[4];
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 8].Value = dTotal[5];
                ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);


                ewres.Cells[iRowIndex, 9].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                ewres.Cells[iRowIndex, 10].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

            }

            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}