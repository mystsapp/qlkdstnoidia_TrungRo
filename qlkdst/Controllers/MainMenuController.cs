using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class MainMenuController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, int page = 1, int pagesize = 10)
        {
            var dao = new mainmenuDao();
            var model = dao.ListAllPageList(searchString, page, pagesize);
            ViewBag.searchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(mainmenu dm)

        {
            var dao = new mainmenuDao();

            try
            {
                dm.show_mk = true;//mac dinh la hien len
                string id = dao.Insert(dm);

                if (id != null)
                {
                    SetAlert("Thêm Thành Công", "success");
                    return RedirectToAction("Index", "mainmenu");
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");
                    ModelState.AddModelError("", "Tạo mới không thành công");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi:" + ex.Message, "warning");
                ModelState.AddModelError("", "Tạo mới không thành công");
            }


            return View(dm);
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var dao = new mainmenuDao();
            mainmenu model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(mainmenu dm)
        {
            var session = Session["username"];


            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new mainmenuDao();
                    dm.show_mk = true;
                    string id = dao.Update(dm);

                    SetAlert("Sửa  Thành Công", "success");
                    ModelState.AddModelError("", "Sửa thành công");



                }
                catch (Exception ex)
                {
                    SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
                    ModelState.AddModelError("", "Sửa không thành công");
                }

            }
            else
            {
                return View(dm);
            }



            return RedirectToAction("Index");

        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new mainmenuDao();
            mainmenu model = dao.Details(id);

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
            var dao = new mainmenuDao();

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
                ModelState.AddModelError("", "Xóa không thành công");
            }

            return RedirectToAction("Index");
        }

        public ActionResult SetShowMk(int id, bool showmk)
        {
            var session = Session["username"];

            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var dao = new mainmenuDao();
            mainmenu model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                model.show_mk = showmk;
                string idd = dao.Update(model);

                SetAlert("Sửa  Thành Công", "success");
                ModelState.AddModelError("", "Sửa thành công");

            }
            catch (Exception ex)
            {
                SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
                ModelState.AddModelError("", "Sửa không thành công");
            }

            return RedirectToAction("Index");

        }
    }
}