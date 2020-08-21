using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            qlkdtrEntities db = new qlkdtrEntities();
            if (Session["username"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            tourDAO dao = new tourDAO();            
            //ViewBag.dstour = db.tour.Where(x => x.daily == sDaily);//,"idtour","sgtcode");
            string sDaily = "", sChiNhanh = "";
            users usr = new users();
            if (Session["USER_SESSION"] != null)
            {
                usr = (users)Session["USER_SESSION"];
                sDaily = usr.daily;
                sChiNhanh = usr.chinhanh;
            }
            ViewBag.dstour = dao.GetDsTour("","","","", sDaily, usr);
            return View();
        }        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult LayDSTT(decimal id)
        {
            var dao = new thongtinDAO();

            try
            {
                List<vie_tourvathongtin> tt = dao.LayDSTT(id);
                return Json(tt, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
            
        }

    }
}