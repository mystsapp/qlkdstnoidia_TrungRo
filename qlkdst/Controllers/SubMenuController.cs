using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;
using System.Linq;

namespace qlkdst.Controllers
{
    public class SubMenuController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, string Role, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new submenuDao();

            if (Role == null || Role == "")
            {
                Role = "";
            }

            ViewBag.Role = new SelectList(db.roles, "roleName", "roleName");

            var model = dao.ListAllPageList(searchString, Role, page, pagesize);
            ViewBag.RoleSelected = Role;
            ViewBag.searchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.areaid = new SelectList(db.mainmenu, "areaid", "areaname");
            ViewBag.Role = new SelectList(db.roles, "roleName", "roleName");
            submenu model = new submenu();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(submenu model)

        {

            var session = Session["username"];
            model.nguoitao = Session["userId"].ToString();
            model.ngaytao = DateTime.Now;

            var dao = new submenuDao();
            try
            {
                model.show_mk = true;
                string id = dao.Insert(model);

                if (id != null)
                {
                    SetAlert("Thêm Thành Công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");
                    ModelState.AddModelError("", "Tạo mới không thành công");
                    return RedirectToAction("ShowError", "SubMenu");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");
                ModelState.AddModelError("", "Tạo mới không thành công");
                return RedirectToAction("ShowError", "SubMenu");
            }
        }

        public ActionResult Edit(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new submenuDao();
            submenu model = dao.Details(id);
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

            ViewBag.areaid = new SelectList(db.mainmenu, "areaid", "areaname", model.areaid);
            ViewBag.Role = new SelectList(db.roles, "roleName", "roleName", model.role);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(submenu model)
        {
            var session = Session["username"];


            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new submenuDao();
                    model.show_mk = true;

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
                    ModelState.AddModelError("", "Sửa thành công");

                }
                catch (Exception ex)
                {
                    SetAlert("Sửa  Không Thành Công: " + ex.Message, "error");
                    ModelState.AddModelError("", "Sửa không thành công");
                    return RedirectToAction("ShowError", "SubMenu");
                }

            }
            else
            {
                ViewBag.areaid = new SelectList(db.mainmenu, "areaid", "areaname", model.areaid);
                ViewBag.Role = new SelectList(db.roles, "roleName", "roleName", model.role);
                return View(model);//de hien len loi
            }

            ViewBag.areaid = new SelectList(db.mainmenu, "areaid", "areaname", model.areaid);
            ViewBag.Role = new SelectList(db.roles, "roleName", "roleName", model.role);

            return RedirectToAction("Index");

        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new submenuDao();
            submenu model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult SetShowMk(int id, bool showmk)
        {
            var session = Session["username"];


            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new submenuDao();
            submenu model = dao.Details(id);
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
                return RedirectToAction("ShowError", "SubMenu");
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
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new submenuDao();
            submenu model = dao.Details(id);

            if (model == null)
            {
                ModelState.AddModelError("", "không tồn tại");
                return RedirectToAction("ShowError", "SubMenu");
            }

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
                ModelState.AddModelError("", "Xóa không thành công");
                return RedirectToAction("ShowError", "SubMenu");
            }

            return RedirectToAction("Index");

        }
    }
}