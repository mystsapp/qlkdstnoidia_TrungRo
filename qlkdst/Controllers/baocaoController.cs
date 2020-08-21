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
using OfficeOpenXml.Drawing.Chart;

namespace qlkdst.Controllers
{
    public class baocaoController : BaseController
    {
        // GET: baocao
        public ActionResult Index()
        {
            return View();
        }

        #region "Doanh số theo tuyen"

        public ActionResult doanhthutheotuyen(string tungay, string denngay, string dlcn)
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

            ViewBag.dlcn = new SelectList(DungChung.LayDSChiNhanhTheoUser(sUserName), "Value", "Text");
            List<chinhanh> lst = DungChung.LayChiNhanhTheoUser(sUserName);

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

            dt = dao.BCDoanhThuTheoTuyen(d1, d2, Session["username"].ToString(), sRoles, dlcn, sCongTyPre);


            return View(dt);
        }

        public ActionResult ExcelTheoTuyen(string tungay, string denngay, string schinhanh)
        {

            //tungay = "01-07-2017";
            //denngay = "01-07-2018";
            //CongTy = "";
            //CongTyPre = "STA,STB,STH,STO,STS,STT";
            string Filename = "BCDoanhThuTheoTuyen_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelTheoTuyen(tungay, denngay, schinhanh);
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

        private DataSet GetDuLieuBCTheoTuyen(DateTime d1, DateTime d2, string schinhanh)
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

            DataSet ds = dao.BCDoanhThuTheoTuyen(d1, d2, Session["username"].ToString(), sRoles, schinhanh,sCongTyPre);

            return ds;
        }

        private Stream CreateExcelTheoTuyen(string tungay, string denngay, string schinhanh, Stream stream = null)
        {
            DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");


            if (tungay != null && denngay != null && !tungay.Equals("") && !denngay.Equals(""))
            {
                d1 = DateTime.Parse(tungay);
                d2 = DateTime.Parse(denngay);
            }

            DataSet list = GetDuLieuBCTheoTuyen(d1, d2, schinhanh);

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel              
                excelPackage.Workbook.Properties.Title = "BÁO CÁO DOANH SỐ THEO TUYẾN";

                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("bc");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet = FormatWorkSheetTheoTuyen(list, workSheet, d1, d2);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }



        public ExcelWorksheet FormatWorkSheetTheoTuyen(DataSet dt, ExcelWorksheet ew, DateTime d1, DateTime d2)
        {

            int iColReport = 8;

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");
            Color colorThanhLy = System.Drawing.ColorTranslator.FromHtml("#7FFF00");
            Color colorChuaThanhLy = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            ewres.Cells[2, 1].Value = "BÁO CÁO DOANH SỐ THEO TUYẾN";
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "THỐNG KÊ THEO TUYẾN " + d1.ToString("dd/MM/yyyy") + " - " + d2.ToString("dd/MM/yyyy");
            ewres.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[4, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            ewres.Cells[4, 1, 4, iColReport].Merge = true;

            ewres.Cells[5, 1].LoadFromText("STT");
            ewres.Cells[5, 2].LoadFromText("Tuyến");
            ewres.Cells[5, 3].LoadFromText("Số khách");
            ewres.Cells[5, 4].LoadFromText("Doanh số tuyến");
            ewres.Cells[5, 5].LoadFromText("Tỉ trọng Doanh số theo tuyến (%)");
            ewres.Cells[5, 6].LoadFromText("Doanh số khu vực");
            ewres.Cells[5, 7].LoadFromText("Tỉ trọng Doanh số theo khu vực (%)");
            ewres.Cells[5, 8].LoadFromText("Tỉ trọng Doanh số đường xa / đường gần (%)");


            //create header
            // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            using (var range = ewres.Cells[5, 1, 5, iColReport])
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

            if (dt.Tables.Count > 3)
            {
                DataTable dt0 = dt.Tables[0];
                DataTable dt1 = dt.Tables[1];//3 tuyến có số khách đông nhất :
                DataTable dt2 = dt.Tables[2];//3 tuyến có tỷ trọng Doanh số cao nhất
                DataTable dt3 = dt.Tables[3];//3 khu co so khach cao nhat
                DataTable dt4 = dt.Tables[4];// 3 khu vực có tỉ trọng Doanh số cao nhất:

                decimal dDoanhThuTT = 0, dSKTT = 0, dTotalDoanhThuTT = 0, dTotalSKTT = 0;
                decimal dTotalDoanhThuTheoKhu = 0;
                decimal dTotalTyTrongTheoTuyen = 0;
                decimal dTyTrongTheoTuyen = 0, dTotalTytrongtheokhu = 0;
                decimal dTotalTytrongtheophamvi = 0;
                DataView view = new DataView(dt0);
                view.Sort = "phamvi";
                //distinct theo pham vi
                DataTable distinctPhamvi = view.ToTable(true, "phamvi");

                //distinct theo khu

                DataTable distinctValues = view.ToTable(true, "tenkhu");//lay distinct ten cac khu vuc

                //cho tinh tong
                foreach (DataRow item in dt0.Rows)
                {
                    dDoanhThuTT = @Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : @item["doanhthutt"].ToString());
                    dSKTT = @Decimal.Parse(item["sokhachtt"].ToString() == "" ? "0" : @item["sokhachtt"].ToString());
                    dTotalDoanhThuTT = dTotalDoanhThuTT + dDoanhThuTT;
                    dTotalSKTT = dTotalSKTT + dSKTT;
                }
                int iRowIndex = 6;
                int idem = 1;

                int[] iArrViTriMergeCellPHamvi = new int[distinctPhamvi.Rows.Count];
                int[] iArrSodongTheoPhamvi = new int[distinctPhamvi.Rows.Count];

                int[] iArrViTriMergeCell = new int[distinctValues.Rows.Count];
                int[] iArrSodongMoiKhu = new int[distinctValues.Rows.Count];

                int iDemPhamVi = 0;

                foreach (DataRow rpv in distinctPhamvi.Rows)
                {
                    //TINH TONG

                    decimal[] dTotalTheoPhamVi = new decimal[2];
                    int iIndexTheoPhamVi = 0;
                    bool bMergedCellPhamVi = false;
                    //TINH TONG GAN XA HAY THEO PHAM VI
                    DataRow[] rowspv = dt0.Select("phamvi='" + rpv["phamvi"].ToString() + "'");
                    int iSodongMoiPHamVi = rowspv.Length;
                    foreach (DataRow r1 in rowspv)
                    {
                        dTotalTheoPhamVi[iIndexTheoPhamVi] = dTotalTheoPhamVi[iIndexTheoPhamVi] + Decimal.Parse(r1["doanhthutt"].ToString() == "" ? "0" : r1["doanhthutt"].ToString());
                    }

                    //tinh tong khu
                    int iIndexTheoKhu = 0;
                    decimal[] dTotalTheoKhu = new decimal[distinctValues.Rows.Count];

                    foreach (DataRow rr in distinctValues.Rows)
                    {

                        DataRow[] rows = dt0.Select("phamvi='" + rpv["phamvi"].ToString() + "' AND tenkhu='" + rr["tenkhu"].ToString() + "'");

                        foreach (DataRow item in rows)
                        {
                            dDoanhThuTT = Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : item["doanhthutt"].ToString());
                            dTotalTheoKhu[iIndexTheoKhu] = dTotalTheoKhu[iIndexTheoKhu] + dDoanhThuTT;

                        }
                        iIndexTheoKhu = iIndexTheoKhu + 1;
                    }//end khu

                    //HIEN THI
                    iIndexTheoKhu = 0;

                    decimal dTytrongtheokhu = 0;

                    foreach (DataRow rr in distinctValues.Rows)
                    {
                        if (rr["tenkhu"].ToString() != "")
                        {
                            DataRow[] rows = dt0.Select("phamvi='" + rpv["phamvi"].ToString() + "' AND tenkhu='" + rr["tenkhu"].ToString() + "'");
                            int iSoDongMoiKhu = rows.Length;
                            int iLastRow = 1;//co de hien ty trong o dong cuoi cung theo khu
                            int iIndexTenKhu = 0;

                            foreach (DataRow item in rows)
                            {
                                dDoanhThuTT = Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : item["doanhthutt"].ToString());
                                dTyTrongTheoTuyen = dDoanhThuTT / dTotalDoanhThuTT;
                                dTytrongtheokhu = dTotalTheoKhu[iIndexTheoKhu] / dTotalDoanhThuTT;
                                dTotalTyTrongTheoTuyen = dTotalTyTrongTheoTuyen + dTyTrongTheoTuyen;

                                ewres.Cells[iRowIndex, 1].Value = idem.ToString();
                                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                                ewres.Cells[iRowIndex, 2].Value = item["tuyentq"].ToString();
                                //  ewres.Cells[iRowIndex, 2].Style.Numberformat.Format = "#,##0";
                                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                                ewres.Cells[iRowIndex, 3].Value = Decimal.Parse(item["sokhachtt"].ToString() == "" ? "0" : @item["sokhachtt"].ToString());
                                ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                                ewres.Cells[iRowIndex, 4].Value = dDoanhThuTT;
                                ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                                ewres.Cells[iRowIndex, 5].Value = dTyTrongTheoTuyen;
                                ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "#0.0%";
                                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                                dTotalDoanhThuTheoKhu = dTotalDoanhThuTheoKhu + dTotalTheoKhu[iIndexTheoKhu];
                                dTotalTytrongtheokhu = dTotalTytrongtheokhu + dTytrongtheokhu;


                                if (iIndexTenKhu == 0)
                                {
                                    iArrViTriMergeCell[iIndexTheoKhu] = iRowIndex;
                                    iArrSodongMoiKhu[iIndexTheoKhu] = iSoDongMoiKhu - 1;
                                    ewres.Cells[iRowIndex, 6].Value = dTotalTheoKhu[iIndexTheoKhu];
                                    ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                                    ewres.Cells[iRowIndex, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                                }


                                ewres.Cells[iRowIndex, 7].Value = dTytrongtheokhu;
                                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#0.0%";
                                ewres.Cells[iRowIndex, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                                decimal dTytrongtheophamvi = 0;


                                if (dTotalDoanhThuTT > 0)
                                {
                                    dTytrongtheophamvi = dTotalTheoPhamVi[iIndexTheoPhamVi] / dTotalDoanhThuTT;
                                    dTotalTytrongtheophamvi = dTotalTytrongtheophamvi + dTytrongtheophamvi;
                                }

                                if (bMergedCellPhamVi == false)
                                {
                                    iArrViTriMergeCellPHamvi[iDemPhamVi] = iRowIndex;
                                    iArrSodongTheoPhamvi[iDemPhamVi] = iSodongMoiPHamVi - 1;

                                    ewres.Cells[iRowIndex, 8].Value = dTytrongtheophamvi;
                                    ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#0.0%";
                                    ewres.Cells[iRowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                                    bMergedCellPhamVi = true;
                                }



                                iLastRow = iLastRow + 1;
                                idem = idem + 1;
                                iRowIndex = iRowIndex + 1;
                                iIndexTenKhu = iIndexTenKhu + 1;
                            }


                            iIndexTheoKhu = iIndexTheoKhu + 1;

                        }

                    }//end khu                    

                    iIndexTheoPhamVi = iIndexTheoPhamVi + 1;
                    iDemPhamVi = iDemPhamVi + 1;
                }//end phamvi

                //MERGED CELL
                for (int i = 0; i < iArrViTriMergeCell.Length; i++)
                {
                    ewres.Cells[iArrViTriMergeCell[i], 6, iArrViTriMergeCell[i] + iArrSodongMoiKhu[i], 6].Merge = true;
                    ewres.Cells[iArrViTriMergeCell[i], 7, iArrViTriMergeCell[i] + iArrSodongMoiKhu[i], 7].Merge = true;
                }

                for (int i = 0; i < iArrViTriMergeCellPHamvi.Length; i++)
                {
                    ewres.Cells[iArrViTriMergeCellPHamvi[i], 8, iArrViTriMergeCellPHamvi[i] + iArrSodongTheoPhamvi[i], 8].Merge = true;
                }
                //END MERGED CELL

                //them dong tong
                ewres.Cells[iRowIndex, 1].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 3].Value = dTotalSKTT;
                ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 4].Value = dTotalDoanhThuTT;
                ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 5].Value = dTotalTyTrongTheoTuyen;
                ewres.Cells[iRowIndex, 5].Style.Numberformat.Format = "#0.0%";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 6].Value = dTotalDoanhThuTT;
                ewres.Cells[iRowIndex, 6].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 7].Value = 1;
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#0%";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 8].Value = 1;
                ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#0%";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                //THEM DONG 3 tuyến có số khách đông nhất
                iRowIndex = iRowIndex + 2;

                ewres.Cells[iRowIndex, 2].Value = "3 tuyến có số khách đông nhất";
                ewres.Cells[iRowIndex, 2].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
                //DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 2, iRowIndex, iColReport].Merge = true;

                iRowIndex = iRowIndex + 1;
                foreach (DataRow row in dt1.Rows)
                {
                    ewres.Cells[iRowIndex, 2].Value = row["tuyentq"].ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 3].Value = row["sokhachtt"];
                    ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    iRowIndex = iRowIndex + 1;
                }

                //THEM DONG 3 tuyến có tỷ trọng Doanh số cao nhất 
                //tinh tong truoc
                iRowIndex = iRowIndex + 1;

                decimal dTotal4 = 0;
                foreach (DataRow row in dt2.Rows)
                {
                    dTotal4 = dTotal4 + Decimal.Parse(row["doanhthutt"].ToString() == "" ? "0" : @row["doanhthutt"].ToString());
                }

                ewres.Cells[iRowIndex, 2].Value = "3 tuyến có tỷ trọng Doanh số cao nhất";
                ewres.Cells[iRowIndex, 2].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
                //DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 2, iRowIndex, iColReport].Merge = true;

                iRowIndex = iRowIndex + 1;

                int iDem4 = 0;
                decimal dDoanhthutt4 = 0, dTytrong4 = 0;

                foreach (DataRow row in dt2.Rows)
                {
                    dDoanhthutt4 = Decimal.Parse(row["doanhthutt"].ToString() == "" ? "0" : @row["doanhthutt"].ToString());
                    dTytrong4 = dDoanhthutt4 / dTotal4;
                    if (iDem4 < 3)//chi lay 3 gia tri cao nhat
                    {
                        ewres.Cells[iRowIndex, 2].Value = row["tuyentq"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 3].Value = dDoanhthutt4;
                        ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 4].Value = dTytrong4;
                        ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#0.0%";
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    }
                    iRowIndex = iRowIndex + 1;
                    iDem4 = iDem4 + 1;
                }

                //THEM DONG 3 khu có số khách đông nhất
                //iRowIndex = iRowIndex + 2;

                ewres.Cells[iRowIndex, 2].Value = "3 khu có số khách đông nhất";
                ewres.Cells[iRowIndex, 2].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
                // DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 2, iRowIndex, iColReport].Merge = true;

                iRowIndex = iRowIndex + 1;
                foreach (DataRow row in dt3.Rows)
                {
                    ewres.Cells[iRowIndex, 2].Value = row["tenkhu"].ToString();
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    ewres.Cells[iRowIndex, 3].Value = row["sokhachtt"];
                    ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    iRowIndex = iRowIndex + 1;
                }

                //THEM DONG 3 khu vực có tỉ trọng Doanh số cao nhất
                //tinh tong truoc
                iRowIndex = iRowIndex + 1;
                decimal dTotal3 = 0;
                foreach (DataRow row in dt4.Rows)
                {
                    dTotal3 = dTotal3 + Decimal.Parse(row["doanhthutt"].ToString() == "" ? "0" : @row["doanhthutt"].ToString());
                }

                ewres.Cells[iRowIndex, 2].Value = "3 khu vực có tỉ trọng Doanh số cao nhất";
                ewres.Cells[iRowIndex, 2].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
                //DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 2, iRowIndex, iColReport].Merge = true;

                iRowIndex = iRowIndex + 1;

                int iDem3 = 0;
                decimal dDoanhthutt3 = 0, dTytrong3 = 0;

                foreach (DataRow row in dt4.Rows)
                {
                    dDoanhthutt3 = Decimal.Parse(row["doanhthutt"].ToString() == "" ? "0" : row["doanhthutt"].ToString());
                    dTytrong3 = dDoanhthutt3 / dTotal3;

                    if (iDem3 < 3)
                    {
                        ewres.Cells[iRowIndex, 2].Value = row["tenkhu"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 3].Value = dDoanhthutt3;
                        ewres.Cells[iRowIndex, 3].Style.Numberformat.Format = "#,##0";
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        ewres.Cells[iRowIndex, 4].Value = dTytrong3;
                        ewres.Cells[iRowIndex, 4].Style.Numberformat.Format = "#0.0%";
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    }
                    iDem3 = iDem3 + 1;
                    iRowIndex = iRowIndex + 1;
                }


            }

            ewres.Cells.AutoFitColumns();
            return ewres;
        }
        #endregion



        public ActionResult doanhthutheosales(string tungay, string denngay, string dlcn)
        {
            var dao = new baocaoDAO();
            //DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            //DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");
            string sMM = "", sYYYY = "0";
            sMM = DateTime.Now.ToString("MM");
            sYYYY = DateTime.Now.ToString("yyyy");
            DateTime d1 = DateTime.Parse(sYYYY + "-" + sMM + "-1");
            DateTime d2 = DateTime.Parse(sYYYY + "-" + sMM + "-" + DungChung.LaySoNgayTrongThang(sMM, int.Parse(sYYYY)));


            DataTable dt = null;

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
            //ViewBag.CongTyPre = sCongTyPre;//dung xuat excel
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
            string sHoTen =  Session["hoten"].ToString();

            dt = dao.BCDoanhThuTheoSales(d1, d2, Session["username"].ToString(), sRoles, dlcn, sCongTyPre, sHoTen).Tables[0];


            return View(dt);
        }

        public ActionResult ShowError()
        {
            return View();
        }

        #region "My Chart"

        [HttpGet]
        public ActionResult Chart(string tungay, string denngay, string schinhanh)
        {

            //tungay = "01-07-2017";
            //denngay = "01-07-2018";
            //CongTy = "";
            //CongTyPre = "STA,STB,STH,STO,STS,STT";
            string Filename = "ChartSales_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelChart(tungay, denngay, schinhanh);
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

        private Stream CreateExcelChart(string tungay, string denngay, string schinhanh, Stream stream = null)
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
                excelPackage.Workbook.Properties.Title = "BÁO CÁO DOANH SỐ THEO SALES";

                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel                
                excelPackage.Workbook.Worksheets.Add("chart");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác              
                var workSheetChart = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                //workSheet = FormatWorkSheet(list, workSheet, d1, d2);
                workSheetChart = FormatWorkSheetChart(list, workSheetChart, d1, d2);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        public ExcelWorksheet FormatWorkSheetChart(DataSet ds, ExcelWorksheet workSheetChart, DateTime d1, DateTime d2)
        {
            
            //END HEADER          
            DataTable dt1 = ds.Tables[1];

            #region "Chart"        

            int iTotalRow1 = dt1.Rows.Count;
            if (dt1.Rows.Count > 0)
            {
                workSheetChart.Cells[1, 1].LoadFromText("Sale");
                DungChung.TrSetCellBorder(workSheetChart, 1, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                workSheetChart.Cells[1, 2].LoadFromText("Doanh số");
                DungChung.TrSetCellBorder(workSheetChart, 1,2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                int iRowIndex1 = 2;
                foreach (DataRow item in dt1.Rows)
                {
                    //COT 5
                    workSheetChart.Cells[iRowIndex1, 1].Value = item["tentheocn"].ToString();
                    DungChung.TrSetCellBorder(workSheetChart, iRowIndex1, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    //COT 6
                    workSheetChart.Cells[iRowIndex1, 2].Value = Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : @item["doanhthutt"].ToString());
                    workSheetChart.Cells[iRowIndex1, 2].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(workSheetChart, iRowIndex1, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                    iRowIndex1 = iRowIndex1 + 1;
                }
            }

            var lineChart = workSheetChart.Drawings.AddChart("lineChart", eChartType.ColumnClustered);
            //workSheetChart.Cells["A1"].LoadFromDataTable(dt1, false);
            //set the title
            lineChart.Title.Font.LatinFont = "Times New Roman";
            lineChart.Title.Font.Size = 16;
            lineChart.Title.Font.Bold = true;
            lineChart.Title.Text = "Đoàn đi tour từ ngày " + d1.ToString("dd/MM/yyyy") + " đến ngày " + d2.ToString("dd/MM/yyyy");
            //create the ranges for the chart
            iTotalRow1 = iTotalRow1 + 1;//+1 do bat dau tu row a2,b2
            var rangeLabel = workSheetChart.Cells["A2:A" + iTotalRow1];
            var range1 = workSheetChart.Cells["B2:B" + iTotalRow1];
            //var range2 = workSheetChart.Cells["B3:K3"];
            //add the ranges to the chart
            var lineSerires = (ExcelBarChartSerie)lineChart.Series.Add(range1, rangeLabel);
            //lineChart.Series.Add(range2, rangeLabel);

            lineSerires.DataLabel.Font.LatinFont = "Times New Roman";
            lineSerires.DataLabel.Font.Size = 13;
            //set the names of the legend
            lineChart.Series[0].Header = "Doanh số";
            //lineChart.Series[1].Header = workSheetChart.Cells["A3"].Value.ToString();
            //position of the legend
            lineChart.Legend.Position = eLegendPosition.Right;

            //size of the chart
            if (iTotalRow1 < 10)
            {
                lineChart.SetSize(800, 600);
            }
            else if (iTotalRow1 >=10 && iTotalRow1<20)
            {
                lineChart.SetSize(1024, 786);
            }
            else
            {
                lineChart.SetSize(1920, 1080);
            }
            
            //add the chart at cell B6
            lineChart.SetPosition(1, 0, 4, 0);

            workSheetChart.Cells.AutoFitColumns();
            #endregion

            
            return workSheetChart;
        }

        #endregion

        [HttpGet]
        public ActionResult Excel(string tungay, string denngay, string schinhanh)
        {

            //tungay = "01-07-2017";
            //denngay = "01-07-2018";
            //CongTy = "";
            //CongTyPre = "STA,STB,STH,STO,STS,STT";
            string Filename = "BCDoanhThuTheoSales_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

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
            string sHoTen = Session["hoten"].ToString();
            DataSet ds = dao.BCDoanhThuTheoSales(d1, d2, Session["username"].ToString(), sRoles, schinhanh, sCongTyPre, sHoTen);

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
                excelPackage.Workbook.Properties.Title = "BÁO CÁO DOANH SỐ THEO SALES";

                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("bc");
                
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];                
                // Đổ data vào Excel file
                //workSheet = FormatWorkSheet(list, workSheet, d1, d2);
                workSheet = FormatWorkSheet(list, workSheet,  d1, d2);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }
   

        public ExcelWorksheet FormatWorkSheet(DataSet ds, ExcelWorksheet ew,  DateTime d1, DateTime d2)
        {
            //DU LIEU = item.ItemArray.Length = 17 =  mahh,tenhh,mapb,tenpb, 13 cot ( 201707-201807)

            int iColReport = 11;//so cot bao bieu, STT	Code đoàn	Khách hàng	Thời gian	Tour	Tuyến tham quan	SK dự kiến	DT dự kiến	SK thực tế	Doanh số thực tế	Sales

            ExcelWorksheet ewres = ew;
            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");
            Color colorTotalRow = System.Drawing.ColorTranslator.FromHtml("#66ccff");
            Color colorThanhLy = System.Drawing.ColorTranslator.FromHtml("#7FFF00");
            Color colorChuaThanhLy = System.Drawing.ColorTranslator.FromHtml("#FFDEAD");

            ewres.Cells[1, 1].Value = "CÔNG TY DVLH SAIGONTOURIST";
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ewres.Cells[1, 1, 1, iColReport].Merge = true;

            ewres.Cells[2, 1].Value = "BÁO CÁO DOANH SỐ THEO SALES";
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            ewres.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[2, 1, 2, iColReport].Merge = true;

            ewres.Cells[3, 1].Value = "Đoàn đi tour từ ngày " + d1.ToString("dd/MM/yyyy") + " đến ngày " + d2.ToString("dd/MM/yyyy");
            ewres.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
            ewres.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ewres.Cells[3, 1, 3, iColReport].Merge = true;

            ewres.Cells[4, 1].Value = "Thời gian xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            ewres.Cells[4, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Regular));
            ewres.Cells[4, 1, 4, iColReport].Merge = true;

            ewres.Cells[5, 1].LoadFromText("STT");
            ewres.Cells[5, 2].LoadFromText("Code đoàn");
            ewres.Cells[5, 3].LoadFromText("Tên công ty/Khách hàng");
            ewres.Cells[5, 4].LoadFromText("Thời gian");
            ewres.Cells[5, 5].LoadFromText("Chủ đề tour");
            ewres.Cells[5, 6].LoadFromText("Tuyến tham quan");
            ewres.Cells[5, 7].LoadFromText("SK dự kiến");
            ewres.Cells[5, 8].LoadFromText("Doanh số dự kiến");
            ewres.Cells[5, 9].LoadFromText("SK thực tế");
            ewres.Cells[5, 10].LoadFromText("Doanh số thực tế");
            ewres.Cells[5, 11].LoadFromText("Sales");

            //create header
            // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            using (var range = ewres.Cells[5, 1, 5, iColReport])
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
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];    

            if (dt.Rows.Count > 0)
            {

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "username");//lay distinct ten cac sales

                int iRowIndex = 6;
                int idem = 1;
                //int iRowIndex = 0;
                decimal dFinalTotal = 0, dFinalTotalSK = 0;

                foreach (DataRow rr in distinctValues.Rows)
                {
                    DataRow[] rows = dt.Select("username='" + rr["username"].ToString() + "'");

                    decimal totalSKDK = 0, totalDoanhThuDK = 0;//chua thanh ly
                    decimal totalSKTT1 = 0, totalDoanhthuTT1 = 0;//da thanh ly
                    decimal dTotalSK = 0, dTotalDT = 0;
                    foreach (DataRow item in rows)
                    {
                        string sbatdau = item["batdau"].ToString() == "" ? "" : DateTime.Parse(item["batdau"].ToString()).ToString("dd/MM");
                        string sketthuc = item["ketthuc"].ToString() == "" ? "" : DateTime.Parse(item["ketthuc"].ToString()).ToString("dd/MM/yyyy");
                        string sngay = sbatdau + "~" + sketthuc;
                        string sTrangThai = item["trangthai"].ToString();
                        //COT 1
                        ewres.Cells[iRowIndex, 1].Value = idem;
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                        //ewres.Cells[iRowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        
                        //COT 2
                        ewres.Cells[iRowIndex, 2].Value = item["sgtcode"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);
                        ewres.Cells[iRowIndex, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        if (sTrangThai == "3")
                        {
                            ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(colorThanhLy);
                        }
                        else if (sTrangThai == "2")
                        {
                            ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        }
                        else if (sTrangThai == "4")
                        {
                            ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        }
                        else
                        {
                            ewres.Cells[iRowIndex, 2].Style.Fill.BackgroundColor.SetColor(Color.White);
                        }

                        //COT 3
                        ewres.Cells[iRowIndex, 3].Value = item["tenkh"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);                       

                        //COT 4
                        ewres.Cells[iRowIndex, 4].Value = sngay;
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        //COT 5
                        ewres.Cells[iRowIndex, 5].Value = item["chudetour"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        //COT 6
                        ewres.Cells[iRowIndex, 6].Value = item["diemtq"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);


                        if (sTrangThai != "3")
                        {//chua thanh ly
                            //COT 7                        
                            ewres.Cells[iRowIndex, 7].Value = Decimal.Parse(item["sokhachdk"].ToString() == "" ? "0" : @item["sokhachdk"].ToString());
                            ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                            //COT 8                       
                            ewres.Cells[iRowIndex, 8].Value = Decimal.Parse(item["doanhthudk"].ToString() == "" ? "0" : @item["doanhthudk"].ToString());
                            ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        }
                        else
                        {
                            //COT 7                        
                            ewres.Cells[iRowIndex, 7].Value = null;
                            ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                            //COT 8                       
                            ewres.Cells[iRowIndex, 8].Value = null;
                            ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        }

                        if (sTrangThai != "3")
                        {
                            //COT 9                       
                            ewres.Cells[iRowIndex, 9].Value = null;
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                            //COT 10                    
                            ewres.Cells[iRowIndex, 10].Value = null;
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        }
                        else
                        {
                            //COT 9                       
                            ewres.Cells[iRowIndex, 9].Value = Decimal.Parse(item["sokhachtt"].ToString() == "" ? "0" : @item["sokhachtt"].ToString());
                            ewres.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                            //COT 10                    
                            ewres.Cells[iRowIndex, 10].Value = Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : @item["doanhthutt"].ToString());
                            ewres.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0";
                            DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        }

                        //COT 9                       
                        //ewres.Cells[iRowIndex, 9].Value = dSKTT;
                        // DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        //COT 10                    
                        //ewres.Cells[iRowIndex, 10].Value = Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : @item["doanhthutt"].ToString()).ToString("#,#");
                        //DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        //COT 11
                        ewres.Cells[iRowIndex, 11].Value = item["fullName"].ToString();
                        DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Regular);

                        if (item["trangthai"].ToString() != "3")
                        {//chua thanh ly

                            totalSKDK += @Decimal.Parse(item["sokhachdk"].ToString() == "" ? "0" : @item["sokhachdk"].ToString());
                            totalDoanhThuDK += @Decimal.Parse(item["doanhthudk"].ToString() == "" ? "0" : @item["doanhthudk"].ToString());
                            //totalSKTT += @Decimal.Parse(item["sokhachtt"].ToString() == "" ? "0" : @item["sokhachtt"].ToString());
                            //totalDoanhthuTT += @Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : @item["doanhthutt"].ToString());                        
                        }
                        else  //da thanh ly
                        {
                            // totalSKDK1 += @Decimal.Parse(item["sokhachdk"].ToString() == "" ? "0" : @item["sokhachdk"].ToString());
                            //totalDoanhThuDK1 += @Decimal.Parse(item["doanhthudk"].ToString() == "" ? "0" : @item["doanhthudk"].ToString());
                            totalSKTT1 += @Decimal.Parse(item["sokhachtt"].ToString() == "" ? "0" : @item["sokhachtt"].ToString());
                            totalDoanhthuTT1 += @Decimal.Parse(item["doanhthutt"].ToString() == "" ? "0" : @item["doanhthutt"].ToString());
                        }

                        iRowIndex = iRowIndex + 1;
                        idem = idem + 1;
                    }//end rows

                    //tong so khach du tinh = tong so khach du tinh - tong so khach thuc te hay so khach cua tour da thanh ly
                    //doanh so du tinh= Doanh số du tinh - doanh so thuc te
                    decimal dTotalSKDT = 0, dTotalDTDT = 0;
                    //dTotalSKDT = totalSKDK - totalSKTT1;
                    //dTotalDTDT = totalDoanhThuDK - totalDoanhthuTT1;
                    dTotalSKDT = totalSKDK;
                    dTotalDTDT = totalDoanhThuDK;
                    dTotalSK = dTotalSKDT + totalSKTT1;
                    dTotalDT = dTotalDTDT + totalDoanhthuTT1;
                    //them dong tong chua thanh ly
                    //iRowIndex = iRowIndex+1;

                    //COT 1
                    ewres.Cells[iRowIndex, 1].Value = null;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);


                    //COT 2
                    ewres.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
                    //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 3
                    ewres.Cells[iRowIndex, 3].Value = "CHƯA THANH LÝ HỢP ĐỒNG";
                    //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 4].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                    ewres.Cells[iRowIndex, 5].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                    ewres.Cells[iRowIndex, 6].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                    //COT 7
                    ewres.Cells[iRowIndex, 7].Value = dTotalSKDT;
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 8
                    ewres.Cells[iRowIndex, 8].Value = dTotalDTDT;
                    ewres.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 9
                    ewres.Cells[iRowIndex, 9].Value = null;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 10
                    ewres.Cells[iRowIndex, 10].Value = null;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 11
                    ewres.Cells[iRowIndex, 11].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                    //them dong tong da thanh ly
                    iRowIndex = iRowIndex + 1;

                    //COT 1
                    ewres.Cells[iRowIndex, 1].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                    //COT 2
                    ewres.Cells[iRowIndex, 2].Value = "";
                    //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 3
                    ewres.Cells[iRowIndex, 3].Value = "ĐÃ THANH LÝ HỢP ĐỒNG";
                    //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 4].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                    ewres.Cells[iRowIndex, 5].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                    ewres.Cells[iRowIndex, 6].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                    //COT 7
                    ewres.Cells[iRowIndex, 7].Value = null;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 8
                    ewres.Cells[iRowIndex, 8].Value = null;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 9
                    ewres.Cells[iRowIndex, 9].Value = totalSKTT1;
                    ewres.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 10
                    ewres.Cells[iRowIndex, 10].Value = totalDoanhthuTT1;
                    ewres.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 11
                    ewres.Cells[iRowIndex, 11].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);

                    //THEM DONG TONG CONG
                    iRowIndex = iRowIndex + 1;

                    //COT 1
                    ewres.Cells[iRowIndex, 1].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                    //COT 2
                    ewres.Cells[iRowIndex, 2].Value = "";
                    //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    //COT 3
                    ewres.Cells[iRowIndex, 3].Value = "TỔNG CỘNG";
                    //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                    ewres.Cells[iRowIndex, 4].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                    ewres.Cells[iRowIndex, 5].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                    ewres.Cells[iRowIndex, 6].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                    //COT 7
                    ewres.Cells[iRowIndex, 7].Value = dTotalSK;
                    ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                    ewres.Cells[iRowIndex, 7, iRowIndex, 8].Merge = true;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                    ewres.Cells[iRowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //COT 9
                    ewres.Cells[iRowIndex, 9].Value = dTotalDT;
                    ewres.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                    ewres.Cells[iRowIndex, 9, iRowIndex, 10].Merge = true;
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                    ewres.Cells[iRowIndex, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //COT 11
                    ewres.Cells[iRowIndex, 11].Value = "";
                    DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);

                    //END THEM DONG TONG CONG
                    dFinalTotal = dFinalTotal + dTotalDT;
                    dFinalTotalSK = dFinalTotalSK + dTotalSK;

                    //tang index cho lan tiep theo
                    iRowIndex = iRowIndex + 1;
                }

                //THEM DONG FINAL TOTAL

                //COT 1
                ewres.Cells[iRowIndex, 1].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                //COT 2
                ewres.Cells[iRowIndex, 2].Value = "TỔNG CỘNG";
                //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                DungChung.TrSetCellBorder(ewres, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                //COT 3
                ewres.Cells[iRowIndex, 3].Value = "";
                //range.Style.Font.SetFromFont(new Font("Times New Roman", 13, FontStyle.Bold));
                DungChung.TrSetCellBorder(ewres, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 12, FontStyle.Bold);

                ewres.Cells[iRowIndex, 4].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                ewres.Cells[iRowIndex, 5].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                ewres.Cells[iRowIndex, 6].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);


                //COT 7
                ewres.Cells[iRowIndex, 7].Value = dFinalTotalSK;
                ewres.Cells[iRowIndex, 7].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 7, iRowIndex, 8].Merge = true;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //COT 9
                ewres.Cells[iRowIndex, 9].Value = dFinalTotal;
                ewres.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 9, iRowIndex, 10].Merge = true;
                DungChung.TrSetCellBorder(ewres, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 12, FontStyle.Bold);
                ewres.Cells[iRowIndex, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //COT 11
                ewres.Cells[iRowIndex, 11].Value = "";
                DungChung.TrSetCellBorder(ewres, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black);
                //set mau nen
                using (var range = ewres.Cells[iRowIndex, 1, iRowIndex, iColReport])
                {
                    // Canh giữa cho các text                   
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Set Font cho text  trong Range hiện tại                    
                    range.Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(colorTotalRow);
                }

                //End dong Final total
            }
            double column3Width = 50;
            ewres.Column(2).Width = 20;
            ewres.Column(3).Width = column3Width;
            ewres.Column(4).Width = 20;
            ewres.Column(5).Width = 50;
            ewres.Column(6).Width = 20;
            ewres.Column(8).Width = 15;
            ewres.Column(10).Width = 15;
            ewres.View.FreezePanes(1, 4);
            // ewres.Cells.AutoFitColumns();
            return ewres;
        }
    }
}