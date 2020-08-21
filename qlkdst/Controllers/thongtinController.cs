using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class thongtinController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();
        // GET: BienNhan
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            return View();
        }

        public ActionResult ThongTinIndex(decimal id/*,string searchString*/, int page = 1, int pagesize = 10)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            thongtinDAO dao = new thongtinDAO();
            var model = dao.ListAllPageList(id,"", page, pagesize);
            ViewBag.searchString = "";
            ViewBag.idtour = id;                      
            

            return View(model);
        }

        public ActionResult Insert(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            ViewBag.idtour = id;
            thongtintour model = new thongtintour();
            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["username"].ToString();
            tourDAO daotour = new tourDAO();

            tour t = daotour.Details(id);

            string sSgtCode = "";
            if (t != null) sSgtCode = t.sgtcode;

            ViewBag.sgtcode = sSgtCode;
            //ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily");
            return View(model);
        }

        /// <summary>
        /// sua thong tin ve, id: id ve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            thongtinDAO dao = new thongtinDAO();
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            thongtintour model = new thongtintour();
            model = dao.Details(id);
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();
            Session["nguoitao"] = model.nguoitao;
            //var usr = db.users.W   //.Where(x => x.userId == model..nguoitao).FirstOrDefault();
            usersDAO usrDAO = new usersDAO();
            var usr = usrDAO.Details(model.nguoitao);
            if (usr != null)
            {
                model.nguoitao = usr.username;
            }
                        
            Session["ngaytao"] = model.ngaytao;
            tourDAO daotour = new tourDAO();

            tour t = daotour.Details((decimal)model.idtour);

            string sSgtCode = "";
            if (t != null) sSgtCode = t.sgtcode;

            ViewBag.sgtcode = sSgtCode;
            // ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily", model.daily);
            return View(model);
        }

        /// <summary>
        /// Sua thong tin bien nhan, id: id  bien nhan
        /// </summary>

        /// <returns></returns>
        public ActionResult SuaTT(decimal id, decimal idk, string noidung)
        {
            thongtinDAO dao = new thongtinDAO();

            if (idk == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            thongtintour model = dao.Details(idk);
          
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

            model.nguoitao = Session["nguoitao"].ToString();
            model.nguoisua = Session["userId"].ToString();
            model.ngaysua = DateTime.Now;
            model.noidungtin = noidung;
             
           

            try
            {
                string s = dao.Update(model);

                if (s != "")
                {
                    SetAlert("Sửa Thành Công", "success");
                }
                else
                {
                    SetAlert("Sửa Không Thành Công", "warning");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Sửa Không Thành Công, lỗi: " + ex.Message, "error");
            }
            return RedirectToAction("Index", "QLKhachDoan");
        }


        /// <summary>
        /// them thongtin , id: idtour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateInput(true)]
        //[ValidateAntiForgeryToken]
        public ActionResult ThemTT(decimal id, string noidung)
        {
            thongtinDAO dao = new thongtinDAO();
            thongtintour model = new thongtintour();
            model.idtour = id;          
            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["userId"].ToString();
            model.noidungtin = noidung;

            try
            {
                string s = dao.Insert(model);

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

            return RedirectToAction("ThongTinIndex", "thongtin",new { id = model.idtour });
        }

        public ActionResult Xoa(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new thongtinDAO();

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index", "QLKhachDoan");
        }
    }
}