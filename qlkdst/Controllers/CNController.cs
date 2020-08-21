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
    public class CNController : BaseController
    {
        // GET: CN
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new chinhanhDAO();

            var model = dao.ListAllPageList(searchString, page, pagesize);
            ViewBag.searchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            chinhanh model = new chinhanh();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;
            model.trangthai = true;


            ViewBag.idkhucn =new SelectList(db.phankhucn,"idkhucn","tenkhucn");
            return View(model);
        }


        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(chinhanh model)
        {

            var session = Session["username"];

            var dao = new chinhanhDAO();
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
            ViewBag.idkhucn = new SelectList(db.phankhucn, "idkhucn", "tenkhucn");
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new chinhanhDAO();
            chinhanh model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            Session["nguoitao"] = model.nguoitao;
            Session["ngaytao"] = model.ngaytao;


            if (model.nguoitao != null)
            {
                string sUserNm = db.users.Where(x => x.userId == model.nguoitao).FirstOrDefault().username;
                model.nguoitao = sUserNm;
            }
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();
            ViewBag.idkhucn = new SelectList(db.phankhucn, "idkhucn", "tenkhucn", model.idkhucn);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(chinhanh model)
        {
            var session = Session["username"];
            ViewBag.idkhucn = new SelectList(db.phankhucn, "idkhucn", "tenkhucn", model.idkhucn);

            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new chinhanhDAO();

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
            var dao = new chinhanhDAO();
            chinhanh model = dao.Details(id);

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
            var dao = new chinhanhDAO();
            chinhanh model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                model.trangthai =!model.trangthai;
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
            var dao = new chinhanhDAO();
            chinhanh model = dao.Details(id);

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