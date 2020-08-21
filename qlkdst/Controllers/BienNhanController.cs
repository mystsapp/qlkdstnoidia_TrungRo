using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace qlkdst.Controllers
{
    public class BienNhanController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();
        // GET: BienNhan
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            return View();
        }

        public ActionResult BienNhanIndex(decimal id/*,string searchString*/,  int page = 1, int pagesize = 10)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            biennhanDAO dao = new biennhanDAO();
            var model = dao.ListAllPageList(id,"", page, pagesize);
            ViewBag.searchString = "";
           ViewBag.idtour = id;
            return View(model);
        }


        /// <summary>
        /// phat sinh so bien nha, dinh dang:   STT ( 6 so) + "BO" + NamHienTai
        /// BO: Outbout, BN: Noi dia
        /// ndnc: cho biet la noi dua hay nuoc ngoai
        /// </summary>
        /// <returns></returns>
        private string TaoSoBN(string ndnc)
        {

            biennhanDAO dao = new biennhanDAO();
            string sSoThuTuBNHienTai = dao.GetStrSoBienNhan();//lay so thu tu bien  nhan hien tai
            string sSoBienNhan = "";
            if (sSoThuTuBNHienTai == "")
            {
                if (ndnc == "ob")
                {
                    sSoBienNhan = "000001" + "OBD" + DateTime.Now.Year.ToString();
                }else
                {
                    sSoBienNhan = "000001" + "NBD" + DateTime.Now.Year.ToString();
                }
                
            }else
            {
                //cong 1 cho so bien nhan hien tai
                int iSBN = int.Parse(sSoThuTuBNHienTai) +1;
                string sSTT = "";
                switch (iSBN.ToString().Length)
                {
                    case 5:
                        sSTT = "0" + iSBN.ToString();
                        break;
                    case 4:
                        sSTT = "00" + iSBN.ToString();
                        break;
                    case 3:
                        sSTT = "000" + iSBN.ToString();
                        break;
                    case 2:
                        sSTT = "0000" + iSBN.ToString();
                        break;
                    case 1:
                        sSTT = "00000" + iSBN.ToString();
                        break;

                        //case 5:
                        //    sSTT = "0" + iSBN.ToString();
                        //    break;
                        //case 4:
                        //    sSTT = "0" + iSBN.ToString();
                        //    break;
                        //case 3:
                        //    sSTT = "00" + iSBN.ToString();
                        //    break;
                        //case 2:
                        //    sSTT = "000" + iSBN.ToString();
                        //    break;
                        //case 1:
                        //    sSTT = "0000" + iSBN.ToString();
                        //    break;
                }

                if (ndnc == "ob")
                {
                    sSoBienNhan = sSTT + "BOD" + DateTime.Now.Year.ToString();
                }
                else
                {
                    sSoBienNhan = sSTT + "NBD" + DateTime.Now.Year.ToString();
                }

            }

            return sSoBienNhan;
        }

        public ActionResult Insert(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            ViewBag.idtour = id;
            datcoc model = new datcoc();
            model.sobiennhan = TaoSoBN("nd");
            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["username"].ToString();
            //model.nguoilambn = Session["username"].ToString();
            model.nguoilambn = Session["hoten"].ToString();//30052019 lay luon ho ten 
            if (Session["daily"] != null)
            {
                model.daily = Session["daily"].ToString();
            }

            tourDAO daotour = new tourDAO();

            tour t = daotour.Details(id);

            string sSgtCode = "",sDiaChiKh="";
            if (t != null)
            {
                sSgtCode = t.sgtcode;
                sDiaChiKh = t.diachi;
            }
            model.diachi = sDiaChiKh;

            ViewBag.sgtcode = sSgtCode;
            //ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily");
            return View(model);
        }

        /// <summary>
        /// sua thong tin bien nhan, id: id bien nhan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            biennhanDAO dao = new biennhanDAO();
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            datcoc model = new datcoc();
            model = dao.Details(id);
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();
            //model.nguoilambn = Session["username"].ToString();
            model.nguoilambn = Session["hoten"].ToString();//30052019 lay luon ho ten 
            Session["nguoitaobn"] = model.nguoitao;
            Session["nguoilambn"] = model.nguoilambn;
            Session["ngaytao"] = model.ngaytao;
            Session["dailydatcoc"] = model.daily;
            tourDAO daotour = new tourDAO();

            tour t = daotour.Details((decimal)model.idtour);

            string sSgtCode = "";
            if (t != null) sSgtCode = t.sgtcode;

            ViewBag.sgtcode = sSgtCode;
            //ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily",model.daily);
            return View(model);
        }

        /// <summary>
        /// Sua thong tin bien nhan, id: id  bien nhan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ngaybn"></param>
        /// <param name="txsobn"></param>
        /// <param name="txnguoibn"></param>
        /// <param name="daily"></param>
        /// <param name="tenkhach"></param>
        /// <param name="diachi"></param>
        /// <param name="dienthoai"></param>
        /// <param name="noidung"></param>
        /// <param name="sotien"></param>
        /// <param name="htthanhtoan"></param>
        /// <param name="chungtu"></param>
        /// <returns></returns>
        public ActionResult SuaBienNhan(decimal id,decimal idk, string ngaybn, string txsobn, string txnguoibn, string daily, string tenkhach, string diachi, string dienthoai, string noidung, decimal? sotien, string htthanhtoan, string chungtu, string loaitien, decimal? tygia)
        {
            biennhanDAO dao = new biennhanDAO();
            datcoc model = new datcoc();
            model.idtour = id;
            model.iddatcoc = idk;

            if (ngaybn != null)
            {
                try
                {
                    model.ngaydatcoc = DateTime.Parse(ngaybn);
                }
                catch
                {

                }

            }
            model.sobiennhan = txsobn;
            model.nguoilambn = txnguoibn;
            model.daily = daily;
            model.tenkhach = tenkhach;
            model.diachi = diachi;
            model.dienthoai = dienthoai;
            model.sotien = sotien;
            model.hinhthucthanhtoan = htthanhtoan;
            model.chungtugoc = chungtu;
            model.noidung = noidung;
            model.loaitien = loaitien;
            model.tygia = tygia; 

            if (Session["ngaytao"] != null)
            {
                try
                {
                    model.ngaytao = DateTime.Parse(Session["ngaytao"].ToString());
                }
                catch
                {

                }

            }

            if (Session["nguoilambn"] != null)
            {
                try
                {
                    model.nguoilambn = Session["nguoilambn"].ToString();
                }
                catch
                {

                }

            }
            //if (Session["dailydatcoc"] != null)
            //{
            //    try
            //    {
            //        model.daily = Session["dailydatcoc"].ToString();
            //    }
            //    catch
            //    {

            //    }

            //}           

            model.nguoitao = Session["nguoitaobn"].ToString();
            model.nguoisua = Session["userId"].ToString();
            model.ngaysua = DateTime.Now;

            string IP = Request.UserHostName;
            string compName = DetermineCompName(IP);

            model.tenmay = compName;

            try
            {
                string s = dao.Update(model);


                datcoclog log = new datcoclog();
                log = DungChung.SetBienNhanLogModel(log, model, Session["username"].ToString(), "SuaBienNhan");
                dao.InsertLog(log);


                if (s != "")
                {
                    SetAlert("Thêm Thành Công", "success");
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");
            }
            return RedirectToAction("Index", "QLKhachDoan");
        }

        public   string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }

        /// <summary>
        /// them datcoc , id: idtour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateInput(true)]
        //[ValidateAntiForgeryToken]
        public ActionResult ThemBienNhan(decimal id,string ngaybn,string txsobn,string txnguoibn,string daily,string tenkhach,string diachi, string dienthoai,string noidung,decimal? sotien, string htthanhtoan,string chungtu,string loaitien,decimal? tygia)
        {
            biennhanDAO dao = new biennhanDAO();
            datcoc model = new datcoc();
            model.idtour = id;

            if (ngaybn != null)
            {
                try
                {
                    model.ngaydatcoc = DateTime.Parse(ngaybn);
                }
                catch    {
                   
                }

            }
            model.sobiennhan = txsobn;
            model.nguoilambn = txnguoibn;
            model.daily = daily;
            model.tenkhach = tenkhach;
            model.diachi = diachi;
            model.dienthoai = dienthoai;
            model.sotien = sotien;
            model.hinhthucthanhtoan = htthanhtoan;
            model.chungtugoc = chungtu;
            model.noidung = noidung;
            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["userId"].ToString();
            model.loaitien = loaitien;
            model.tygia = tygia;
           
            string IP = Request.UserHostName;
            string compName = DetermineCompName(IP);

            model.tenmay = compName;

            try
            {
                string s = dao.Insert(model);

                datcoclog log = new datcoclog();
                log=DungChung.SetBienNhanLogModel(log, model, Session["username"].ToString(), "ThemBienNhan");
                dao.InsertLog(log);

                if (s !="")
                {
                    SetAlert("Thêm Thành Công", "success");                   
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");                    
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");              
            }
            return RedirectToAction("Index", "QLKhachDoan");
        }

        public ActionResult Xoa(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new biennhanDAO();

            try
            {

                datcoc model = dao.Details(id);
                string res = dao.Delete(id);                

                datcoclog log = new datcoclog();
                log = DungChung.SetBienNhanLogModel(log, model, Session["username"].ToString(), "XoaBienNhan");
                dao.InsertLog(log);

            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index","QLKhachDoan");
        }
    }
}