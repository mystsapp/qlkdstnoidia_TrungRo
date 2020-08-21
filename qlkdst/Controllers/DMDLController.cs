using qlkdst.Common;
using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class DMDLController : BaseController
    {
        // GET: DMDL
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new dmdailyDAO();

            var model = dao.ListAllPageList(searchString, page, pagesize);
            ViewBag.searchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            dmdaily model = new dmdaily();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;
            model.trangthai = true;
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn");
            return View(model);
        }


        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(dmdaily model)
        {

            var session = Session["username"];
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn");
            var dao = new dmdailyDAO();
            try
            {

                model.nguoitao = Session["userId"].ToString();
                model.ngaytao = DateTime.Now;
                model.trangthai = true;
                string id = dao.Insert(model);

                if (id != null)
                {
                    SetAlert("Thêm Thành Công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");
                    //return RedirectToAction("ShowError", "NC");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");
                //return RedirectToAction("ShowError", "NC");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new dmdailyDAO();
            dmdaily model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn",model.chinhanh);
            Session["nguoitao"] = model.nguoitao;
            Session["ngaytao"] = model.ngaytao;

            if (model.nguoitao != null)
            {
                string sUserNm = db.users.Where(x => x.userId == model.nguoitao).FirstOrDefault().username;
                model.nguoitao = sUserNm;
            }
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(dmdaily model)
        {
            var session = Session["username"];
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn", model.chinhanh);
            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new dmdailyDAO();

                    model.ngaysua = DateTime.Now;
                    model.nguoisua = Session["userId"].ToString();

                    if (Session["nguoitao"] != null)
                    {
                        model.nguoitao = Session["nguoitao"].ToString();
                    }

                    if (Session["ngaytao"] != null)
                    {
                        model.ngaytao = DateTime.Parse(Session["ngaytao"].ToString());
                    }

                    string id = dao.Update(model);

                    SetAlert("Sửa  Thành Công", "success");
                }
                catch (Exception ex)
                {
                    SetAlert("Sửa  Không Thành Công: " + ex.Message, "error");
                    //return RedirectToAction("ShowError", "NC");
                }
            }
            else
            {
                return View(model);//de hien len loi
            }


            return RedirectToAction("Index");

        }

        public ActionResult Details(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmdailyDAO();
            dmdaily model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }



        public ActionResult SetShowMk(int id)
        {
            var session = Session["username"];


            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmdailyDAO();
            dmdaily model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                model.trangthai = !model.trangthai;
                string idd = dao.Update(model);

                SetAlert("Sửa  Thành Công", "success");
            }
            catch (Exception ex)
            {
                SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
                //return RedirectToAction("ShowError", "NC");
            }

            return RedirectToAction("Index");

        }

        public ActionResult ShowError()
        {
            return View();
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }


        // GET: quanly/dmxes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmdailyDAO();
            dmdaily model = dao.Details(id);

            if (model == null)
            {
                SetAlert("Không tồn tại dữ liệu này!", "warning");
                //return RedirectToAction("ShowError", "NC");
            }

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
                //return RedirectToAction("ShowError", "NC");
            }

            return RedirectToAction("Index");

        }
    }
}