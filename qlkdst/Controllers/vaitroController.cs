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
    public class vaitroController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new rolesDAO();
            
            var model = dao.ListAllPageList(searchString, page, pagesize);
            ViewBag.searchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            roles model = new roles();
            return View(model);
        }

       

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(roles model)
        {

            var session = Session["username"];
            var dao = new rolesDAO();
            try
            {
         
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
                    return RedirectToAction("ShowError", "Dmkh");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");
                ModelState.AddModelError("", "Tạo mới không thành công");
                return RedirectToAction("ShowError", "Dmkh");
            }
        }

        public ActionResult Edit(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new rolesDAO();
            roles model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }
        
            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(roles model)
        {
            var session = Session["username"];

            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new rolesDAO();                   

                    string id = dao.Update(model);

                    SetAlert("Sửa  Thành Công", "success");
                    ModelState.AddModelError("", "Sửa thành công");

                }
                catch (Exception ex)
                {
                    SetAlert("Sửa  Không Thành Công: " + ex.Message, "error");
                    ModelState.AddModelError("", "Sửa không thành công");
                    return RedirectToAction("ShowError", "Dmkh");
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
            var dao = new rolesDAO();
            roles model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
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
            var dao = new rolesDAO();
            roles model = dao.Details(id);

            if (model == null)
            {
                ModelState.AddModelError("", "không tồn tại");
                return RedirectToAction("ShowError", "Dmkh");
            }

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
                ModelState.AddModelError("", "Xóa không thành công");
                return RedirectToAction("ShowError", "Dmkh");
            }

            return RedirectToAction("Index");

        }
    }
}