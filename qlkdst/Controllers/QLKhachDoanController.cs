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
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml;
using System.Data;
using System.Web;
using static qlkdstDB.DAO.dmkhachhangDAO;
using System.Configuration;
using OfficeOpenXml.Drawing;
using System.Web.Routing;

namespace qlkdst.Controllers
{
    public class QLKhachDoanController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, string ngayditourb, string ngayditoure, string tencongty, string sohopdong, string nguoitao, string tuyentq, int page = 1, int pagesize = 10)
        {
            var dao = new tourDAO();

            string sDaily = "", sChiNhanh = "";
            users usr = new users();
            if (Session["USER_SESSION"] != null)
            {
                usr = (users)Session["USER_SESSION"];
                sDaily = usr.daily;
                sChiNhanh = usr.chinhanh;
            }
            //DateTime d1 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-1-1");
            DateTime d1 = DateTime.Now;
            // DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");
            DateTime d2 = DateTime.Parse(DateTime.Now.ToString("yyyy") + "-12-31");
            double giohientai = DateTime.Now.Hour;
            d2 = d2.AddHours(giohientai + 1);

            if (Request["ngayditourb"] != null && Request["ngayditoure"] != null)
            {
                try
                {
                    d1 = DateTime.Parse(Request["ngayditourb"].ToString());
                    d2 = DateTime.Parse(Request["ngayditoure"].ToString());
                }
                catch
                {

                }                

            }
            else
            {
                if (ngayditourb != null && ngayditoure != null && !ngayditourb.Equals("") && !ngayditoure.Equals(""))
                {
                    d1 = DateTime.Parse(ngayditourb);
                    d2 = DateTime.Parse(ngayditoure);

                }
            }
         

            ViewBag.ngayditourb = d1.ToString("dd/MM/yyyy");
            ViewBag.ngayditoure = d2.ToString("dd/MM/yyyy");


            if (d2 < d1)
            {
                SetAlert("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!", "warning");
                ModelState.AddModelError("", "Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");
                return RedirectToAction("ShowError", "BaoCao");
            }


            // List<vie_quyenusrtheocn> lstquyen = DungChung.LstQuyenUsr(usr.userId,db);
            string[] sCongTyPre = DungChung.ArrStrQuyenUsr(usr.userId, db);

            //var model = dao.ListAllPageList(searchString, d1, d2, tencongty, sohopdong, sChiNhanh, sDaily, usr, page, pagesize);            
            var model = dao.ListAllPageList(searchString, d1, d2, tencongty, sohopdong, sChiNhanh, sDaily, usr, nguoitao, tuyentq, sCongTyPre, page, pagesize);

            ViewBag.searchString = searchString;
            ViewBag.tencongty = tencongty;
            ViewBag.sohopdong = sohopdong;
            ViewBag.nguoitao = nguoitao;
            ViewBag.tuyentq = tuyentq;
            return View(model);
        }

        /// <summary>
        /// update tour add 15082019
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public UploadMessage UpdateTourExcel(DataTable dt)
        {

            UploadMessage msg = new UploadMessage();
            if (dt.Rows.Count > 0)
            {

                var dao = new tourDAO();
                msg.message = "<sodong>" + dt.Rows.Count + "</sodong>";

                foreach (DataRow row in dt.Rows)
                {

                    bool bCoLoi = false;//mac dinh la khong co loi


                    if (row[0].ToString() != "") //co du lieu moi xu ly
                    {
                        string sSTT = row[0].ToString();//chua su dung
                        string sSgtCode = row[1].ToString();
                        if (sSgtCode != null) sSgtCode = sSgtCode.Trim();
                        string sSales = row[2].ToString();
                        string sNgayTao = row[3].ToString();
                        string sMakh = row[4].ToString();
                        string sTenKh = row[5].ToString();
                        string sDiaChi = row[6].ToString();
                        string sDienThoai = row[7].ToString();
                        string sFax = row[8].ToString();
                        string sEmail = row[9].ToString();
                        if (sEmail != null) sEmail = sEmail.Trim();
                        string sNgayDiTour = row[10].ToString();
                        string sKetThuc = row[11].ToString();
                        string sChudetour = row[12].ToString();
                        string sTuyenTQ = row[13].ToString();
                        string sDiemTQ = row[14].ToString();
                        string sNgayDamPhan = row[15].ToString();
                        string sDaiDienKH = row[16].ToString();
                        string sLoaiTour = row[17].ToString();
                        string sNgayKyHD = row[18].ToString();
                        string sNguoiKyHD = row[19].ToString();
                        string sSoHD = row[20].ToString();
                        string sSKDK = row[21].ToString();
                        string sDTDK = row[22].ToString();
                        string sNgayDCVeMB = row[23].ToString();
                        string sDoiTacNcNgoai = row[24].ToString();
                        string sNgayThanhLyHD = row[25].ToString();
                        string sSKTT = row[26].ToString();
                        string sDTTT = row[27].ToString();
                        string sNgayHuy = row[28].ToString();
                        string sLiDoHuy = row[29].ToString();
                        string sChinhanh = row[30].ToString();

                        msg.message += "<sgtcode>" + sSgtCode;

                        tour modelt = dao.DetailsByCode(sSgtCode);

                        if (modelt != null)
                        {

                            try
                            {
                                //modelt.sgtcode = sSgtCode;
                                modelt.nguoitao = sSales;
                                modelt.makh = sMakh;
                                modelt.tenkh = sTenKh;
                                modelt.diachi = sDiaChi;
                                modelt.dienthoai = sDienThoai;
                                modelt.fax = sFax;
                                modelt.email = sEmail;
                                modelt.chudetour = sChudetour;
                                modelt.tuyentq = sTuyenTQ;
                                modelt.diemtq = sDiemTQ;
                                modelt.nguoidaidien = sDaiDienKH;
                                modelt.loaitourid = sLoaiTour;
                                modelt.nguoikyhopdong = sNguoiKyHD;
                                modelt.sohopdong = sSoHD;

                                modelt.doitacnuocngoai = sDoiTacNcNgoai;
                                modelt.noidungthanhlyhd = sLiDoHuy;
                                modelt.chinhanh = sChinhanh;
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                bCoLoi = true;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                msg.message += "<caccotdau>" + ex.Message + "</caccotdau>";
                            }

                            try
                            {
                                modelt.sokhachdk = sSKDK == null ? 0 : int.Parse(sSKDK);

                            }
                            catch (Exception ex)
                            {
                                modelt.sokhachdk = 0;
                            }

                            try
                            {
                                modelt.doanhthudk = sDTDK == null ? 0 : decimal.Parse(sDTDK);

                            }
                            catch (Exception ex)
                            {
                                modelt.doanhthudk = 0;
                            }

                            try
                            {
                                modelt.sokhachtt = sSKTT == null ? 0 : int.Parse(sSKTT);

                            }
                            catch (Exception ex)
                            {
                                modelt.sokhachtt = 0;
                            }

                            try
                            {
                                modelt.doanhthutt = sDTTT == null ? 0 : decimal.Parse(sDTTT);
                            }
                            catch (Exception ex)
                            {
                                modelt.doanhthutt = 0;
                            }

                            if (sNgayTao != null && sNgayTao != "")
                            {
                                try
                                {
                                    modelt.ngaytao = DateTime.Parse(sNgayTao);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    // msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<ngaytao>" + sNgayTao + " - " + ex.Message + "</ngaytao>";
                                }

                            }

                            if (sNgayDiTour != null && sNgayDiTour != "")
                            {
                                try
                                {
                                    modelt.batdau = DateTime.Parse(sNgayDiTour);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<ngayditour>" + sNgayDiTour + " - " + ex.Message + "</ngayditour>";
                                }

                            }



                            if (sKetThuc != null && sKetThuc != "")
                            {
                                try
                                {
                                    modelt.ketthuc = DateTime.Parse(sKetThuc);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<ketthuc>" + sKetThuc + " - " + ex.Message + "</ketthuc>";
                                }

                            }

                            if (sNgayDamPhan != null && sNgayDamPhan != "")
                            {
                                try
                                {
                                    modelt.ngaydamphan = DateTime.Parse(sNgayDamPhan);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    msg.message += "<ngaydamphan>" + sNgayDamPhan + " - " + ex.Message + "</ngaydamphan>";
                                    bCoLoi = true;
                                }

                            }

                            if (sNgayKyHD != null && sNgayKyHD != "")
                            {
                                try
                                {
                                    modelt.ngaykyhopdong = DateTime.Parse(sNgayKyHD);
                                }
                                catch (Exception ex)
                                {
                                    // msg.errorCount += 1;
                                    // msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<ngaykyhopdong>" + sNgayKyHD + " - " + ex.Message + "</ngaykyhopdong>";
                                }

                            }

                            if (sNgayDCVeMB != null && sNgayDCVeMB != "")
                            {
                                try
                                {
                                    modelt.hanxuatvmb = DateTime.Parse(sNgayDCVeMB);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<vemb>" + sNgayDCVeMB + " - " + ex.Message + "</vemb>";
                                }

                            }

                            //sNgayThanhLyHD
                            if (sNgayThanhLyHD != null && sNgayThanhLyHD != "")
                            {
                                try
                                {
                                    modelt.ngaythanhlyhd = DateTime.Parse(sNgayThanhLyHD);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<ngaythanhlyhd>" + sNgayThanhLyHD + " - " + ex.Message + "</ngaythanhlyhd>";
                                }

                            }

                            if (sNgayHuy != null && sNgayHuy != "")
                            {
                                try
                                {
                                    modelt.ngayhuytour = DateTime.Parse(sNgayHuy);
                                }
                                catch (Exception ex)
                                {
                                    //msg.errorCount += 1;
                                    //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                    bCoLoi = true;
                                    msg.message += "<ngayhuy>" + sNgayHuy + " - " + ex.Message + "</ngayhuy>";
                                }

                            }


                            string id = null;

                            try
                            {
                                if (modelt.ngaydamphan != null)
                                {
                                    modelt.trangthai = "1";
                                }


                                if (modelt.ngaykyhopdong != null)
                                {
                                    modelt.trangthai = "2";
                                }

                                if (modelt.ngaythanhlyhd != null)
                                {
                                    modelt.trangthai = "3";
                                }

                                if (modelt != null)
                                {

                                    id = dao.Update(modelt);
                                }


                            }
                            catch (Exception ex)
                            {
                                // msg.message += model.sgtcode + " - " + ex.Message + "<br/>";
                                msg.message += "<loithemtour>" + ex.Message + "</loithemtour>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }


                            if (bCoLoi == true)
                            {
                                msg.errorCount += 1;
                            }
                            else
                            {
                                msg.count += 1;
                            }


                            msg.message += "</sgtcode>";

                        }//END modelt != null



                    }//end co du lieu moi xu ly

                }//end foreach
                 //  msg.message += "</Sheet>";
            }

            return msg;
        }

        public UploadMessage ThemTourExcelKhongKiemTraDK(DataTable dt)
        {

            UploadMessage msg = new UploadMessage();
            if (dt.Rows.Count > 0)
            {
                //du lieu file Excel có các cột sau
                //Họ tên	Hộ chiếu	Ngày hết hạn HC	ngày sinh	CMND	Ngày làm CMND	Nơi cấp CMND	Phái	Điện thoại 	Quốc tịch



                var dao = new tourDAO();
                msg.message = "<sodong>" + dt.Rows.Count + "</sodong>";

                foreach (DataRow row in dt.Rows)
                {
                    tour model = new tour();
                    bool bCoLoi = false;//mac dinh la khong co loi


                    if (row[0].ToString() != "") //co du lieu moi xu ly
                    {



                        string sSTT = row[0].ToString();//chua su dung
                        string sSgtCode = row[1].ToString();
                        if (sSgtCode != null) sSgtCode = sSgtCode.Trim();
                        string sSales = row[2].ToString();
                        string sNgayTao = row[3].ToString();
                        string sMakh = row[4].ToString();
                        string sTenKh = row[5].ToString();
                        string sDiaChi = row[6].ToString();
                        string sDienThoai = row[7].ToString();
                        string sFax = row[8].ToString();
                        string sEmail = row[9].ToString();
                        if (sEmail != null) sEmail = sEmail.Trim();
                        string sNgayDiTour = row[10].ToString();
                        string sKetThuc = row[11].ToString();
                        string sChudetour = row[12].ToString();
                        string sTuyenTQ = row[13].ToString();
                        string sDiemTQ = row[14].ToString();
                        string sNgayDamPhan = row[15].ToString();
                        string sDaiDienKH = row[16].ToString();
                        string sLoaiTour = row[17].ToString();
                        string sNgayKyHD = row[18].ToString();
                        string sNguoiKyHD = row[19].ToString();
                        string sSoHD = row[20].ToString();
                        string sSKDK = row[21].ToString();
                        string sDTDK = row[22].ToString();
                        string sNgayDCVeMB = row[23].ToString();
                        string sDoiTacNcNgoai = row[24].ToString();
                        string sNgayThanhLyHD = row[25].ToString();
                        string sSKTT = row[26].ToString();
                        string sDTTT = row[27].ToString();
                        string sNgayHuy = row[28].ToString();
                        string sLiDoHuy = row[29].ToString();
                        string sChinhanh = row[30].ToString();

                        msg.message += "<sgtcode>" + sSgtCode;

                        try
                        {
                            model.sgtcode = sSgtCode;
                            model.nguoitao = sSales;
                            model.makh = sMakh;
                            model.tenkh = sTenKh;
                            model.diachi = sDiaChi;
                            model.dienthoai = sDienThoai;
                            model.fax = sFax;
                            model.email = sEmail;
                            model.chudetour = sChudetour;
                            model.tuyentq = sTuyenTQ;
                            model.diemtq = sDiemTQ;
                            model.nguoidaidien = sDaiDienKH;
                            model.loaitourid = sLoaiTour;
                            model.nguoikyhopdong = sNguoiKyHD;
                            model.sohopdong = sSoHD;

                            model.doitacnuocngoai = sDoiTacNcNgoai;
                            model.noidungthanhlyhd = sLiDoHuy;
                            model.chinhanh = sChinhanh;
                        }
                        catch (Exception ex)
                        {
                            //msg.errorCount += 1;
                            bCoLoi = true;
                            //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                            msg.message += "<caccotdau>" + ex.Message + "</caccotdau>";
                        }

                        try
                        {
                            model.sokhachdk = sSKDK == null ? 0 : int.Parse(sSKDK);

                        }
                        catch (Exception ex)
                        {
                            model.sokhachdk = 0;
                        }

                        try
                        {
                            model.doanhthudk = sDTDK == null ? 0 : decimal.Parse(sDTDK);

                        }
                        catch (Exception ex)
                        {
                            model.doanhthudk = 0;
                        }

                        try
                        {
                            model.sokhachtt = sSKTT == null ? 0 : int.Parse(sSKTT);

                        }
                        catch (Exception ex)
                        {
                            model.sokhachtt = 0;
                        }

                        try
                        {
                            model.doanhthutt = sDTTT == null ? 0 : decimal.Parse(sDTTT);
                        }
                        catch (Exception ex)
                        {
                            model.doanhthutt = 0;
                        }

                        if (sNgayTao != null && sNgayTao != "")
                        {
                            try
                            {
                                model.ngaytao = DateTime.Parse(sNgayTao);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                // msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngaytao>" + sNgayTao + " - " + ex.Message + "</ngaytao>";
                            }

                        }

                        if (sNgayDiTour != null && sNgayDiTour != "")
                        {
                            try
                            {
                                model.batdau = DateTime.Parse(sNgayDiTour);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngayditour>" + sNgayDiTour + " - " + ex.Message + "</ngayditour>";
                            }

                        }



                        if (sKetThuc != null && sKetThuc != "")
                        {
                            try
                            {
                                model.ketthuc = DateTime.Parse(sKetThuc);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ketthuc>" + sKetThuc + " - " + ex.Message + "</ketthuc>";
                            }

                        }

                        if (sNgayDamPhan != null && sNgayDamPhan != "")
                        {
                            try
                            {
                                model.ngaydamphan = DateTime.Parse(sNgayDamPhan);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                msg.message += "<ngaydamphan>" + sNgayDamPhan + " - " + ex.Message + "</ngaydamphan>";
                                bCoLoi = true;
                            }

                        }

                        if (sNgayKyHD != null && sNgayKyHD != "")
                        {
                            try
                            {
                                model.ngaykyhopdong = DateTime.Parse(sNgayKyHD);
                            }
                            catch (Exception ex)
                            {
                                // msg.errorCount += 1;
                                // msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngaykyhopdong>" + sNgayKyHD + " - " + ex.Message + "</ngaykyhopdong>";
                            }

                        }

                        if (sNgayDCVeMB != null && sNgayDCVeMB != "")
                        {
                            try
                            {
                                model.hanxuatvmb = DateTime.Parse(sNgayDCVeMB);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<vemb>" + sNgayDCVeMB + " - " + ex.Message + "</vemb>";
                            }

                        }

                        //sNgayThanhLyHD
                        if (sNgayThanhLyHD != null && sNgayThanhLyHD != "")
                        {
                            try
                            {
                                model.ngaythanhlyhd = DateTime.Parse(sNgayThanhLyHD);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngaythanhlyhd>" + sNgayThanhLyHD + " - " + ex.Message + "</ngaythanhlyhd>";
                            }

                        }

                        if (sNgayHuy != null && sNgayHuy != "")
                        {
                            try
                            {
                                model.ngayhuytour = DateTime.Parse(sNgayHuy);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngayhuy>" + sNgayHuy + " - " + ex.Message + "</ngayhuy>";
                            }

                        }


                        //IQueryable<tour> oj = db.tour.Where(x => x.sgtcode == sSgtCode);
                        //bool bTrung = dao.KTTourTrung(sSgtCode);

                        //if (bTrung)  //da nhap  roi , bao loi
                        //{
                        // msg.message = model.sgtcode + " - " + "Dữ liệu đã có rồi";
                        //msg.errorCount += 1;
                        //msg.message += "<kttrungdulieu>Đã có code này rồi</kttrungdulieu>";
                        //bCoLoi = true;
                        // }
                        // else
                        // {
                        //if (model.batdau == null)
                        //{
                        //    //msg.message += model.sgtcode + "Không để trống ngày bắt đầu <br/>";
                        //    msg.message += "<ktbatdau>Không để trống ngày bắt đầu</ktbatdau>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.ketthuc == null)
                        //{
                        //    //msg.message += model.sgtcode + "Không để trống ngày kết thúc <br/>";
                        //    msg.message += "<ktketthuc>Không để trống ngày kết thúc</ktketthuc>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.chudetour == "" || model.chudetour == null)
                        //{
                        //    //msg.message += model.sgtcode + "Không để trống chủ đề tour <br/>";
                        //    msg.message += "<ktchudetour>Không để trống chủ đề tour</ktchudetour>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.tuyentq == "" || model.tuyentq == null)
                        //{
                        //    //msg.message += model.sgtcode + "Không để trống tuyến tham quan <br/>";
                        //    msg.message += "<kttuyentq>Không để trống tuyến tham quan</kttuyentq>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.sokhachdk == null)
                        //{
                        //    // msg.message += model.sgtcode + "Không để trống số khách dự kiến <br/>";
                        //    msg.message += "<ktskdk>Không để trống số khách dự kiến</ktskdk>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.doanhthudk == null)
                        //{
                        //    //msg.message += model.sgtcode + "Không để trống doanh thu dự kiến <br/>";
                        //    msg.message += "<ktdtdk>Không để trống doanh thu dự kiến</ktdtdk>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.ketthuc < model.batdau)
                        //{
                        //    //msg.message += model.sgtcode + "Ngày bắt đầu phải nhỏ hơn ngày kết thúc<br/>";
                        //    msg.message += "<ktbdkt>Ngày bắt đầu phải nhỏ hơn ngày kết thúc</ktbdkt>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.ngaythanhlyhd < model.ketthuc && model.ngaythanhlyhd != null)
                        //{
                        //    // msg.message += model.sgtcode + "Ngày thanh lý nhỏ hơn ngày kết thúc<br/>";
                        //    msg.message += "<ktthanhly>Ngày thanh lý nhỏ hơn ngày kết thúc</ktthanhly>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else if (model.sgtcode.Length > 17)
                        //{
                        //    // msg.message += model.sgtcode + "Ngày thanh lý nhỏ hơn ngày kết thúc<br/>";
                        //    msg.message += "<sgtcodelen>Code đoàn không lớn hơn 17 ký tự</sgtcodelen>";
                        //    //msg.errorCount += 1;
                        //    bCoLoi = true;
                        //}
                        //else
                        //{
                            string id = null;

                            try
                            {
                                if (model.ngaydamphan != null)
                                {
                                    model.trangthai = "1";
                                }


                                if (model.ngaykyhopdong != null)
                                {
                                    model.trangthai = "2";
                                }

                                if (model.ngaythanhlyhd != null)
                                {
                                    model.trangthai = "3";
                                }

                            model.trangthai = "4";

                                id = dao.Insert(model);
                            }
                            catch (Exception ex)
                            {
                                // msg.message += model.sgtcode + " - " + ex.Message + "<br/>";
                                msg.message += "<loithemtour>" + ex.Message + "</loithemtour>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }




                        //}


                        //}

                        if (bCoLoi == true)
                        {
                            msg.errorCount += 1;
                        }
                        else
                        {
                            msg.count += 1;
                        }


                        msg.message += "</sgtcode>";

                    }//end co du lieu moi xu ly

                }//end foreach
                 //  msg.message += "</Sheet>";
            }

            return msg;
        }

        public UploadMessage ThemTourExcel(DataTable dt)
        {

            UploadMessage msg = new UploadMessage();
            if (dt.Rows.Count > 0)
            {
                //du lieu file Excel có các cột sau
                //Họ tên	Hộ chiếu	Ngày hết hạn HC	ngày sinh	CMND	Ngày làm CMND	Nơi cấp CMND	Phái	Điện thoại 	Quốc tịch

                

                var dao = new tourDAO();
                msg.message = "<sodong>"+dt.Rows.Count+"</sodong>";

                foreach (DataRow row in dt.Rows)
                {
                    tour model = new tour();
                    bool bCoLoi = false;//mac dinh la khong co loi
                    

                    if (row[0].ToString() != "") //co du lieu moi xu ly
                    {
                        


                        string sSTT = row[0].ToString();//chua su dung
                        string sSgtCode = row[1].ToString();
                        if (sSgtCode != null) sSgtCode = sSgtCode.Trim();
                        string sSales = row[2].ToString();
                        string sNgayTao = row[3].ToString();
                        string sMakh = row[4].ToString();
                        string sTenKh = row[5].ToString();
                        string sDiaChi = row[6].ToString();
                        string sDienThoai = row[7].ToString();
                        string sFax = row[8].ToString();
                        string sEmail = row[9].ToString();
                        if (sEmail != null) sEmail = sEmail.Trim();
                        string sNgayDiTour = row[10].ToString();
                        string sKetThuc = row[11].ToString();
                        string sChudetour = row[12].ToString();
                        string sTuyenTQ = row[13].ToString();
                        string sDiemTQ = row[14].ToString();
                        string sNgayDamPhan = row[15].ToString();
                        string sDaiDienKH = row[16].ToString();
                        string sLoaiTour = row[17].ToString();
                        string sNgayKyHD = row[18].ToString();
                        string sNguoiKyHD = row[19].ToString();
                        string sSoHD = row[20].ToString();
                        string sSKDK = row[21].ToString();
                        string sDTDK = row[22].ToString();
                        string sNgayDCVeMB = row[23].ToString();
                        string sDoiTacNcNgoai = row[24].ToString();
                        string sNgayThanhLyHD = row[25].ToString();
                        string sSKTT = row[26].ToString();
                        string sDTTT = row[27].ToString();
                        string sNgayHuy = row[28].ToString();
                        string sLiDoHuy = row[29].ToString();
                        string sChinhanh = row[30].ToString();

                        msg.message += "<sgtcode>" + sSgtCode;

                        try
                        {
                            model.sgtcode = sSgtCode;
                            model.nguoitao = sSales;
                            model.makh = sMakh;
                            model.tenkh = sTenKh;
                            model.diachi = sDiaChi;
                            model.dienthoai = sDienThoai;
                            model.fax = sFax;
                            model.email = sEmail;
                            model.chudetour = sChudetour;
                            model.tuyentq = sTuyenTQ;
                            model.diemtq = sDiemTQ;
                            model.nguoidaidien = sDaiDienKH;
                            model.loaitourid = sLoaiTour;
                            model.nguoikyhopdong = sNguoiKyHD;
                            model.sohopdong = sSoHD;

                            model.doitacnuocngoai = sDoiTacNcNgoai;
                            model.noidungthanhlyhd = sLiDoHuy;
                            model.chinhanh = sChinhanh;
                        }
                        catch (Exception ex)
                        {
                            //msg.errorCount += 1;
                            bCoLoi = true;
                            //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                            msg.message += "<caccotdau>"+ex.Message+"</caccotdau>";
                        }

                        try
                        {
                            model.sokhachdk = sSKDK == null ? 0 : int.Parse(sSKDK);

                        }
                        catch (Exception ex)
                        {
                            model.sokhachdk = 0;
                        }

                        try
                        {
                            model.doanhthudk = sDTDK == null ? 0 : decimal.Parse(sDTDK);

                        }
                        catch (Exception ex)
                        {
                            model.doanhthudk = 0;
                        }

                        try
                        {
                            model.sokhachtt = sSKTT == null ? 0 : int.Parse(sSKTT);

                        }
                        catch (Exception ex)
                        {
                            model.sokhachtt = 0;
                        }

                        try
                        {
                            model.doanhthutt = sDTTT == null ? 0 : decimal.Parse(sDTTT);
                        }
                        catch (Exception ex)
                        {
                            model.doanhthutt = 0;
                        }

                        if (sNgayTao != null && sNgayTao != "")
                        {
                            try
                            {
                                model.ngaytao = DateTime.Parse(sNgayTao);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                // msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngaytao>" + sNgayTao + " - " + ex.Message+"</ngaytao>";
                            }

                        }

                        if (sNgayDiTour != null && sNgayDiTour != "")
                        {
                            try
                            {
                                model.batdau = DateTime.Parse(sNgayDiTour);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngayditour>"+ sNgayDiTour+" - " + ex.Message + "</ngayditour>";
                            }

                        }



                        if (sKetThuc != null && sKetThuc != "")
                        {
                            try
                            {
                                model.ketthuc = DateTime.Parse(sKetThuc);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ketthuc>" + sKetThuc + " - " + ex.Message + "</ketthuc>";
                            }

                        }

                        if (sNgayDamPhan != null && sNgayDamPhan != "")
                        {
                            try
                            {
                                model.ngaydamphan = DateTime.Parse(sNgayDamPhan);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                msg.message += "<ngaydamphan>" + sNgayDamPhan + " - " + ex.Message + "</ngaydamphan>";
                                bCoLoi = true;
                            }

                        }

                        if (sNgayKyHD != null && sNgayKyHD != "")
                        {
                            try
                            {
                                model.ngaykyhopdong = DateTime.Parse(sNgayKyHD);
                            }
                            catch (Exception ex)
                            {
                                // msg.errorCount += 1;
                                // msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngaykyhopdong>" + sNgayKyHD + " - " + ex.Message + "</ngaykyhopdong>";
                            }

                        }

                        if (sNgayDCVeMB != null && sNgayDCVeMB != "")
                        {
                            try
                            {
                                model.hanxuatvmb = DateTime.Parse(sNgayDCVeMB);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<vemb>" + sNgayDCVeMB + " - " + ex.Message + "</vemb>";
                            }

                        }

                        //sNgayThanhLyHD
                        if (sNgayThanhLyHD != null && sNgayThanhLyHD != "")
                        {
                            try
                            {
                                model.ngaythanhlyhd = DateTime.Parse(sNgayThanhLyHD);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngaythanhlyhd>" + sNgayThanhLyHD + " - " + ex.Message + "</ngaythanhlyhd>";
                            }

                        }

                        if (sNgayHuy != null && sNgayHuy != "")
                        {
                            try
                            {
                                model.ngayhuytour = DateTime.Parse(sNgayHuy);
                            }
                            catch (Exception ex)
                            {
                                //msg.errorCount += 1;
                                //msg.message += model.sgtcode + " - " + ex.Message + ";<br/>";
                                bCoLoi = true;
                                msg.message += "<ngayhuy>" + sNgayHuy + " - " + ex.Message + "</ngayhuy>";
                            }

                        }


                        //IQueryable<tour> oj = db.tour.Where(x => x.sgtcode == sSgtCode);
                        bool bTrung = dao.KTTourTrung(sSgtCode);

                        if (bTrung)  //da nhap  roi , bao loi
                        {
                           // msg.message = model.sgtcode + " - " + "Dữ liệu đã có rồi";
                            //msg.errorCount += 1;
                            msg.message += "<kttrungdulieu>Đã có code này rồi</kttrungdulieu>";
                            bCoLoi = true;
                        }
                        else
                        {
                            if (model.batdau == null)
                            {
                                //msg.message += model.sgtcode + "Không để trống ngày bắt đầu <br/>";
                                msg.message += "<ktbatdau>Không để trống ngày bắt đầu</ktbatdau>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            else if (model.ketthuc == null)
                            {
                                //msg.message += model.sgtcode + "Không để trống ngày kết thúc <br/>";
                                msg.message += "<ktketthuc>Không để trống ngày kết thúc</ktketthuc>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            //else if (model.chudetour == "" || model.chudetour == null)
                            //{
                            //    //msg.message += model.sgtcode + "Không để trống chủ đề tour <br/>";
                            //    msg.message += "<ktchudetour>Không để trống chủ đề tour</ktchudetour>";
                            //    //msg.errorCount += 1;
                            //    bCoLoi = true;
                            //}
                            //else if (model.tuyentq == "" || model.tuyentq == null)
                            //{
                            //    //msg.message += model.sgtcode + "Không để trống tuyến tham quan <br/>";
                            //    msg.message += "<kttuyentq>Không để trống tuyến tham quan</kttuyentq>";
                            //    //msg.errorCount += 1;
                            //    bCoLoi = true;
                            //}
                            else if (model.sokhachdk == null)
                            {
                                // msg.message += model.sgtcode + "Không để trống số khách dự kiến <br/>";
                                msg.message += "<ktskdk>Không để trống số khách dự kiến</ktskdk>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            else if (model.doanhthudk == null)
                            {
                                //msg.message += model.sgtcode + "Không để trống doanh thu dự kiến <br/>";
                                msg.message += "<ktdtdk>Không để trống doanh thu dự kiến</ktdtdk>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            else if (model.ketthuc < model.batdau)
                            {
                                //msg.message += model.sgtcode + "Ngày bắt đầu phải nhỏ hơn ngày kết thúc<br/>";
                                msg.message += "<ktbdkt>Ngày bắt đầu phải nhỏ hơn ngày kết thúc</ktbdkt>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            else if (model.ngaythanhlyhd < model.ketthuc && model.ngaythanhlyhd != null)
                            {
                                // msg.message += model.sgtcode + "Ngày thanh lý nhỏ hơn ngày kết thúc<br/>";
                                msg.message += "<ktthanhly>Ngày thanh lý nhỏ hơn ngày kết thúc</ktthanhly>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            else if (model.sgtcode.Length>17)
                            {
                                // msg.message += model.sgtcode + "Ngày thanh lý nhỏ hơn ngày kết thúc<br/>";
                                msg.message += "<sgtcodelen>Code đoàn không lớn hơn 17 ký tự</sgtcodelen>";
                                //msg.errorCount += 1;
                                bCoLoi = true;
                            }
                            else
                            {
                                string id = null;

                                try
                                {
                                    if (model.ngaydamphan != null)
                                    {
                                        model.trangthai = "1";
                                    }


                                    if (model.ngaykyhopdong != null)
                                    {
                                        model.trangthai = "2";
                                    }

                                    if (model.ngaythanhlyhd != null)
                                    {
                                        model.trangthai = "3";
                                    }

                                    id = dao.Insert(model);
                                }
                                catch (Exception ex)
                                {
                                    // msg.message += model.sgtcode + " - " + ex.Message + "<br/>";
                                    msg.message += "<loithemtour>" +ex.Message+ "</loithemtour>";
                                    //msg.errorCount += 1;
                                    bCoLoi = true;
                                }


                                //if (id != null)
                                //{
                                //    msg.count += 1;
                                //}
                                //else
                                //{
                                //    msg.message += "<br/>";
                                //    msg.errorCount += 1;

                                //}

                            }


                        }

                        if (bCoLoi == true)
                        {
                            msg.errorCount += 1;
                        }else
                        {
                            msg.count += 1;
                        }


                        msg.message+="</sgtcode>";

                    }//end co du lieu moi xu ly

                }//end foreach
              //  msg.message += "</Sheet>";
            }

            return msg;
        }

        public ActionResult ImportTourFromExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportTourFromExcel(HttpPostedFileBase uploadExcel)
        {
            if (Request.Files.Count > 0)
            {
                //xoa thong bao truoc do
                TempData["AlertType"] = null;
                TempData["AlertMessage"] = null;

                //var file = Request.Files[0];

                if (uploadExcel != null && uploadExcel.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(uploadExcel.FileName);
                    var path = Path.Combine(Server.MapPath("~/ExcelFiles/Tour/"), fileName);

                    if (Path.GetExtension(fileName).ToLower() == ".xlsx" || Path.GetExtension(fileName).ToLower() == ".xls")
                    {

                        ExcelPackage package = new ExcelPackage(uploadExcel.InputStream);
                        //DataTable dt = EPPLusExtensions.ExcelToDataTableRichText(package);
                        DataSet ds = EPPLusExtensions.ExcelToDataTableMultiSheet(package);
                        UploadMessage message = new UploadMessage();
                        List<UploadMessage> messages = new List<UploadMessage>();

                        try
                        {
                            foreach (DataTable dt in ds.Tables)
                            {
                               // message = UpdateTourExcel(dt);
                                 message = ThemTourExcel(dt);
                                // message = ThemTourExcelKhongKiemTraDK(dt);
                                messages.Add(message);
                            }

                            string sMsg = "<Tours>";
                            foreach (UploadMessage msg in messages)
                            {
                                sMsg = sMsg + "<Sheet>";
                                sMsg = sMsg + "<thanhcong>"+msg.count+"</thanhcong>";
                                sMsg = sMsg + "<thatbai>" + msg.errorCount + "</thatbai>";
                                sMsg = sMsg + msg.message;
                                sMsg = sMsg + "</Sheet>";
                            }
                            sMsg = sMsg + "</Tours>";
                            SetAlert(sMsg, "warning");                          

                            package = null;
                            ds = null;
                            message = null;

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
                    //sau khi xu lý
                    uploadExcel = null;


                }

                return RedirectToAction("ImportTourFromExcel", "QLKhachDoan");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ShowError()
        {
            return View();
        }

        public ActionResult GetDsNN(ObjDsNN o)
        {
            var dao = new NguyenNhanDAO();

            //List<dmkhachhang> lst = dao.GetDmKh(o.lstmakh);
            var lst = dao.GetDsNN(o.lstnn);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// lay chi tiet tour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CTTour(int id)
        {
            var dao = new tourDAO();
            var model = dao.Details(id);

            return View(model);
        }

        /// <summary>
        /// lay chi tiet tour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LayCTChuongTrinhTour(int id)
        {
            var dao = new tourDAO();
            var model = dao.Details(id);

            return View(model);
        }

        /// <summary>
        /// lay danh muc hoa hong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LayDmHoaHong(decimal id)
        {
            var dao = new hoahongDAO();
            var model = dao.GetDSHoaHong(id);
            ViewBag.idtour = id;
            //lay sgtcode
            var dao1 = new tourDAO();
            var model1 = dao1.Details(id);
            ViewBag.sgtcode = model1.sgtcode;
            return View(model);
        }

        /// <summary>
        /// lay danh muc hoa hong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LayLichSuThamGiaTour(string id)
        {
            var dao = new dmkhachhangDAO();
            dmkhachhang model = new dmkhachhang();
            model = dao.Details(id);
            return View(model);
        }


        /// <summary>
        /// lay ds khach di tour, id la id tour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LayDSKhachTour(decimal id)
        {
            //var dao = new dmkhachtourDAO();
            //var model = dao.GetDSKhachTour(id);

            var dao = new tourDAO();
            var model = dao.Details(id);

            ViewBag.idtour = id;
            return View(model);
        }

        public ActionResult PrintDSKhach(decimal id)
        {
            string Filename = "DSKhachDiTour_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            // Gọi lại hàm để tạo file excel
            var stream = CreateExcelKhachDiTour(id);
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

        public List<dmkhachtour> GetDSKhachDiTour(decimal id)
        {
            List<dmkhachtour> lst = new List<dmkhachtour>();
            var dao = new dmkhachtourDAO();
            lst = dao.GetDSKhachTour(id);
            return lst;
        }

        private Stream CreateExcelKhachDiTour(decimal id, Stream stream = null)
        {
            List<dmkhachtour> list = GetDSKhachDiTour(id);
            tourDAO dao = new tourDAO();
            tour tt = dao.Details(id);
            //list = FormatBCTheoDB(list);
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Trung";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Danh sách khách đi tour";
                // thêm tí comments vào làm màu
                excelPackage.Workbook.Properties.Comments = "Comments";
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add("DSKhachTour");
                // Lấy Sheet bạn vừa mới tạo ra để thao tác
                var workSheet = excelPackage.Workbook.Worksheets[1];
                // Đổ data vào Excel file
                workSheet = FormatWorkSheetDSKhachTour(tt, list, workSheet);

                //BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        public ExcelWorksheet FormatWorkSheetDSKhachTour(tour tt, List<dmkhachtour> dt, ExcelWorksheet ew)
        {
            //TEN KHACH-NGAY SINH - HO CHIEU - HIEU LUC - CMND - NGAY CAP - NOI CAP - PHAI - DT - QUOC TICH
            int iColReport = 10;

            ExcelWorksheet ewres = ew;

            ewres.Cells[1, 1].Value = "DANH SÁCH KHÁCH ĐI TOUR: " + tt.sgtcode;
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            ewres.Cells[1, 10].Merge = true;

            ewres.Cells[2, 1].Value = "CHỦ ĐỀ TOUR " + tt.chudetour;
            ewres.Cells[2, 1].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            ewres.Cells[2, 10].Merge = true;


            ewres.Cells[3, 1].LoadFromText("Tên khách");
            ewres.Cells[3, 2].LoadFromText("Ngày sinh");
            ewres.Cells[3, 3].LoadFromText("Hộ chiếu");
            ewres.Cells[3, 4].LoadFromText("Hiệu lực");
            ewres.Cells[3, 5].LoadFromText("CMND");
            ewres.Cells[3, 6].LoadFromText("Ngày cấp");
            ewres.Cells[3, 7].LoadFromText("Nơi cấp");
            ewres.Cells[3, 8].LoadFromText("Phái");
            ewres.Cells[3, 9].LoadFromText("Điện thoại");
            ewres.Cells[3, 10].LoadFromText("Quốc tịch");

            //create header
            // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
            using (var range = ewres.Cells[3, 1, 3, iColReport])
            {
                range.Style.WrapText = false;
                // Canh giữa cho các text
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại
                range.Style.Font.SetFromFont(new Font("Arial", 10, FontStyle.Bold));
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

            //TEN KHACH-NGAY SINH - HO CHIEU - HIEU LUC - CMND - NGAY CAP - NOI CAP - PHAI - DT - QUOC TICH
            int iRowIndex = 4;
            foreach (dmkhachtour v in dt)
            {
                ewres.Cells[iRowIndex, 1].Value = v.tenkhach;

                try
                {
                    if (v.ngaysinh != null)
                    {
                        ewres.Cells[iRowIndex, 2].Value = DateTime.Parse(v.ngaysinh.ToString()).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ewres.Cells[iRowIndex, 2].Value = "";
                    }
                }
                catch
                {
                    ewres.Cells[iRowIndex, 2].Value = "";
                }

                ewres.Cells[iRowIndex, 3].Value = v.hochieu;

                try
                {
                    if (v.hieuluchochieu != null)
                    {
                        ewres.Cells[iRowIndex, 4].Value = DateTime.Parse(v.hieuluchochieu.ToString()).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ewres.Cells[iRowIndex, 4].Value = "";
                    }
                }
                catch
                {
                    ewres.Cells[iRowIndex, 4].Value = "";
                }

                ewres.Cells[iRowIndex, 5].Value = v.socmnd;

                try
                {
                    if (v.ngaycmnd != null)
                    {
                        ewres.Cells[iRowIndex, 6].Value = DateTime.Parse(v.ngaycmnd.ToString()).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ewres.Cells[iRowIndex, 6].Value = "";
                    }
                }
                catch
                {
                    ewres.Cells[iRowIndex, 6].Value = "";
                }

                ewres.Cells[iRowIndex, 7].Value = v.noicapcmnd;

                string sPhai = "";
                if (v.phai == "1")
                {
                    sPhai = "Nam";
                }
                else if (v.phai == "2")
                {
                    sPhai = "Nữ";
                }
                else if (v.phai == "3")
                {
                    sPhai = "Khác";
                }

                ewres.Cells[iRowIndex, 8].Value = sPhai;
                ewres.Cells[iRowIndex, 9].Value = v.dienthoai;
                ewres.Cells[iRowIndex, 10].Value = v.quoctich;

                using (var range = ewres.Cells[iRowIndex, 1, iRowIndex, iColReport])
                {
                    range.Style.WrapText = false;
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


            ewres.Cells[ewres.Dimension.Address].AutoFitColumns();

            for (int col = 1; col <= ewres.Dimension.End.Column; col++)
            {
                ewres.Column(col).Width = ewres.Column(col).Width + 1;
            }

            return ewres;
        }

        public List<SelectListItem> TaoListPhai()
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Nam", Value = "1" },
                new SelectListItem() { Text = "Nữ", Value = "2" },
                new SelectListItem() { Text = "Khác", Value = "3" }
            };

            return items;
        }

        public ActionResult ThemKhach(decimal id)
        {
            dmkhachtour model = new dmkhachtour();

            ViewBag.phai = new SelectList(TaoListPhai(), "Value", "Text");
            model.idtour = id;
            return View(model);
        }

        /// <summary>
        /// sua thong tin khach di tour, id: id khach
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditKhach(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new dmkhachtourDAO();
            dmkhachtour model = dao.KhachDetails(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.phai = new SelectList(TaoListPhai(), "Value", "Text", model.phai);
            return View(model);
        }


        public ActionResult XoaKhach(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmkhachtourDAO();

            try
            {
                string res = dao.XoaKhachTour(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index");
        }

        //EditKhachTour
        public ActionResult EditKhachTour(dmkhachtour model)
        {
            var dao = new dmkhachtourDAO();

            try
            {
                string sUsrId = Session["userId"].ToString();

                string id = dao.EditKhachTour(model);

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

            return View();
        }

        public ActionResult CreateKhachTour(dmkhachtour model)
        {
            var dao = new dmkhachtourDAO();

            try
            {
                string sUsrId = Session["userId"].ToString();

                string id = dao.InsertKhachTour(model);

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

            return View();
        }

        public ActionResult GetSTTSoHD(ObjSoHopDong o)
        {
            var dao = new tourDAO();
            int nam = 0, thang = 0;
            string sSTT = "";
            int iSTT = 0;
            string sBatDau = "";
            string sSHD = "", sThang = "";
            sBatDau = o.batdau;

            try
            {
                if (sBatDau.Length == 10)
                {
                    sThang = sBatDau.Substring(3, 2);
                    nam = int.Parse(sBatDau.Substring(6, 4));
                    thang = int.Parse(sThang);

                    sSTT = dao.GetStrSTTCuaSoHopDong(nam, thang);
                    iSTT = int.Parse(sSTT);
                    iSTT = iSTT + 1;
                    //dinh dang so hop dong  thang cua ngay bat dau di tour + 3 So thu tu, tong cong 5 ky tu
                    switch (sThang.Length)
                    {
                        case 1:
                            sThang = "0" + sThang;
                            break;
                    }

                    switch (iSTT.ToString().Length)
                    {
                        case 1:
                            sSHD = sThang + "00" + iSTT.ToString();
                            break;
                        case 2:
                            sSHD = sThang + "0" + iSTT.ToString();
                            break;
                        case 3:
                            sSHD = sThang + iSTT.ToString();
                            break;
                        default:
                            sSHD = sThang + "001";
                            break;
                    }
                }

            }
            catch (Exception)
            {
                sSHD = sThang + "001";
            }

            return Json(sSHD, JsonRequestBehavior.AllowGet);
        }


        public string TaoCode(string chinhanh, string year)
        {
            string sResSgtCode = "";

            int iYear = 0;
            if (year != null && year != "") iYear = int.Parse(year);

            //LAY DS TOUR THEO NĂM TẠO va cung chi nhanh
            var dstour = db.tour.Where(x => x.ngaytao.Value.Year == iYear && x.chinhanh == chinhanh);
            var yc = (from y in dstour orderby y.ngaytao descending select y).FirstOrDefault();

            //STSTOB-2018-01490
            //CHI NHÁNH + 084 + SỐ NĂM + STT
            //STS084-2019-00001
            int stt = 0;
            string sSTT = "";
            if (yc != null)
            {
                string sSgtCode = yc.sgtcode;

                sSTT = sSgtCode == "" ? "0" : sSgtCode.Substring(12);                

                string stt_ = "";
                stt = int.Parse(sSTT) + 1;
                switch (stt.ToString().Length)
                {
                    case 1:
                        stt_ = "0000" + stt.ToString();
                        break;
                    case 2:
                        stt_ = "000" + stt.ToString();
                        break;
                    case 3:
                        stt_ = "00" + stt.ToString();
                        break;
                    case 4:
                        stt_ = "0" + stt.ToString();
                        break;
                    case 5:
                        stt_ = stt.ToString();
                        break;
                    default:
                        break;
                }
                sResSgtCode = "STN084-" + year + "-" + stt_;
            }
            else
                sResSgtCode = "STN084-" + year + "-" + "00001";
            return sResSgtCode;
        }

        [HttpGet]
        public ActionResult Create()
        {
            chinhanhDAO chinhanhDAO = new chinhanhDAO();
            TempData["chinhanh"] = chinhanhDAO.Chinhanhs();
            
            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            ViewBag.chiNhanhs = new SelectList(chinhanhDAO.Chinhanhs(), "chinhanh1", "chinhanh1", sChinhanh);
            string sRole = Session["RoleName"].ToString();

            tour model = new tour();
            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["username"].ToString();
            ViewBag.loaitourid = new SelectList(db.loaitour, "tenloaitour", "tenloaitour");
            ViewBag.dmquocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "Id", "TenNuoc");
            ViewBag.dmthanhpho = new SelectList(db.quan.OrderBy(x => x.tenquan), "maquan", "tenquan");
            ViewBag.nguontour = new SelectList(DungChung.ListNguonTour(), "value", "text");
            // ViewBag.dmkh = new SelectList(db.dmkhachhang.OrderBy(x=>x.tengiaodich), "makh", "tengiaodich");
            dmkhachhangDAO dmkhDAO = new dmkhachhangDAO();
            List<DDLDmKH> lst = dmkhDAO.GetDDLDmKH(sUserId, sRole, sChinhanh);
            ViewBag.dmkh = new SelectList(lst.OrderBy(x => x.tengiaodich), "makh", "tengiaodich");

            //lay chinhanh            
            users usr = db.users.Where(x => x.userId == sUserId).FirstOrDefault();
            string sCN = usr.chinhanh;

            //model.sgtcode = TaoCode(sCN, DateTime.Now.ToString("yyyy"));
            return View(model);
        }


        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tour model, HttpPostedFileBase fileChuongTrinhTour)
        {
            var dao = new tourDAO();

            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            string sRole = Session["RoleName"].ToString();

            try
            {
                string sUsrId = Session["userId"].ToString();
                model.ngaytao = DateTime.Now;
                model.nguoitao = Session["username"].ToString();
                model.trangthai = "0";//mac dinh la moi tao

                //lay chinhanh
                users usr = db.users.Where(x => x.userId == sUsrId).FirstOrDefault();
                model.daily = usr.daily;
                model.chinhanh = usr.chinhanh;
                

                DateTime d1 = DateTime.Parse(model.batdau.ToString());
                DateTime d2 = DateTime.Parse(model.ketthuc.ToString());

                string sMakh = model.makh == null ? "" : model.makh;

                string sSgtCode = dao.TaoCodeDoan(model.chinhanh, d1, d2, 0, model.diemtq, model.chudetour, sMakh, model.nguoitao);
                // model.sgtcode = TaoCode(usr.chinhanh, DateTime.Now.ToString("yyyy"));
                model.sgtcode = sSgtCode;
                //kiem tra trung du lieu
                tour t = db.tour.Where(x => x.sgtcode == sSgtCode).FirstOrDefault();

                if (t != null)
                {
                    SetAlert("Đã có code đoàn này!", "error");
                    return RedirectToAction("Index", "QLKhachDoan");
                }

                if (model.ngaydamphan != null)
                {
                    model.trangthai = "1";
                }

                if (fileChuongTrinhTour != null)
                {
                    if (fileChuongTrinhTour.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileChuongTrinhTour.FileName);
                        fileName = "CTT_" + model.sgtcode + '_' + fileName;
                        string sUploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                        var path = Path.Combine(Server.MapPath(sUploadPath), fileName);

                        string sAllowUpFile = ConfigurationManager.AppSettings["AllowUploadFile"].ToString();
                        string sExt = Path.GetExtension(fileChuongTrinhTour.FileName);

                        if (sAllowUpFile.Contains(sExt.ToLower()))
                        {
                           string s = MyUploadFile(DungChung.FTP_PATH + DungChung.CTTOUR_FTP_PATH, DungChung.USER, DungChung.PASS, fileChuongTrinhTour, model.sgtcode, "CTT_");
                            model.chuongtrinhtour = fileName;
                        }
                        else
                        {
                            SetAlert("Định dạng file không cho phép!", "warning");

                            if (Session["chuongtrinhtour"] != null)
                            {
                                model.chuongtrinhtour = Session["chuongtrinhtour"].ToString();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
              

            

                if (sSgtCode == "")
                {
                    SetAlert("Có lỗi khi tạo code, xin tạo lại!", "error");
                }
                else
                {
                    model.sgtcode = sSgtCode;
                    string id = dao.Insert(model);

                    if (id != null)
                    {
                        SetAlert("Thêm Thành Công", "success");

                        try
                        {

                            tourlogDAO dao1 = new tourlogDAO();
                            tourlog modellog = new tourlog();
                            modellog = DungChung.SetModelGhiLog(modellog, model, Session["username"].ToString(), "HuyTour");
                            dao1.Insert(modellog);
                        }
                        catch (Exception ex)
                        {

                        }

                        return RedirectToAction("Index", "QLKhachDoan");
                    }
                    else
                    {
                        SetAlert("Thêm Không Thành Công", "warning");
                    }
                }              

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi:" + ex.Message, "warning");
            }

            ViewBag.loaitourid = new SelectList(db.loaitour, "tenloaitour", "tenloaitour");
            ViewBag.dmquocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "Id", "TenNuoc");
            ViewBag.dmthanhpho = new SelectList(db.quan.OrderBy(x => x.tenquan), "maquan", "tenquan");
            dmkhachhangDAO dmkhDAO = new dmkhachhangDAO();
            List<DDLDmKH> lst = dmkhDAO.GetDDLDmKH(sUserId, sRole, sChinhanh);
            ViewBag.dmkh = new SelectList(lst.OrderBy(x => x.tengiaodich), "makh", "tengiaodich");
            ViewBag.nguontour = new SelectList(DungChung.ListNguonTour(), "value", "text");
            return View(model);
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }

       

        public ActionResult GetDmNguyenNhan(ObjDsNN o)
        {
            var dao = new NguyenNhanDAO();

            List<cacnoidunghuytour> lst = dao.GetDSNguyenNhanTheoND(o.noidung);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDmKhachhangTheoMaKH(string makh)
        {
            var dao = new dmkhachhangDAO();

            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            string sRole = Session["RoleName"].ToString();

            List<dmkhachhang> lst = dao.GetAllKH(makh, sUserId, sRole, sChinhanh);

            //var khlst = (from k in lst where k.makh.StartsWith(makh) || (k.makhold != null && k.makhold.StartsWith(makh)) select new { k.makh, k.makhold, k.tengiaodich, k.email, k.diachi, k.telephone, k.fax });
            var khlst = (from k in lst where k.makh.StartsWith(makh) || (k.codecn != null && k.codecn.StartsWith(makh)) select new { k.makh, k.codecn, k.tengiaodich, k.email, k.diachi, k.telephone, k.fax });

            return Json(khlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDmKhachhang(ObjDmKhachHang o)
        {
            var dao = new dmkhachhangDAO();

            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            string sRole = Session["RoleName"].ToString();

            List<DDLDmKH> lst = dao.GetDDLDmKHByTenGiaoDich(o.tengiaodich, sUserId, sRole, sChinhanh);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuan(ObjDmQuan o)
        {
            var dao = new tourDAO();

            List<quan> lst = dao.GetQuanByLstId(o.lstquan);
            lst.Reverse();

            string s = "";
            foreach (quan q in lst)
            {
                s += q.tenquan + "-";
            }

            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDmKh(ObjDmKH o)
        {
            var dao = new tourDAO();

            //List<dmkhachhang> lst = dao.GetDmKh(o.lstmakh);
            var lst = dao.GetDmKh(o.lstmakh);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDmquan(ObjDmQuocGia o)
        {
            var dao = new tourDAO();

            List<quan> lst = dao.GetQuan(o.quocgia);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.chiNhanhTao = Session["chinhanh"].ToString();
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var dao = new tourDAO();
            tour model = dao.Details(id);

            // chinhanh dh
            chinhanhDAO chinhanhDAO = new chinhanhDAO();
            TempData["chinhanh"] = chinhanhDAO.Chinhanhs();
            ViewBag.chiNhanhs = new SelectList(chinhanhDAO.Chinhanhs(), "chinhanh1", "chinhanh1", model.ChiNhanhDH);
            ViewBag.chiNhanhDH = model.ChiNhanhDH;
            // chinhanh dh

            if (model == null)
            {
                return HttpNotFound();
            }

            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            string sRole = Session["RoleName"].ToString();

            //users usr = db.users.Where(x => x.userId == model.nguoitao).FirstOrDefault();
            users usr = db.users.Where(x => x.username == model.nguoitao || x.fullName==model.nguoitao).FirstOrDefault();

            Session["nguoitao"] = model.nguoitao;
            Session["ngaytao"] = model.ngaytao;
            Session["ndthanhly"] = model.noidungthanhlyhd;
            Session["nddichvu"] = model.dichvu;
            Session["chuongtrinhtour"] = model.chuongtrinhtour;
            Session["filekhachditour"] = model.filekhachditour;
            Session["filevemaybay"] = model.filevemaybay;
            Session["ngaybatdau"] = model.batdau;
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();


            try
            {
                model.nguoitao = usr.fullName;
            }
            catch
            {
                model.nguoitao = "";
            }


            ViewBag.loaitourid = new SelectList(db.loaitour, "tenloaitour", "tenloaitour", model.loaitourid);
            ViewBag.dmquocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "Id", "TenNuoc");
            ViewBag.dmthanhpho = new SelectList(db.quan.OrderBy(x => x.tenquan), "maquan", "tenquan");
            ViewBag.nguontour = new SelectList(DungChung.ListNguonTour(), "value", "text", model.nguontour);
            //ViewBag.dmkh = new SelectList(db.dmkhachhang.OrderBy(x => x.tengiaodich), "makh", "tengiaodich");
            dmkhachhangDAO dmkhDAO = new dmkhachhangDAO();
            List<DDLDmKH> lst = dmkhDAO.GetDDLDmKH(sUserId, sRole, sChinhanh);
            ViewBag.dmkh = new SelectList(lst.OrderBy(x => x.tengiaodich), "makh", "tengiaodich");


            ViewBag.nguyennhanhuythau = new MultiSelectList(db.cacnoidunghuytour, "idhuytour", "noidung");

            //nhan tu trang danh sach tour
            ViewBag.ngayditourb = Request["ngayditourb"];
            ViewBag.ngayditoure = Request["ngayditoure"];

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tour model, HttpPostedFileBase fileThanhLyHD, HttpPostedFileBase fileDV, HttpPostedFileBase fileChuongTrinhTour, HttpPostedFileBase fileDSKhach, HttpPostedFileBase fileVMB)
        {
            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            string sRole = Session["RoleName"].ToString();

            var session = Session["username"];
            ViewBag.loaitourid = new SelectList(db.loaitour, "tenloaitour", "tenloaitour", model.loaitourid);
            ViewBag.dmquocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "Id", "TenNuoc");
            ViewBag.dmthanhpho = new SelectList(db.quan.OrderBy(x => x.tenquan), "maquan", "tenquan");
            // ViewBag.dmkh = new SelectList(db.dmkhachhang.OrderBy(x => x.tengiaodich), "makh", "tengiaodich");
            ViewBag.nguontour = new SelectList(DungChung.ListNguonTour(), "value", "text", model.nguontour);
            dmkhachhangDAO dmkhDAO = new dmkhachhangDAO();
            List<DDLDmKH> lst = dmkhDAO.GetDDLDmKH(sUserId, sRole, sChinhanh);
            ViewBag.dmkh = new SelectList(lst.OrderBy(x => x.tengiaodich), "makh", "tengiaodich");
            ViewBag.nguyennhanhuythau = new MultiSelectList(db.cacnoidunghuytour, "idhuytour", "noidung");
            //if (ModelState.IsValid)
            //{
            try
            {
                var dao = new tourDAO();

                string sNguoiTao = "";
                if (Session["nguoitao"] != null)
                {
                    sNguoiTao = Session["nguoitao"].ToString();
                    //var usr = db.users.Where(x => x.userId == sNguoiTao).FirstOrDefault();
                    //var usr = db.users.Where(x => x.username == sNguoiTao).FirstOrDefault();
                    var usr = db.users.Where(x => x.username == sNguoiTao || x.fullName==sNguoiTao).FirstOrDefault();

                    if (usr != null)
                    {
                        model.nguoitao = usr.username;
                        //lay chinhanh                       
                        model.daily = usr.daily;
                        model.chinhanh = usr.chinhanh;
                    }
                    else
                    {
                        model.nguoitao = "";
                        //lay chinhanh                       
                        model.daily = "";
                        model.chinhanh = Session["chinhanh"].ToString();
                    }

                    if (Session["ngaytao"] != null)
                    {
                        model.ngaytao = DateTime.Parse(Session["ngaytao"].ToString());
                    }
                }

                if (model.ngaydamphan != null)
                {
                    model.trangthai = "1";
                }


                if (model.ngaykyhopdong != null)
                {
                    model.trangthai = "2";
                }

                if (model.ngaythanhlyhd != null)
                {
                    model.trangthai = "3";
                }

                //luu file upload

                if (fileThanhLyHD != null)
                {
                    if (fileThanhLyHD.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileThanhLyHD.FileName);
                        fileName = "TL_" + model.sgtcode + '_' + fileName;
                        string sUploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                        var path = Path.Combine(Server.MapPath(sUploadPath), fileName);

                        string sAllowUpFile = ConfigurationManager.AppSettings["AllowUploadFile"].ToString();
                        string sExt = Path.GetExtension(fileThanhLyHD.FileName);

                        if (sAllowUpFile.Contains(sExt.ToLower()))
                        {
                            //fileThanhLyHD.SaveAs(path);

                            //dung FTP
                            //string s = MyUploadFile(DungChung.FTP_PATH+DungChung.TL_FTP_PATH, DungChung.USER, DungChung.PASS, fileThanhLyHD);
                            string s = MyUploadFile(DungChung.FTP_PATH + DungChung.TL_FTP_PATH, DungChung.USER, DungChung.PASS, fileThanhLyHD, model.sgtcode, "TL_");

                            model.noidungthanhlyhd = fileName;

                        }
                        else
                        {


                            SetAlert("Định dạng file không cho phép!", "warning");

                            if (Session["ndthanhly"] != null)
                            {
                                model.noidungthanhlyhd = Session["ndthanhly"].ToString();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    if (Session["ndthanhly"] != null)
                    {
                        model.noidungthanhlyhd = Session["ndthanhly"].ToString();
                    }
                }

                if (fileDV != null)
                {
                    if (fileDV.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileDV.FileName);
                        fileName = "DV_" + model.sgtcode + '_' + fileName;
                        string sUploadPath = ConfigurationManager.AppSettings["DVUploadPath"].ToString();
                        var path = Path.Combine(Server.MapPath(sUploadPath), fileName);

                        string sAllowUpFile = ConfigurationManager.AppSettings["AllowUploadFile"].ToString();
                        string sExt = Path.GetExtension(fileDV.FileName);

                        if (sAllowUpFile.Contains(sExt.ToLower()))
                        {

                            //Upload File vao folder ung dung thi uncomment
                            //fileDV.SaveAs(path);

                            //Upload len FTP Server
                            //  string s = MyUploadFile(DungChung.FTP_PATH + DungChung.DV_FTP_PATH, DungChung.USER,DungChung.PASS, fileDV,model.sgtcode,"DV_");
                            string s = MyUploadFile(DungChung.FTP_PATH + DungChung.DV_FTP_PATH, DungChung.USER, DungChung.PASS, fileDV, model.sgtcode, "DV_");

                            model.dichvu = fileName;
                        }
                        else
                        {
                            SetAlert("Định dạng file không cho phép!", "warning");

                            if (Session["nddichvu"] != null)
                            {
                                model.dichvu = Session["nddichvu"].ToString();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    if (Session["nddichvu"] != null)
                    {
                        model.dichvu = Session["nddichvu"].ToString();
                    }
                }

                if (fileChuongTrinhTour != null)
                {
                    if (fileChuongTrinhTour.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileChuongTrinhTour.FileName);
                        fileName = "CTT_" + model.sgtcode + '_' + fileName;
                        string sUploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                        var path = Path.Combine(Server.MapPath(sUploadPath), fileName);

                        string sAllowUpFile = ConfigurationManager.AppSettings["AllowUploadFile"].ToString();
                        string sExt = Path.GetExtension(fileChuongTrinhTour.FileName);

                        if (sAllowUpFile.Contains(sExt.ToLower()))
                        {
                            string s = MyUploadFile(DungChung.FTP_PATH + DungChung.CTTOUR_FTP_PATH, DungChung.USER, DungChung.PASS, fileChuongTrinhTour, model.sgtcode, "CTT_");
                            model.chuongtrinhtour = fileName;

                        }
                        else
                        {
                            SetAlert("Định dạng file không cho phép!", "warning");

                            if (Session["chuongtrinhtour"] != null)
                            {
                                model.chuongtrinhtour = Session["chuongtrinhtour"].ToString();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    if (Session["chuongtrinhtour"] != null)
                    {
                        model.chuongtrinhtour = Session["chuongtrinhtour"].ToString();
                    }
                }
                //DS Khach
                if (fileDSKhach != null)
                {
                    if (fileDSKhach.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileDSKhach.FileName);
                        fileName = "DSK_" + model.sgtcode + '_' + fileName;
                        string sUploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                        var path = Path.Combine(Server.MapPath(sUploadPath), fileName);

                        string sAllowUpFile = ConfigurationManager.AppSettings["AllowUploadFile"].ToString();
                        string sExt = Path.GetExtension(fileDSKhach.FileName);

                        if (sAllowUpFile.Contains(sExt.ToLower()))
                        {
                            string s = MyUploadFile(DungChung.FTP_PATH + DungChung.DSKHACH_FTP_PATH, DungChung.USER, DungChung.PASS, fileDSKhach, model.sgtcode, "DSK_");
                            model.filekhachditour = fileName;

                        }
                        else
                        {
                            SetAlert("Định dạng file không cho phép!", "warning");

                            if (Session["filekhachditour"] != null)
                            {
                                model.filekhachditour = Session["filekhachditour"].ToString();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    if (Session["filekhachditour"] != null)
                    {
                        model.chuongtrinhtour = Session["filekhachditour"].ToString();
                    }
                }

                //Ve may bay
                if (fileVMB != null)
                {
                    if (fileVMB.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(fileVMB.FileName);
                        fileName = "VMB_" + model.sgtcode + '_' + fileName;
                        string sUploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                        var path = Path.Combine(Server.MapPath(sUploadPath), fileName);

                        string sAllowUpFile = ConfigurationManager.AppSettings["AllowUploadFile"].ToString();
                        string sExt = Path.GetExtension(fileVMB.FileName);

                        if (sAllowUpFile.Contains(sExt.ToLower()))
                        {
                            string s = MyUploadFile(DungChung.FTP_PATH + DungChung.VEMAYBAY_FTP_PATH, DungChung.USER, DungChung.PASS, fileVMB, model.sgtcode, "VMB_");
                            model.filevemaybay = fileName;

                        }
                        else
                        {
                            SetAlert("Định dạng file không cho phép!", "warning");

                            if (Session["filevemaybay"] != null)
                            {
                                model.filevemaybay = Session["filevemaybay"].ToString();
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    if (Session["filevemaybay"] != null)
                    {
                        model.filevemaybay = Session["filevemaybay"].ToString();
                    }
                }

                model.nguoisua = Session["username"].ToString();
                model.ngaysua = DateTime.Now;

                //kt co thay doi nam thi khong cho phep
                DateTime dBatDau = (DateTime)Session["ngaybatdau"];//lay gia tri cu
                DateTime d1 = DateTime.Parse(model.batdau.ToString());
                //reset
                Session["ngaybatdau"] = null;

                if (dBatDau.Year != d1.Year)
                {
                    SetAlert("Không được sửa năm của ngày bắt đầu đi tour!", "error");
                }else
                {
                    string id = dao.Update(model);

                    try
                    {

                        tourlogDAO dao1 = new tourlogDAO();
                        tourlog modellog = new tourlog();
                        modellog = DungChung.SetModelGhiLog(modellog, model, Session["username"].ToString(), "Edit");
                        dao1.Insert(modellog);
                    }
                    catch (Exception ex)
                    {

                    }


                    SetAlert("Sửa  Thành Công", "success");
                }                
            }
            catch (Exception ex)
            {
                SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
            }

            //}
            //else
            //{

            //    return View(model);
            //}



            //return RedirectToAction("Index");
            //edit 28082019
            return RedirectToAction("Index", "QLKhachDoan", new RouteValueDictionary(new { searchString = model.sgtcode, ngayditourb = Request["ngayditourb"], ngayditoure = Request["ngayditoure"] }));

        }

        #region "FTP"

        /// <summary>
        /// dinh dang lai chuoi ftp:
        /// ftp:/192.168.3.43 ->  ftp://192.168.3.43
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string CorrectFTPPath(string path)
        {
            int iIndex = path.IndexOf('/');

            if (iIndex > -1)
            {
                path = path.Substring(iIndex + 1);

                iIndex = path.IndexOf('/');

                if (iIndex == 0)
                {
                    path = path.TrimStart('/');
                }

                path = "ftp://" + path;
            }
            return path;
        }
        private string MyUploadFile(string sFTP, string sUsr, string sPwd, HttpPostedFileBase file, string sgtcode, string foldername)
        {
            //Read the FileName and convert it to Byte array.
            string fileName = Path.GetFileName(file.FileName);
            fileName = foldername + sgtcode + '_' + fileName;
            int BUFFER_SIZE = 8 * 1024;
            var sourceStream = file.InputStream;
            byte[] buffer = new byte[BUFFER_SIZE];
            int bytesRead = sourceStream.Read(buffer, 0, BUFFER_SIZE);

            try
            {
                // Get the object used to communicate with the server.
                // FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sFTP + "/" + file.FileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sFTP + "/" + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(sUsr, sPwd);
                request.ContentLength = sourceStream.Length;
                request.UsePassive = true;
                request.UseBinary = true;
                //request.ServicePoint.ConnectionLimit = sourceStream.Length;
                request.EnableSsl = false;
                request.KeepAlive = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    do
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                        bytesRead = sourceStream.Read(buffer, 0, BUFFER_SIZE);
                    } while (bytesRead > 0);

                    //  requestStream.Write(fileBytes, 0, fileBytes.Length);
                    sourceStream.Close();
                    requestStream.Close();
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    string sDes = response.StatusDescription;
                    response.Close();
                    return sDes;
                }

            }
            catch (WebException we)
            {
                sourceStream.Close();
                //return we.Message;
                return we.Message;
            }

        }

        private long GetFTPFileSize(string fullftpfilepath, string user, string pwd)
        {
            long size = 0;

            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(fullftpfilepath));
                request.Proxy = null;
                request.Credentials = new NetworkCredential(user, pwd);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                size = response.ContentLength;
                response.Close();
            }
            catch
            {

            }


            return size;
        }

        /// <summary>
        /// lay file tu ftp va view online/download
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult ShowSoftwareNewJsTreeFTP(string path)
        {
            string strFileName = "";
            //For test
            //Session["userid"] = mUsrTest;
            //Session["pass"] = mPwd;
            path = path.Replace("aabbccddee", ":");
            path = path.Replace("asdfghjkl", "/");
            path = path.Replace("aqwsedrf", "+");
            path = HttpUtility.UrlDecode(path);
            int iIndex = path.IndexOf('/');

            if (iIndex > -1)
            {
                path = path.Substring(iIndex + 1);

                iIndex = path.IndexOf('/');

                if (iIndex == 0)
                {
                    path = path.TrimStart('/');
                }

                int iLastIndex = path.LastIndexOf('/');
                if (iLastIndex > -1)
                {
                    strFileName = path.Substring(iLastIndex + 1);
                }


                path = "ftp://" + path;
            }


            //http://stackoverflow.com/questions/1176022/unknown-file-type-mime
            //return base.File(ftpPath, "application/octet-stream");

            //LARGE FILE              
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(DungChung.USER, DungChung.PASS);
                FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();

                //LARGE FILE

                // **************************************************
                Response.Buffer = false;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName);

                // **************************************************
                // 8KB

                //---------------------------------------------------
                long lngFileLength = GetFTPFileSize(path, DungChung.USER, DungChung.PASS);
                Response.AddHeader("Content-Length", lngFileLength.ToString());

                using (Stream responseStream = ftpResponse.GetResponseStream())
                {
                    int intBufferSize = 8 * 1024;

                    // Create buffer for reading [intBufferSize] bytes from file
                    byte[] bytBuffers = new System.Byte[intBufferSize];

                    // Total bytes that should be read
                    long lngDataToRead = lngFileLength;

                    // Read the bytes of file
                    while (lngDataToRead > 0)
                    {
                        // Verify that the client is connected or not?
                        if (Response.IsClientConnected)
                        {
                            // Read the data and put it in the buffer.
                            int intTheBytesThatReallyHasBeenReadFromTheStream =
                                responseStream.Read(buffer: bytBuffers, offset: 0, count: intBufferSize);

                            // Write the data from buffer to the current output stream.
                            Response.OutputStream.Write(buffer: bytBuffers, offset: 0, count: intTheBytesThatReallyHasBeenReadFromTheStream);

                            // Flush (Send) the data to output
                            // (Don't buffer in server's RAM!)
                            Response.Flush();

                            lngDataToRead =
                                lngDataToRead - intTheBytesThatReallyHasBeenReadFromTheStream;
                        }
                        else
                        {
                            // Prevent infinite loop if user disconnected!
                            lngDataToRead = -1;
                        }
                    }

                    if (responseStream != null)
                    {
                        //Close the file.
                        responseStream.Close();
                        responseStream.Dispose();
                    }
                }

                //END LARGER FILE



            }
            catch (WebException we)
            {
                FtpWebResponse response = (FtpWebResponse)we.Response;
                return Content("Lỗi: " + response.StatusDescription);
            }
            catch (Exception ex)
            {
                return Content("Lỗi không đọc được file: " + ex.Message + " !");
            }
            finally
            {
                //Response.Close();
                //nen dung
                this.HttpContext.ApplicationInstance.CompleteRequest();
            }
            //END LARGER FILE                                  

            return View();

        }
    //}

    #endregion
    public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new tourDAO();
            tour model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        // GET: quanly/dmxes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new tourDAO();

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index");
        }
        public ActionResult SetHuyTour(int id, string nguyennhanhuytour)
        {
            var session = Session["username"];
            string result = "Có lỗi! Hủy không được!";
            var dao = new tourDAO();
            tour model = dao.Details(id);
            if (model == null)
            {
                result = "Tour không tồn tại!";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            model.trangthai = "4";//huy tour
            model.nguyennhanhuythau = nguyennhanhuytour;
            model.ngayhuytour = DateTime.Now;
            try
            {
                string idd = dao.Update(model);


                try
                {

                    tourlogDAO dao1 = new tourlogDAO();
                    tourlog modellog = new tourlog();
                    modellog = DungChung.SetModelGhiLog(modellog, model, Session["username"].ToString(), "HuyTour");
                    dao1.Insert(modellog);
                }
                catch (Exception ex)
                {

                }

                result = "Đã hủy tour!";

            }
            catch (Exception ex)
            {
                result = "Có lỗi! Hủy không được!" + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }
        public ActionResult SetTrangThai(int id, string trangthai)
        {
            var session = Session["username"];

            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var dao = new tourDAO();
            tour model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            model.trangthai = trangthai;

            try
            {
                string idd = dao.Update(model);

                SetAlert("Sửa  Thành Công", "success");

            }
            catch (Exception ex)
            {
                SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
            }

            return RedirectToAction("Index");

        }

        #region "ROOM"

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

            item3 = new SelectListItem();
            item3.Text = "Double room + Extra bed";
            item3.Value = "Double room + Extra bed";
            lst.Add(item3);

            item3 = new SelectListItem();
            item3.Text = "Twin room + Extra bed";
            item3.Value = "Twin room + Extra bed";
            lst.Add(item3);

            return lst;
        }

        public ActionResult RoomIndex(decimal id)
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

        public ActionResult RoomDelete(decimal id)
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
                foreach (roominglistd l in lst)
                {
                    try
                    {
                        daoD.Delete(l.id_roomlistd);
                    }
                    catch (Exception ex)
                    {
                        res = ex.Message;
                        bDOK = false;
                    }
                }

                if (bDOK)
                {
                    res = dao.Delete(id);
                }

            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index", "QLKhachDoan");
        }



        static public void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }



        [HttpGet]
        public ActionResult ExportRoom(decimal id)
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

        private void AddImage(ExcelWorksheet oSheet, int rowIndex, int colIndex, string imagePath)
        {
            //Bitmap image = new Bitmap(imagePath);
            Image image = Image.FromFile(imagePath);

            ExcelPicture excelImage = null;
            if (image != null)
            {
                excelImage = oSheet.Drawings.AddPicture("Debopam Pal", image);
                //excelImage.From.Column = colIndex;
                //excelImage.From.Row = rowIndex;
                excelImage.SetPosition(rowIndex, 0, colIndex, 0);
                excelImage.SetSize(300, 62);
                // 2x2 px space for better alignment
                excelImage.From.ColumnOff = Pixel2MTU(2);
                excelImage.From.RowOff = Pixel2MTU(2);
            }
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        [HttpGet]
        public ExcelWorksheet FormatWorkSheet(List<vie_roominglist> dt, ExcelWorksheet ewres)
        {
            string sImagePath = Server.MapPath("~/Content/images/saigontourist_banner_small.png");
            try
            {
                //// chen logo
                AddImage(ewres, 0, 0, sImagePath);
                // Tao header

                int iColReport = 8;
                string sTenKS = "", sNgayCheckin = "", sNgayCheckout = "", sTourName = "";
                if (dt.Count > 0)
                {
                    sTenKS = dt[0].tenkhachsan;
                    sTourName = dt[0].chudetour;
                    sNgayCheckin = DateTime.Parse(dt[0].ngaycheckin.ToString()).ToString("dd/MM/yyyy");
                    sNgayCheckout = DateTime.Parse(dt[0].ngaycheckout.ToString()).ToString("dd/MM/yyyy");
                }
                //"ROOMLIST OF TOUR " 
                ewres.Cells[5, 1].Value = "ROOMING LIST  OF TOUR " + sTourName;
                ewres.Cells[5, 1].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
                ewres.Cells[5, 1, 5, 8].Merge = true;
                ewres.Cells[5, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ewres.Cells[6, 1].Value = "HOTEL: " + sTenKS + " Check in:  " + sNgayCheckin + " Check out: " + sNgayCheckout;
                ewres.Cells[6, 1].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
                ewres.Cells[6, 1, 6, 8].Merge = true;
                ewres.Cells[6, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //header data
                ewres.Cells[7, 1].LoadFromText("Order");
                ewres.Cells[7, 2].LoadFromText("FULL NAME");
                ewres.Cells[7, 3].LoadFromText("GENDER");
                ewres.Cells[7, 4].LoadFromText("DOB");
                ewres.Cells[7, 5].LoadFromText("PASSPORT");
                ewres.Cells[7, 6].LoadFromText("DOE");
                ewres.Cells[7, 7].LoadFromText("Room Type");
                ewres.Cells[7, 8].LoadFromText("Note");
                //create header
                // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
                using (var range = ewres.Cells[7, 1, 7, iColReport])
                {
                    //range.Style.WrapText = false;
                    // Canh giữa cho các text
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Set Font cho text  trong Range hiện tại
                    range.Style.Font.SetFromFont(new Font("Arial", 9));
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
                //data
                int iRowIndex = 8, beginMergeRowIndex = 8;
                int tang = 0, iCount = 0;
                string sSoPhongPrev = "";
                //merged cells 1 va 7
                foreach (vie_roominglist v in dt)
                {
                    iCount++;
                    ewres.Cells[iRowIndex, 1].Value = v.sophong;
                    ewres.Cells[iRowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ewres.Cells[iRowIndex, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ewres.Cells[iRowIndex, 7].Value = v.loaiphong;
                    ewres.Cells[iRowIndex, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ewres.Cells[iRowIndex, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    if (iRowIndex > 8 && sSoPhongPrev == v.sophong)
                    {
                        tang++;

                        if (iCount == dt.Count)//neu la dong cuoi thi thuc hien rowspan
                        {
                            var rangeMerge = ewres.Cells[beginMergeRowIndex, 1, beginMergeRowIndex + tang, 1];
                            var rangeMergeLoaiP = ewres.Cells[beginMergeRowIndex, 7, beginMergeRowIndex + tang, 7];
                            rangeMerge.Merge = true;
                            rangeMergeLoaiP.Merge = true;
                            beginMergeRowIndex = iRowIndex;
                            tang = 0;

                            rangeMerge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rangeMerge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rangeMergeLoaiP.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            rangeMergeLoaiP.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }
                    else
                    {
                        var rangeMerge = ewres.Cells[beginMergeRowIndex, 1, beginMergeRowIndex + tang, 1];
                        var rangeMergeLoaiP = ewres.Cells[beginMergeRowIndex, 7, beginMergeRowIndex + tang, 7];
                        rangeMerge.Merge = true;
                        rangeMergeLoaiP.Merge = true;
                        beginMergeRowIndex = iRowIndex;
                        tang = 0;

                        rangeMerge.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rangeMerge.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rangeMergeLoaiP.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rangeMergeLoaiP.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    sSoPhongPrev = v.sophong;
                    ewres.Cells[iRowIndex, 2].Value = v.tenkhach;
                    ewres.Cells[iRowIndex, 3].Value = v.phai == "1" ? "F" : "M";

                    if (v.ngaysinh != null)
                    {
                        ewres.Cells[iRowIndex, 4].Value = DateTime.Parse(v.ngaysinh.ToString()).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ewres.Cells[iRowIndex, 4].Value = "";
                    }

                    ewres.Cells[iRowIndex, 5].Value = v.hochieu;

                    if (v.hieuluchochieu != null)
                    {
                        ewres.Cells[iRowIndex, 6].Value = DateTime.Parse(v.hieuluchochieu.ToString()).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        ewres.Cells[iRowIndex, 6].Value = "";
                    }

                    //ewres.Cells[iRowIndex, 7].Value = v.loaiphong;
                    ewres.Cells[iRowIndex, 8].Value = "";//ghi chu

                    using (var range = ewres.Cells[iRowIndex, 1, iRowIndex, iColReport])
                    {
                        //range.Style.WrapText = false;
                        // Set Font cho text  trong Range hiện tại
                        range.Style.Font.SetFromFont(new Font("Arial", 9));
                        // Set Border
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        // Set màu ch Border
                        range.Style.Border.Left.Color.SetColor(Color.Black);
                        range.Style.Border.Top.Color.SetColor(Color.Black);
                        range.Style.Border.Right.Color.SetColor(Color.Black);
                        range.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }

                    iRowIndex = iRowIndex + 1;
                }


            }
            catch (Exception ex)
            {
                System.GC.Collect();
            }
            finally
            {
                //releaseObject(xlSheet);
                //releaseObject(xlBook);
                //releaseObject(ExcelApp);
            }

            ewres.Cells.AutoFitColumns();


            return ewres;

        }
        public ExcelWorksheet FormatWorkSheetBK(List<vie_roominglist> dt, ExcelWorksheet ew)
        {
            //SO PHONG - TEN KHACH-NGAY SINH - LOAI PHONG - SĐT
            int iColReport = 5;
            string sTenKS = "", sNgayCheckin = "", sNgayCheckout = "";
            if (dt.Count > 0)
            {
                sTenKS = dt[0].tenkhachsan;
                sNgayCheckin = DateTime.Parse(dt[0].ngaycheckin.ToString()).ToString("dd/MM/yyyy");
                sNgayCheckout = DateTime.Parse(dt[0].ngaycheckout.ToString()).ToString("dd/MM/yyyy");
            }

            ExcelWorksheet ewres = ew;
            //Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D3D3D3");

            ewres.Cells[1, 1].Value = "ROOMING LIST CHO KHÁCH SẠN " + sTenKS;
            ewres.Cells[1, 1].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            ewres.Cells[1, 5].Merge = true;
            ewres.Cells[2, 1].Value = "NGÀY CHECK IN:  " + sNgayCheckin + " NGÀY CHECK OUT: " + sNgayCheckout;
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

        public ActionResult DeleteRoom(decimal id)
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
                foreach (roominglistd l in lst)
                {
                    try
                    {
                        daoD.Delete(l.id_roomlistd);
                    }
                    catch (Exception ex)
                    {
                        res = ex.Message;
                        bDOK = false;
                    }
                }

                if (bDOK)
                {
                    res = dao.Delete(id);
                }

            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index", "QLKhachDoan");
        }

        /// <summary>
        /// xoa chi tiet roominglistd
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult XoaKhachRoom(decimal id)
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

        public ActionResult SaveRoomingList(string idtour, string tenks, string checkin, string checkout, roominglistd[] khachs)
        {
            string result = "Có lỗi! RoomingList chưa hoàn thành!";
            if (idtour != null && tenks != null && checkin != null && khachs != null)
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

        public ActionResult EditRoom(int id)
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
        public ActionResult EditRoom(roominglist model)
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

            return RedirectToAction("Index", "QLKhachDoan");
        }

        public ActionResult HienCuaSoThemHoaHong(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            hoahongDAO dao = new hoahongDAO();
            dmhoahong model = new dmhoahong();
            model.salesnm = Session["username"].ToString();
            model.idtour = id;
            return View(model);
        }

        public ActionResult GetDataRoomingForEdit(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            roominglistDAO dao = new roominglistDAO();
            roominglist model = dao.Details(id);

            List<vie_khachNotInRoominglist> lstkhach = dao.ListKhachChuaDuocChon(model.idtour);
            ViewBag.id_dsk = new SelectList(lstkhach, "id_dsk", "tenkhach");
            ViewBag.loaiphong = new SelectList(ListLoaiPhong(), "Value", "Text");
            ViewBag.id_roominglist = id;
            return View(model);
        }

        public ActionResult XoaHoaHong(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new hoahongDAO();
            dmhoahong model = dao.Details(id);
            try
            {
                string res = dao.Delete(id);


            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            //return RedirectToAction("Index");
            return RedirectToRoute("Default", new { controller = "QLKhachDoan", action = "LayDmHoaHong", id = model.idtour });
        }

        public ActionResult AddHoaHong(decimal id, dmhoahong[] khachs)
        {
            string result = "Hoa hồng đã thêm thành công! ";
            try
            {
                decimal idtour = id;
                hoahongDAO dao = new hoahongDAO();

                foreach (var item in khachs)
                {
                    dmhoahong m = new dmhoahong();
                    m.idtour = idtour;
                    m.salesnm = item.salesnm;
                    //  m.id_dsk = item.id_dsk;
                    m.tenkhach = item.tenkhach;
                    m.socmnd = item.socmnd;
                    m.sotien = item.sotien;
                    dao.Insert(m);
                }
            }
            catch (Exception ex)
            {
                result = "Có lỗi! Thông tin hoa hồng chưa hoàn thành!" + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditHoaHong(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new hoahongDAO();
            dmhoahong model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditHoaHong(dmhoahong model)
        {
            var dao = new hoahongDAO();

            try
            {
                string sUsrId = Session["userId"].ToString();


                string id = dao.Update(model);

                if (id != null)
                {
                    SetAlert("Thêm Thành Công", "success");
                    //return RedirectToAction("LayDmHoaHong", "QLKhachDoan");
                    return RedirectToRoute("Default", new { controller = "QLKhachDoan", action = "LayDmHoaHong", id = model.idtour });
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

            return View();
        }

        /// <summary>
        /// truyen idtour de tro ve trang LayDmHoaHong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CancelHoaHong(decimal id)
        {
            return RedirectToRoute("Default", new { controller = "QLKhachDoan", action = "LayDmHoaHong", id = id });
        }


        public ActionResult AddKhach(decimal id, roominglistd[] khachs)
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
            }
            catch (Exception ex)
            {
                result = "Có lỗi! RoomingList chưa hoàn thành!" + ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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

        public ActionResult ImportDmKhach(decimal id, HttpPostedFileBase uploadExcel)
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
                            message = ThemKhachExcel(id, dt);

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

        public UploadMessage ThemKhachExcel(decimal idtour, DataTable dt)
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
        #endregion


    }
}