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
    public class BCTController : BaseController
    {
        // GET: BCT
        public ActionResult baocaot(string tungay, string denngay, string trangthai,string dlcn)
        {
            var dao = new baocaoDAO();
            //DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            //DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");
            //DateTime d2 = DateTime.Now;

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

            if (trangthai == null) trangthai = "";

            ViewBag.tungay = d1.ToString("dd/MM/yyyy");
            ViewBag.denngay = d2.ToString("dd/MM/yyyy");
            ViewBag.trangthai = trangthai;
            // ViewBag.LstTrangThai = DungChung.ListTrangThaiTour();
            if (d2 < d1)
            {
                SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");

                return RedirectToAction("ShowError", "BaoCao");
            }

            List<chinhanh> lst = DungChung.LayChiNhanhTheoUser(sUserName);
            //ViewBag.dlcn = new SelectList(DungChung.LayDSChiNhanhTheoUser(sUserName), "Value", "Text", sChinhanh);//mac dinh lay chinhanh cua user de bot du lieu
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

            dt = dao.BCTOUR(d1, d2, Session["username"].ToString(), sRoles, dlcn, trangthai,sCongTyPre);


            return View(dt);
        }

        public ActionResult ShowError()
        {
            return View();
        }

        public ActionResult Excel(string tungay, string denngay, string trangthai, string schinhanh)
        {
            string Filename = "BCTOUR_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcel(tungay, denngay, trangthai, schinhanh);
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

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, string trangthai,string schinhanh)
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

            DataSet ds = dao.BCTOUR(d1, d2, Session["username"].ToString(), sRoles, schinhanh, trangthai, sCongTyPre);

            return ds;
        }

        private Stream CreateExcel(string tungay, string denngay, string trangthai,string schinhanh, Stream stream = null)
        {
            DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");


            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);
            }

            if (trangthai == null) trangthai = "";

            DataSet list = GetDuLieuBC(d1, d2, trangthai, schinhanh);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel              
                excelPackage.Workbook.Properties.Title = "BÁO CÁO TRẠNG THÁI CÁC TOUR";

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

            //int iColReport = 7;
            int iColReport = 10;//add 29082019 : ten khach hang, so khach , doanh so

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");
            Color colorThanhLy = System.Drawing.ColorTranslator.FromHtml("#7FFF00");
            Color colorChuaThanhLy = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            ewres.Cells[2, 1].Value = "BÁO CÁO TRẠNG THÁI CÁC TOUR " + d1.ToString("dd/MM/yyyy") + " - " + d2.ToString("dd/MM/yyyy");
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].LoadFromText("STT");
            ewres.Cells[4, 2].LoadFromText("Code đoàn");
            ewres.Cells[4, 3].LoadFromText("Tên công ty/Khách hàng");
            ewres.Cells[4, 4].LoadFromText("Tuyến tham quan");
            ewres.Cells[4, 5].LoadFromText("Thời gian");
            ewres.Cells[4, 6].LoadFromText("Số khách");
            ewres.Cells[4, 7].LoadFromText("Doanh số");
            ewres.Cells[4, 8].LoadFromText("Sales");
            ewres.Cells[4, 9].LoadFromText("Nguyên nhân hủy tour");
            ewres.Cells[4, 10].LoadFromText("Trạng thái");



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
                int idem = 1;


                DataTable dt = ds.Tables[0];
                decimal[] aTotal = new decimal[2];

                int iRowIndex = 5;

                foreach (DataRow item in dt.Rows)
                {

                    string sbatdau = item["batdau"].ToString() == "" ? "" : DateTime.Parse(item["batdau"].ToString()).ToString("dd/MM");
                    string sketthuc = item["ketthuc"].ToString() == "" ? "" : DateTime.Parse(item["ketthuc"].ToString()).ToString("dd/MM/yyyy");
                    string sngay = sbatdau + "~" + sketthuc;
                    aTotal[0] += item["sokhach"].ToString() == "" ? 0 : Decimal.Parse(item["sokhach"].ToString());
                    aTotal[1] += item["doanhso"].ToString() == "" ? 0 : Decimal.Parse(item["doanhso"].ToString());


                    ewres.Cells[iRowIndex, 1].Value = idem.ToString();
                    ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 2].Value = item["sgtcode"].ToString();
                    ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    //Set background-color
                    ewres.Cells[iRowIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    if (item["trangthai"].ToString() == "3")
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(colorThanhLy);
                    }
                    else if (item["trangthai"].ToString() == "4")
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    }
                    else if (item["trangthai"].ToString() == "2")
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    }
                    else
                    {
                        ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.White);
                    }

                    ewres.Cells[iRowIndex, 3].Value = item["tenkh"].ToString();
                    ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                                        

                    ewres.Cells[iRowIndex, 4].Value = item["diemtq"].ToString();
                    ewres.Cells[iRowIndex, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    ewres.Cells[iRowIndex, 5].Value = sngay;                    
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                                        
                    ewres.Cells[iRowIndex, 6].Value = Decimal.Parse(item["sokhach"].ToString() == "" ? "0" : @item["sokhach"].ToString());
                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 7].Value = Decimal.Parse(item["doanhso"].ToString() == "" ? "0" : @item["doanhso"].ToString());
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";                   
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 8].Value = item["salesnm"].ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 9].Value = item["nguyennhanhuythau"].ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    string sTT = "";
                    sTT=DungChung.GetTenTrangThaiTour(item["trangthai"].ToString());

                    //if (item["trangthai"].ToString() == "0")
                    //{
                    //    sTT = "Mới tạo";
                    //}
                    //else if (item["trangthai"].ToString() == "1")
                    //{
                    //    sTT = "Mới đàm phán";
                    //}
                    //else if (item["trangthai"].ToString() == "2")
                    //{
                    //    sTT = "Đã ký hợp đồng";
                    //}
                    //else if (item["trangthai"].ToString() == "3")
                    //{
                    //    sTT = "Đã thanh lý hợp đồng";
                    //}
                    //else if (item["trangthai"].ToString() == "4")
                    //{
                    //    sTT = "Đã hủy tour";
                    //}
                    //else
                    //{
                    //    sTT = "";
                    //}

                    ewres.Cells[iRowIndex, 10].Value = sTT;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                    iRowIndex = iRowIndex + 1;

                    idem = idem + 1;

                }

                //add total row               

                ewres.Cells[iRowIndex, 1].Value = "";                
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 2].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 3].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 4].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 5].Value = "Tổng cộng";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 6].Value = aTotal[0];
                ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 7].Value = aTotal[1];
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 8].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 9].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                ewres.Cells[iRowIndex, 10].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

            }

            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}