using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using qlkdst.Models;
using System.IO;
using System.Data;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Web;

namespace qlkdst.Controllers
{
    public class RoomController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

       

        /// <summary>
        /// lay roominglist
        /// </summary>
        /// <param name="id">id tour</param>
        /// <returns></returns>
        public ActionResult Index(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            var dao = new roominglistDAO();
            //List<vie_roomtour> model = dao.LayDSKS(id);
            List<roominglist> model = dao.LayRoomingList(id);
            ViewBag.idtour = id;// de truyen cho chuc nang insert , edit  
            List<vie_khachNotInRoominglist> lstkhach = dao.ListKhachChuaDuocChon(id);
            ViewBag.id_dsk = new SelectList(lstkhach, "id_dsk", "tenkhach");
            ViewBag.loaiphong = new SelectList(ListLoaiPhong(), "Value", "Text");
           
            return View(model);
        }

        public static string GetTenKhach(decimal? id)
        {
            var dao = new roominglistDAO();
            dmkhachtour model = dao.GetKhachTourByid(id);

            if (model != null)
                return model.tenkhach;
            else return "";
        }

        //public ActionResult GetDataRoomingForEdit(decimal id)
        //{
        //    roominglistDAO dao = new roominglistDAO();
        //    //string result = "";
        //    roominglist model = dao.Details(id);

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult Indexs(FrmPhieuXKDT input)
        //{
        //    var nonDeletedPhieuDT = input.vie_phieunxkhods.Where(c => !c.IsDeleted);
        //    return View(nonDeletedPhieuDT);
        //}

        public ActionResult GetDataRoomingForEdit(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            roominglistDAO dao = new roominglistDAO();
            roominglist model = dao.Details(id);
            //ViewBag.id_dsk = new SelectList(db.dmkhachtour.Where(x => x.idtour == model.idtour).ToList(), "id_dsk", "tenkhach");
            List<vie_khachNotInRoominglist> lstkhach = dao.ListKhachChuaDuocChon(model.idtour);
            ViewBag.id_dsk = new SelectList(lstkhach, "id_dsk", "tenkhach");
            ViewBag.loaiphong = new SelectList(ListLoaiPhong(), "Value", "Text");
            ViewBag.id_roominglist = id;
            return View(model);
        }

        //[HttpPost]
        //public ActionResult GetDataRoomingForEdit(roominglist model)
        //{
        //    roominglistDAO dao = new roominglistDAO();
        //    //string result = "";
        //    ViewBag.id_dsk = new SelectList(db.dmkhachtour.Where(x => x.idtour == model.idtour).ToList(), "id_dsk", "tenkhach");
        //    ViewBag.loaiphong = new SelectList(ListLoaiPhong(), "Value", "Text");
        //    return View(model);
        //}

        /// <summary>
        /// xoa chi tiet roominglistd
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult XoaKhach(decimal id)
        {
            string result = "Xóa thành công";
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            roominglistdDAO dao = new roominglistdDAO();

            try
            {
                string s = dao.Delete(id);                 
            }
            catch (Exception ex)
            {
                //SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
                result = "Xóa không thành công, lỗi: " + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddKhach(decimal id,roominglistd[] khachs)
        {
            string result = "RoomingList đã thêm thành công! ";          
            try
            {
                decimal did_roomlist = id;

                foreach (var item in khachs)
                {
                    roominglistd m = new roominglistd();
                    m.id_roomlist = did_roomlist;
                    m.sophong = item.sophong;
                    m.loaiphong = item.loaiphong;
                    m.id_dsk = item.id_dsk;
                    db.roominglistd.Add(m);
                }
                db.SaveChanges();
            }catch(Exception ex)
            {
                result = "Có lỗi! RoomingList chưa hoàn thành!" + ex.Message;
            } 
          
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRoomingList(string idtour,string tenks, string checkin, string checkout, roominglistd[] khachs)
        {
            string result = "Có lỗi! RoomingList chưa hoàn thành!";
            if (idtour!=null && tenks != null && checkin != null && khachs != null)
            {                
                roominglist model = new roominglist();                
                model.tenkhachsan = tenks;
                model.idtour = Decimal.Parse(idtour);
                model.ngaycheckin = DateTime.Parse(checkin);
                model.ngaycheckout = DateTime.Parse(checkout);
                db.roominglist.Add(model);
                db.SaveChanges();
                decimal did_roomlist = model.id_roomlist;

                foreach (var item in khachs)
                {                   
                    roominglistd m = new roominglistd();
                    m.id_roomlist = did_roomlist;
                    m.sophong = item.sophong;
                    m.loaiphong = item.loaiphong;
                    m.id_dsk = item.id_dsk;
                    db.roominglistd.Add(m);
                }
                db.SaveChanges();
                result = "RoomingList đã thêm thành công!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// thêm khách sạn rooming list; id: idtour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(decimal id)
        {
            roominglist model = new roominglist();
            model.idtour = id;           
         
            return View(model);
        }

        public ActionResult Cancel(decimal id)
        {
            return RedirectToAction("Index","QLKhachDoan");
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(roominglist model)
        {
            var dao = new roominglistDAO();

            try
            {
                string sUsrId = Session["userId"].ToString();   

                string id = dao.Insert(model);

                if (id != null)
                {
                    SetAlert("Thêm Thành Công", "success");
                    return RedirectToAction("Index", "QLKhachDoan");
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi:" + ex.Message, "warning");
            }

          
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var dao = new roominglistDAO();
            roominglist model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            //users usr = db.users.Where(x => x.userId == model.nguoitao).FirstOrDefault();

            //Session["nguoitao"] = model.nguoitao;
            //Session["ngaytao"] = model.ngaytao;
            //model.ngaysua = DateTime.Now;
            //model.nguoisua = Session["username"].ToString();
            //model.nguoitao = usr.fullName;
                    
            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(roominglist model)
        {
            var session = Session["username"];
         
            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new roominglistDAO();

                    //string sNguoiTao = "";
                    //if (Session["nguoitao"] != null)
                    //{
                    //    sNguoiTao = Session["nguoitao"].ToString();
                    //    var usr = db.users.Where(x => x.userId == sNguoiTao).FirstOrDefault();
                    //    model.nguoitao = usr.userId;
                    //    model.ngaytao = usr.ngaytao;

                    //    //lay chinhanh                       
                    //    model.daily = usr.chinhanh;
                    //}

                    string id = dao.Update(model);

                    SetAlert("Sửa  Thành Công", "success");
                }
                catch (Exception ex)
                {
                    SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
                }

            }
            else
            {

                return View(model);
            }

            return RedirectToAction("Index","QLKhachDoan");
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new roominglistDAO();
            roominglist model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        // GET: quanly/dmxes/Delete/5
        public ActionResult Delete(decimal id)
        {
            string res = "";
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new roominglistDAO();
            var daoD = new roominglistdDAO();
            try
            {
                //xoa danh sach trong roominglist truoc
                List<roominglistd> lst = daoD.GetLstD(id);
                bool bDOK = true;//xoa chi tiet ok? true la xoa thanh cong
                foreach(roominglistd l in lst)
                {
                    try
                    {
                        daoD.Delete(l.id_roomlistd);
                    }catch(Exception ex)
                    {
                        res = ex.Message;
                        bDOK = false;
                    }                   
                }
                
                if (bDOK)
                {
                    res=dao.Delete(id);
                }
                
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index","QLKhachDoan");
        }

        public ActionResult RoomingListWithD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RoomingListWithD(roominglistMasterDetails m,List<roominglistd> details)
        {

            roominglist model = new roominglist();
            model.tenkhachsan = m.tenkhachsan;
            model.ngaycheckin = m.ngaycheckin;
            model.ngaycheckout = m.ngaycheckout;

            db.roominglist.Add(model);
            db.SaveChanges();       

            decimal id = db.roominglist.Where(x => x.id_roomlist == model.id_roomlist).FirstOrDefault().id_roomlist;

            foreach(var detail in details)
            {
                detail.id_roomlist = id;
            }
            db.roominglistd.AddRange(details);
            db.SaveChanges();
            return View();
        }

        public List<SelectListItem> ListLoaiPhong()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "Single room";
            item.Value = "Single room";
            lst.Add(item);

            SelectListItem item1 = new SelectListItem();
            item1.Text = "Twin room";
            item1.Value = "Twin room";

            lst.Add(item1);

            SelectListItem item2 = new SelectListItem();
            item2.Text = "Triple room";
            item2.Value = "Triple room";

            lst.Add(item2);

            SelectListItem item3 = new SelectListItem();
            item3.Text = "Double room";
            item3.Value = "Double room";

            lst.Add(item3);
            return lst;
        }
                
        public ActionResult RoomingListDetails(decimal? i)
        {
            ViewBag.i = i;
            ViewBag.id_dsk = new SelectList(db.dmkhachtour, "id_dsk", "tenkhach");
            ViewBag.loaiphong = new SelectList(ListLoaiPhong(), "Value", "Text");
         
            return PartialView();
        }

        #region "Excel"
    

        public ActionResult ImportDmKhach(decimal id,HttpPostedFileBase uploadExcel)
        {
            if (Request.Files.Count > 0)
            {

                //var file = Request.Files[0];

                if (uploadExcel != null && uploadExcel.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(uploadExcel.FileName);
                    var path = Path.Combine(Server.MapPath("~/ExcelFiles/DmKhach/"), fileName);

                    if (Path.GetExtension(fileName) == ".xlsx" || Path.GetExtension(fileName) == ".xls")
                    {

                        ExcelPackage package = new ExcelPackage(uploadExcel.InputStream);
                        DataTable dt = EPPLusExtensions.ExcelToDataTable(package);
                        UploadMessage message = new UploadMessage();
                        try
                        {                            
                            message = ThemKhach(id, dt);

                            if (message.count > 0)
                            {
                                SetAlert("Nhập dữ liệu thành công : " + message.message, "success");
                            }
                            else if (message.errorCount > 0)
                            {
                                SetAlert("Nhập dữ liệu không được!" + message.message, "error");
                            }

                        }
                        catch (Exception ex)
                        {
                            SetAlert("Nhập dữ liệu không được: " + ex.Message, "error");
                        }


                    }
                    else
                    {
                        SetAlert("Loại file không đúng!", "error");
                    }

                }

                return RedirectToAction("Index", "QLKhachDoan");
            }
            else
            {
                return View();
            }

            

        }

        private string ConvertPhai(string phai)
        {
            switch (phai.ToLower())
            {
                case "nam":
                    return "1";
                case "nữ":
                    return "2";
                case "":
                    return "3";
                default:
                    return "1";
            }
        }

        public UploadMessage ThemKhach(decimal idtour, DataTable dt)
        {

            UploadMessage msg = new UploadMessage();
            if (dt.Rows.Count > 0)
            {
                //du lieu file Excel có các cột sau
                //Họ tên	Hộ chiếu	Ngày hết hạn HC	ngày sinh	CMND	Ngày làm CMND	Nơi cấp CMND	Phái	Điện thoại 	Quốc tịch


                var dao = new dmkhachtourDAO();

                foreach (DataRow row in dt.Rows)
                {
                    dmkhachtour model = new dmkhachtour();

                    if (row[0].ToString() != "") //co du lieu moi xu ly
                    {
                        string sHoTen = row[0].ToString();
                        string sHoChieu = row[1].ToString();
                        string sNgayHetHan = row[2].ToString();
                        string sNgaySinh = row[3].ToString();
                        string sCMND = row[4].ToString();
                        string sNgayLam = row[5].ToString();
                        string sNoiCap = row[6].ToString();
                        string sPhai = ConvertPhai(row[7].ToString());
                        string sDienThoai = row[8].ToString();
                        string sQuocTich = row[9].ToString();

                        IQueryable<dmkhachtour> oj = db.dmkhachtour.Where(x => x.idtour == idtour && x.tenkhach == sHoTen);

                        if (oj.FirstOrDefault() != null)  //da nhap  roi , bao loi
                        {
                            msg.message = "Dữ liệu đã có rồi";
                            msg.errorCount += 1;
                        }
                        else
                        {
                            model.tenkhach = sHoTen;
                            model.hochieu = sHoChieu;

                            if (sNgayHetHan != null)
                            {
                                try
                                {
                                    model.hieuluchochieu = DateTime.Parse(sNgayHetHan);
                                }
                                catch (Exception ex)
                                {
                                    msg.errorCount += 1;
                                    msg.message += ex.Message;
                                }

                            }

                            if (sNgaySinh != null)
                            {
                                try
                                {
                                    model.ngaysinh = DateTime.Parse(sNgaySinh);
                                }
                                catch (Exception ex)
                                {
                                    msg.errorCount += 1;
                                    msg.message += ex.Message;
                                }

                            }
                            model.socmnd = sCMND;
                            model.noicapcmnd = sNoiCap;


                            if (sNgayLam != null)
                            {
                                try
                                {
                                    model.ngaycmnd = DateTime.Parse(sNgayLam);
                                }
                                catch (Exception ex)
                                {
                                    msg.errorCount += 1;
                                    msg.message += ex.Message;
                                }

                            }

                            model.phai = sPhai;
                            model.dienthoai = sDienThoai;
                            model.quoctich = sQuocTich;

                            //model.macn = row[3] == null ? "" : row[3].ToString();                            
                            //model.sotien = row[5] == null ? 0 : decimal.Parse(row[5].ToString());


                            //model.machitietphi = TaoMaCP(); tu dong tang
                            model.idtour = idtour;

                            string id = dao.Insert(model);

                            if (id != null)
                            {
                                msg.count += 1;
                            }
                            else
                            {
                                msg.message += "<br/>";
                                msg.errorCount += 1;
                            }
                        }
                    }//end co du lieu moi xu ly

                }
            }

            return msg;
        }

        [HttpGet]
        public ActionResult Export(decimal id)
        {            
            string Filename = "RoomingList_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcel(id);
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
            return RedirectToAction("~/Home/Index");
        }

        private List<vie_roominglist> GetRoomingListDetails(decimal id)
        {
            var dao = new roominglistDAO();
            List<vie_roominglist> lst = dao.Getvie_roominglist(id);
            return lst;
        }

        private Stream CreateExcel(decimal id, Stream stream = null)
        {
            List<vie_roominglist> list = GetRoomingListDetails(id);
            //list = FormatBCTheoDB(list);
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Rooming List";
                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("RoomingList");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet = FormatWorkSheet(list, workSheet);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }
        public ExcelWorksheet FormatWorkSheet(List<vie_roominglist> dt, ExcelWorksheet ew)
        {
            //SO PHONG - TEN KHACH-NGAY SINH - LOAI PHONG - SĐT
            int iColReport = 5;
            string sTenKS = "",sNgayCheckin="",sNgayCheckout="";
            if (dt.Count > 0)
            {
                sTenKS = dt[0].tenkhachsan;
                sNgayCheckin = DateTime.Parse(dt[0].ngaycheckin.ToString()).ToString("dd/MM/yyyy");
                sNgayCheckout = DateTime.Parse(dt[0].ngaycheckout.ToString()).ToString("dd/MM/yyyy");
            }

            ExcelWorksheet ewres = ew;
            //Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");

            ewres.Cells[1, 1].Value = "ROOMING LIST CHO KHÁCH SẠN "+ sTenKS;
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            ewres.Cells[1, 5].Merge = true;
            ewres.Cells[2, 1].Value = "NGÀY CHECK IN:  " + sNgayCheckin + " NGÀY CHECK OUT: "+ sNgayCheckout;
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            ewres.Cells[2, 5].Merge = true;
        

            ewres.Cells[3, 1].LoadFromText("Số phòng");
            ewres.Cells[3, 2].LoadFromText("Loại phòng");          
            ewres.Cells[3, 3].LoadFromText("Khách");
            ewres.Cells[3, 4].LoadFromText("Ngày sinh");
            ewres.Cells[3, 5].LoadFromText("SĐT");

            //create header
            // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            using (var range = ewres.Cells[3, 1, 3, iColReport])
            {
                range.Style.WrapText = true;
                // Canh giữa cho các text
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại
                range.Style.Font.SetFromFont(new Font("Arial", 10));
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

            //noi dung

            //SO PHONG - TEN KHACH- LOAI PHONG - SĐT
            int iRowIndex = 4;
            foreach (vie_roominglist v in dt)
            {
                ewres.Cells[iRowIndex, 1].Value = v.sophong;
                ewres.Cells[iRowIndex, 2].Value = v.loaiphong;
                ewres.Cells[iRowIndex, 3].Value = v.tenkhach;

                if (v.ngaysinh != null)
                {
                    ewres.Cells[iRowIndex, 4].Value = DateTime.Parse(v.ngaysinh.ToString()).ToString("dd/MM/yyyy");
                }
                else
                {
                    ewres.Cells[iRowIndex, 4].Value = "";
                }
               
                ewres.Cells[iRowIndex, 5].Value = v.dienthoai;

                using (var range = ewres.Cells[iRowIndex, 1, iRowIndex, iColReport])
                {
                    range.Style.WrapText = true;
                    // Set Font cho text  trong Range hiện tại
                    range.Style.Font.SetFromFont(new Font("Arial", 10));
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

                iRowIndex = iRowIndex + 1;
            }

         

            //end noi dung

            return ewres;
        }


        #endregion

    }
}