using OfficeOpenXml;
using OfficeOpenXml.Style;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qlkdst.Common
{

    public class DungChung
    {

        public static string FTP_PATH = ConfigurationManager.AppSettings["FTP_PATH"].ToString();
        public static string USER = ConfigurationManager.AppSettings["FTP_USR"].ToString();
        public static string PASS = ConfigurationManager.AppSettings["FTP_PASS"].ToString();
        public static string DV_FTP_PATH = ConfigurationManager.AppSettings["DICHVU_FTP_PATH"].ToString();
        public static string TL_FTP_PATH = ConfigurationManager.AppSettings["THANHLY_FTP_PATH"].ToString();
        public static string CTTOUR_FTP_PATH = ConfigurationManager.AppSettings["CTTOUR_FTP_PATH"].ToString();

        public static string DSKHACH_FTP_PATH = ConfigurationManager.AppSettings["DSKHACH_FTP_PATH"].ToString();
        public static string ROOMINGLIST_FTP_PATH = ConfigurationManager.AppSettings["ROOMINGLIST_FTP_PATH"].ToString();
        public static string VEMAYBAY_FTP_PATH = ConfigurationManager.AppSettings["VEMAYBAY_FTP_PATH"].ToString();


        public static List<SelectListItem> LayDSLoaiTour()
        {
            qlkdtrEntities db = new qlkdtrEntities();
            List<loaitour> lstCn = db.loaitour.ToList();

            List<SelectListItem> lst = new List<SelectListItem>();

        
            SelectListItem item1 = new SelectListItem();

            item1.Text = "--Chọn loại tour--";
            item1.Value = "";
            lst.Add(item1);
           
            foreach (loaitour cn in lstCn)
            {
                SelectListItem item = new SelectListItem();


                item.Text = cn.tenloaitour;
                //item.Value = cn.loaitourid.ToString();
                item.Value = cn.tenloaitour;
                lst.Add(item);
            }

            return lst;
        }
        public static datcoclog SetBienNhanLogModel(datcoclog modellog, datcoc model, string username, string actionname)
        {           
            modellog.iddatcoc = model.iddatcoc;
            modellog.idtour = model.idtour;
            modellog.ngaydatcoc = model.ngaydatcoc;
            modellog.sobiennhan = model.sobiennhan;
            modellog.nguoilambn = model.nguoilambn;
            modellog.daily = model.daily;
            modellog.tenkhach = model.tenkhach;
            modellog.diachi = model.diachi;
            modellog.dienthoai = model.dienthoai;
            modellog.noidung = model.noidung;
            modellog.sotien = model.sotien;
            modellog.ngaytao = model.ngaytao;
            modellog.nguoitao = model.nguoitao;            
            modellog.ngaysua = model.ngaysua;
            modellog.nguoisua = model.nguoisua;
            modellog.hinhthucthanhtoan = model.hinhthucthanhtoan;
            modellog.chungtugoc = model.chungtugoc;
            modellog.tenmay = model.tenmay;
            modellog.loaitien = model.loaitien;
            modellog.tygia = model.tygia;

            modellog.nguoithaotac = username;
            modellog.hanhdong = actionname;
            modellog.thoigianthaotac = DateTime.Now;

            return modellog;
        }

        public static tourlog SetModelGhiLog(tourlog modellog, tour model, string username, string actionname)
        {
            modellog.idtour = model.idtour;
            modellog.sgtcode = model.sgtcode;
            modellog.chudetour = model.chudetour;
            modellog.ngaytao = model.ngaytao;
            modellog.nguoitao = model.nguoitao;
            modellog.batdau = model.batdau;
            modellog.ketthuc = model.ketthuc;
            modellog.tuyentq = model.tuyentq;
            modellog.diemtq = model.diemtq;
            modellog.sokhachdk = model.sokhachdk;
            modellog.doanhthudk = model.doanhthudk;
            modellog.makh = model.makh;
            modellog.tenkh = model.tenkh;
            modellog.diachi = model.diachi;
            modellog.dienthoai = model.dienthoai;
            modellog.fax = model.fax;
            modellog.email = model.email;
            modellog.ngaydamphan = model.ngaydamphan;
            modellog.hinhthucgiaodich = model.hinhthucgiaodich;
            modellog.ngaykyhopdong = model.ngaykyhopdong;
            modellog.nguoikyhopdong = model.nguoikyhopdong;
            modellog.hanxuatvmb = model.hanxuatvmb;
            modellog.ngaythanhlyhd = model.ngaythanhlyhd;
            modellog.sokhachtt = model.sokhachtt;
            modellog.doanhthutt = model.doanhthutt;
            modellog.chuongtrinhtour = model.chuongtrinhtour;
            modellog.noidungthanhlyhd = model.noidungthanhlyhd;
            modellog.dichvu = model.dichvu;
            modellog.daily = model.daily;
            modellog.loaitourid = model.loaitourid;
            modellog.trangthai = model.trangthai;
            modellog.ngaysua = model.ngaysua;
            modellog.nguoisua = model.nguoisua;
            modellog.chinhanh = model.chinhanh;
            modellog.ngaynhandutien = model.ngaynhandutien;
            modellog.lidonhandu = model.lidonhandu;
            modellog.sohopdong = model.sohopdong;
            modellog.laichuave = model.laichuave;
            modellog.laigomve = model.laigomve;
            modellog.laithuctegomve = model.laithuctegomve;
            modellog.nguyennhanhuythau = model.nguyennhanhuythau;
            modellog.nguontour = model.nguontour;
            modellog.filekhachditour = model.filekhachditour;
            modellog.filevemaybay = model.filevemaybay;
            modellog.filebiennhan = model.filebiennhan;
            modellog.nguoidaidien = model.nguoidaidien;
            modellog.doitacnuocngoai = model.doitacnuocngoai;
            modellog.ngayhuytour = model.ngayhuytour;


            modellog.nguoithaotac = username;
            modellog.hanhdong = actionname;
            modellog.thoigianthaotac = DateTime.Now;

            return modellog;
        }

        /// <summary>
        /// lay danh sach chi nhanh ma  user co quyen
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string StrQuyenUsr(string userId, qlkdtrEntities db)
        {
            string sCongTyPre = "";

            //qlkdtrEntities db = new qlkdtrEntities();
            List<vie_quyenusrtheocn> ds = db.vie_quyenusrtheocn.Where(x=>x.userId==userId).ToList();

            foreach (vie_quyenusrtheocn c in ds)
            {
                sCongTyPre = sCongTyPre + "," + String.Format("\"{0}\"",c.chinhanh);
            }

            //bo dau , dau tien
            if (sCongTyPre.Length > 0)
                sCongTyPre = sCongTyPre.Substring(1, sCongTyPre.Length - 1);


            return sCongTyPre;
        }

        public static string[] ArrStrQuyenUsr(string userId, qlkdtrEntities db)
        {
       

            //qlkdtrEntities db = new qlkdtrEntities();
            List<vie_quyenusrtheocn> ds = db.vie_quyenusrtheocn.Where(x => x.userId == userId).ToList();
            int i = 0;
            string[] sCongTyPre = new string[ds.Count];
            foreach (vie_quyenusrtheocn c in ds)
            {
                sCongTyPre[i] =String.Format("{0}", c.chinhanh);
                i++;
            }           

            return sCongTyPre;
        }

        public static List<vie_quyenusrtheocn> LstQuyenUsr(string userId, qlkdtrEntities db)
        {
            //qlkdtrEntities db = new qlkdtrEntities();
            List<vie_quyenusrtheocn> ds = db.vie_quyenusrtheocn.Where(x => x.userId == userId).ToList();           
            return ds;
        }


        /// <summary>
        /// lay danh sach user co quyen
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<chinhanh> LayChiNhanhTheoUser(string userId)
        {
            qlkdtrEntities db = new qlkdtrEntities();
            List<chinhanh> lstCn = db.chinhanh.ToList();//admin hay superadmin thay het chi nhanh
            string sRole = "", sCn = "";
            users usr = db.users.Where(x => x.userId == userId).FirstOrDefault();

            if (usr != null)
            {
                sRole = usr.role;
                sCn = usr.chinhanh;
            }

            var ds = db.vie_quyenusrtheocn.Where(x => x.userId == userId).Select(x=>x.chinhanh);

            if (ds !=null && sRole != "admin" && sRole != "superadmin") //user co quyen theo khu
            {
                lstCn = lstCn.Where(x => ds.Contains(x.chinhanh1) || x.chinhanh1==usr.chinhanh).ToList();

            }else{

                if (sRole != "admin" && sRole != "superadmin")
                {
                    lstCn = db.chinhanh.Where(x => x.chinhanh1 == sCn).ToList();
                }
            }

            
            return lstCn;
        }

        /// <summary>
        /// lay danh sach chi nhanh user co quyen, dung cho DropDownList
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<SelectListItem> LayDSChiNhanhTheoUser(string userId)
        {
            qlkdtrEntities db = new qlkdtrEntities();
            List<chinhanh> lstCn = db.chinhanh.ToList();
            string sRole = "", sCn = "";
            users usr = db.users.Where(x => x.userId == userId).FirstOrDefault();

            if (usr != null)
            {
                sRole = usr.role;
                sCn = usr.chinhanh;
            }

            //if (sRole != "admin" && sRole != "superadmin")
            //{
            //    lstCn = db.chinhanh.Where(x => x.chinhanh1 == sCn).ToList();
            //}

            //add 29072019
            var ds = db.vie_quyenusrtheocn.Where(x => x.userId == userId).Select(x => x.chinhanh);

            if (ds != null && sRole != "admin" && sRole != "superadmin") //user co quyen theo khu
            {
                lstCn = lstCn.Where(x => ds.Contains(x.chinhanh1) || x.chinhanh1 == usr.chinhanh).ToList();

            }
            else
            {

                if (sRole != "admin" && sRole != "superadmin")
                {
                    lstCn = db.chinhanh.Where(x => x.chinhanh1 == sCn).ToList();
                }
            }


            List<SelectListItem> lst = new List<SelectListItem>();

            if (sRole == "admin" || sRole == "superadmin" || lstCn.Count>1)
            {
                SelectListItem item = new SelectListItem();

                item.Text = "--Chọn chi nhánh--";
                item.Value = "";
                lst.Add(item);
            }

            foreach (chinhanh cn in lstCn)
            {
                SelectListItem item = new SelectListItem();


                item.Text = cn.tencn;
                item.Value = cn.chinhanh1;
                lst.Add(item);
            }

            return lst;
        }
        public static string GetTenTrangThaiTour(string trangthai)
        {
            string sTT = "";
            if (trangthai == "0")
            {
                sTT = "Mới tạo";
            }
            else if (trangthai == "1")
            {
                sTT = "Mới đàm phán";
            }
            else if (trangthai == "2")
            {
                sTT = "Đã ký hợp đồng";
            }
            else if (trangthai == "3")
            {
                sTT = "Đã thanh lý hợp đồng";
            }
            else if (trangthai == "4")
            {
                sTT = "Đã hủy tour";
            }
            else
            {
                sTT = "";
            }

            return sTT;
        }

        public static string GetTenPhamViTuyen(string phamvi)
        {
            string sPhamvi = "";

            if (phamvi == "1")
            {
                sPhamvi = "Tuyến gần";
            }
            else if (phamvi == "2")
            {
                sPhamvi = "Tuyến xa";
            }
            else if (phamvi == "3")
            {
                sPhamvi = "Tuyến đường bay";
            }
            else if (phamvi == "4")
            {
                sPhamvi = "Tuyến đường bộ";
            }

            return sPhamvi;
        }

        public static string LaySoNgayTrongThang(string thang, int nam)
        {
            string sRes = "";
            switch (thang)
            {
                case "01":
                    sRes = "31";
                    break;
                case "02":
                    bool isLeapYear = DateTime.IsLeapYear(nam);
                    if (isLeapYear)
                    {
                        sRes = "29";
                    }
                    else
                    {
                        sRes = "28";
                    }

                    break;
                case "03":
                    sRes = "31";
                    break;
                case "04":
                    sRes = "30";
                    break;
                case "05":
                    sRes = "31";
                    break;
                case "06":
                    sRes = "30";
                    break;
                case "07":
                    sRes = "31";
                    break;
                case "08":
                    sRes = "31";
                    break;
                case "09":
                    sRes = "30";
                    break;
                case "10":
                    sRes = "31";
                    break;
                case "11":
                    sRes = "30";
                    break;
                case "12":
                    sRes = "31";
                    break;
                default:
                    sRes = "0";
                    break;
            }

            return sRes;
        }

        public static List<SelectListItem> ListThang()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();

            item.Text = "1";
            item.Value = "01";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "2";
            item.Value = "02";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "3";
            item.Value = "03";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "4";
            item.Value = "04";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "5";
            item.Value = "05";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "6";
            item.Value = "06";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "7";
            item.Value = "07";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "8";
            item.Value = "08";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "9";
            item.Value = "09";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "10";
            item.Value = "10";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "11";
            item.Value = "11";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "12";
            item.Value = "12";
            lst.Add(item);

            return lst;

        }

        public static List<SelectListItem> ListPhamViTuyen()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();
            item.Text = "Tuyến gần";
            item.Value = "1";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Tuyến xa";
            item.Value = "2";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Tuyến đường bay";
            item.Value = "3";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Tuyến đường bộ";
            item.Value = "4";
            lst.Add(item);

            return lst;
        }


        public static List<SelectListItem> ListNguonTour()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();

            item.Text = "Từ nội bộ";
            item.Value = "Từ nội bộ";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Từ TMĐT";
            item.Value = "Từ TMĐT";
            lst.Add(item);         

            return lst;
        }

        public static List<SelectListItem> ListTrangThaiTour()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();

            item.Text = "";
            item.Value = "";//tat ca trang thai
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Mới tạo";
            item.Value = "0";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Mới đàm phán";
            item.Value = "1";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Đã ký hợp đồng";
            item.Value = "2";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Đã thanh lý hợp đồng";
            item.Value = "3";
            lst.Add(item);

            item = new SelectListItem();
            item.Text = "Giao dịch chưa thành công";
            item.Value = "4";
            lst.Add(item);

            return lst;
        }
        public static void TrSetCellBorder(ExcelWorksheet ewres, int iRowIndex, int colIndex, ExcelBorderStyle excelBorderStyle, ExcelHorizontalAlignment excelHorizontalAlignment, Color borderColor, string fontName, int fontSize, FontStyle fontStyle)
        {
            ewres.Cells[iRowIndex, colIndex].Style.HorizontalAlignment = excelHorizontalAlignment;
            // Set Border
            ewres.Cells[iRowIndex, colIndex].Style.Border.Left.Style = excelBorderStyle;
            ewres.Cells[iRowIndex, colIndex].Style.Border.Top.Style = excelBorderStyle;
            ewres.Cells[iRowIndex, colIndex].Style.Border.Right.Style = excelBorderStyle;
            ewres.Cells[iRowIndex, colIndex].Style.Border.Bottom.Style = excelBorderStyle;
            // Set màu ch Border
            ewres.Cells[iRowIndex, colIndex].Style.Border.Left.Color.SetColor(borderColor);
            ewres.Cells[iRowIndex, colIndex].Style.Border.Top.Color.SetColor(borderColor);
            ewres.Cells[iRowIndex, colIndex].Style.Border.Right.Color.SetColor(borderColor);
            ewres.Cells[iRowIndex, colIndex].Style.Border.Bottom.Color.SetColor(borderColor);

            // Set Font cho text  trong Range hiện tại                    
            ewres.Cells[iRowIndex, colIndex].Style.Font.SetFromFont(new Font(fontName, fontSize, fontStyle));
        }

        public static void TrSetCellBorder(ExcelWorksheet ewres, int iRowIndex, int colIndex, ExcelBorderStyle excelBorderStyle, ExcelHorizontalAlignment excelHorizontalAlignment, Color borderColor)
        {
            ewres.Cells[iRowIndex, colIndex].Style.HorizontalAlignment = excelHorizontalAlignment;
            // Set Border
            ewres.Cells[iRowIndex, colIndex].Style.Border.Left.Style = excelBorderStyle;
            ewres.Cells[iRowIndex, colIndex].Style.Border.Top.Style = excelBorderStyle;
            ewres.Cells[iRowIndex, colIndex].Style.Border.Right.Style = excelBorderStyle;
            ewres.Cells[iRowIndex, colIndex].Style.Border.Bottom.Style = excelBorderStyle;
            // Set màu ch Border
            ewres.Cells[iRowIndex, colIndex].Style.Border.Left.Color.SetColor(borderColor);
            ewres.Cells[iRowIndex, colIndex].Style.Border.Top.Color.SetColor(borderColor);
            ewres.Cells[iRowIndex, colIndex].Style.Border.Right.Color.SetColor(borderColor);
            ewres.Cells[iRowIndex, colIndex].Style.Border.Bottom.Color.SetColor(borderColor);
        }

        public static IQueryable<roles> GetLoaiNDTuUser(string userId)
        {
            roles nd = new roles();

            qlkdtrEntities db = new qlkdtrEntities();
            string sMaNguoiDung = "";

            sMaNguoiDung = db.users.Where(x => x.userId == userId).FirstOrDefault().role;
            return db.roles.Where(x => x.roleName == sMaNguoiDung);
        }

        public static List<roles> GetLstLoaiNDTuUser(string userId, string sRoleName)
        {
            qlkdtrEntities db = new qlkdtrEntities();
            List<roles> lstND = new List<roles>();

            if (sRoleName == "superadmin")
            {
                lstND = db.roles.ToList(); //loai user superadmin moi thay het cac role
            }
            else if (sRoleName == "admin") //thay tat ca role tru role superadmin
            {
                //user không phải role superadmin thì  không thấy được role superadmin
                lstND = db.roles.Where(x => x.roleName != "superadmin").ToList();
            }
            else if (sRoleName == "salemanager") //user khac thi khong duoc thay nhom superadmin admin
            {
                lstND = db.roles.Where(x => x.roleName != "superadmin" && x.roleName != "admin").ToList();
            }
            else
            {
                lstND = db.roles.Where(x => x.roleName != "superadmin" && x.roleName != "admin" && x.roleName != "salemanager").ToList();
            }

            return lstND;
        }

        public static List<roles> GetLstLoaiNDTuUser(string userId)
        {
            qlkdtrEntities db = new qlkdtrEntities();

            IQueryable<roles> io = DungChung.GetLoaiNDTuUser(userId);
            List<roles> lstND = new List<roles>();

            string sRoleName = "";
            sRoleName = io.FirstOrDefault().roleName.ToLower();

            if (sRoleName == "superadmin")
            {
                lstND = db.roles.ToList(); //loai user superadmin moi thay het cac role
            }
            else if (sRoleName == "admin") //thay tat ca role tru role superadmin
            {
                //user không phải role superadmin thì  không thấy được role superadmin
                lstND = db.roles.Where(x => x.roleName != "superadmin").ToList();
            }
            else
            {
                lstND = db.roles.Where(x => x.roleName != "superadmin" && x.roleName != "admin").ToList();
            }

            return lstND;
        }

        /// <summary>
        /// lay trang thai hien tai cua 1 tour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetTrangThaiTour(decimal id)
        {
            qlkdtrEntities db = new qlkdtrEntities();

            tour model = db.tour.Where(x => x.idtour == id).FirstOrDefault();

            if (model != null)
            {
                return model.trangthai;
            }
            else
            {
                return "";
            }
        }

        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }

        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }

        public static string So_chu(decimal gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            decimal Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }
    }
}