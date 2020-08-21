using qlkdst.Common;
using qlkdstDB.DAO;
using System;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml;
using System.Data;
using qlkdstDB.EF;
using System.Collections.Generic;

namespace qlkdst.Controllers
{
    public class BCCPHHController : BaseController
    {
        // GET: BCCPHH
        public ActionResult baocaocphoahong(string tungay, string denngay, string dlcn)
        {
            var dao = new baocaoDAO();
            //DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
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

            dt = dao.BCCPHH(d1, d2, Session["username"].ToString(), sRoles, dlcn, sCongTyPre);


            return View(dt);
        }

        public ActionResult Excel(string tungay, string denngay, string schinhanh)
        {
            string Filename = "BCCPHH_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

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
            return RedirectToAction("~/BCCPHH/Index");
        }

        private DataSet GetDuLieuBC(DateTime d1, DateTime d2, string schinhanh)
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

            DataSet ds = dao.BCCPHH(d1, d2, Session["username"].ToString(), sRoles, schinhanh, sCongTyPre);

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

            DataSet list = GetDuLieuBC(d1, d2, schinhanh);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel              
                excelPackage.Workbook.Properties.Title = "BÁO CÁO CHI PHÍ HOA HỒNG";

                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("bc");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet = FormatWorkSheet(list, workSheet, d1, d2, schinhanh);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }



        public ExcelWorksheet FormatWorkSheet(DataSet ds, ExcelWorksheet ew, DateTime d1, DateTime d2, string schinhanh)
        {

            int iColReport = 7;

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");
            Color colorThanhLy = System.Drawing.ColorTranslator.FromHtml("#7FFF00");
            Color colorChuaThanhLy = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            ewres.Cells[2, 1].Value = "BÁO CÁO CHI PHÍ HOA HỒNG " + d1.ToString("dd/MM/yyyy") + " - " + d2.ToString("dd/MM/yyyy");
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].LoadFromText("STT");
            ewres.Cells[4, 2].LoadFromText("Code đoàn");
            ewres.Cells[4, 3].LoadFromText("Thời gian");
            ewres.Cells[4, 4].LoadFromText("Sales");
            ewres.Cells[4, 5].LoadFromText("Khách hưởng hoa hồng");
            ewres.Cells[4, 6].LoadFromText("CMND");
            ewres.Cells[4, 7].LoadFromText("Hoa hồng thực tế nhận");


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
                DataView view = new DataView(dt);
                DataTable distinctSgtCode = view.ToTable(true, "sgtcode");

                decimal[] dTotal = new decimal[1];
                decimal dSoTien = 0;
                int[] iArrSoDongTheoMoiSgtCode = new int[distinctSgtCode.Rows.Count];
                int[] iArrVitriBatDauRowspan = new int[distinctSgtCode.Rows.Count];
                int iIndexArrRowspan = 0;

                int iRowIndex = 5;

                foreach (DataRow r in distinctSgtCode.Rows)
                {

                    DataRow[] rows = dt.Select("sgtcode='" + r["sgtcode"].ToString() + "'");

                    iArrSoDongTheoMoiSgtCode[iIndexArrRowspan] = rows.Length;

                    for (int i = 0; i < rows.Length; i++)
                    {
                        string sbatdau = rows[i]["batdau"].ToString() == "" ? "" : DateTime.Parse(rows[i]["batdau"].ToString()).ToString("dd/MM");
                        string sketthuc = rows[i]["ketthuc"].ToString() == "" ? "" : DateTime.Parse(rows[i]["ketthuc"].ToString()).ToString("dd/MM/yyyy");
                        string sngay = sbatdau + "~" + sketthuc;
                        dSoTien = Decimal.Parse(rows[i]["sotien"].ToString() == "" ? "0" : @rows[i]["sotien"].ToString());
                        dTotal[0] = dTotal[0] + dSoTien;

                        if (i == 0)
                        {
                            iArrVitriBatDauRowspan[iIndexArrRowspan] = iRowIndex;
                        }

                        ewres.Cells[iRowIndex, 1].Value = idem.ToString();
                        ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 2].Value = rows[i]["sgtcode"].ToString();
                        ewres.Cells[iRowIndex, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 3].Value = sngay;
                        ewres.Cells[iRowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 4].Value = rows[i]["salesnm"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 5].Value = rows[i]["tenkhach"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 6].Value = rows[i]["socmnd"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 7].Value = dSoTien;
                        ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        iRowIndex = iRowIndex + 1;


                    }
                    idem = idem + 1;
                    iIndexArrRowspan = iIndexArrRowspan + 1;
                }




                //them dong tong
                ewres.Cells[iRowIndex, 1].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 2].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 3].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 4].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 5].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 6].Value = "TỔNG CỘNG";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 7].Value = dTotal[0];
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                //merce cell
                for (int i = 0; i < iArrVitriBatDauRowspan.Length; i++)
                {
                    ewres.Cells[iArrVitriBatDauRowspan[i], 1, iArrVitriBatDauRowspan[i] + iArrSoDongTheoMoiSgtCode[i] - 1, 1].Merge = true;
                    ewres.Cells[iArrVitriBatDauRowspan[i], 2, iArrVitriBatDauRowspan[i] + iArrSoDongTheoMoiSgtCode[i] - 1, 2].Merge = true;
                    ewres.Cells[iArrVitriBatDauRowspan[i], 3, iArrVitriBatDauRowspan[i] + iArrSoDongTheoMoiSgtCode[i] - 1, 3].Merge = true;
                }
            }

            ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}