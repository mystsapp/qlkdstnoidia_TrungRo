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
    public class TPController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString,decimal maquocgia=0, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new quanDAO();

           
            var model = dao.ListAllPageList(searchString, maquocgia, page, pagesize);
            ViewBag.searchString = searchString;
            ViewBag.maquocgia = new SelectList(db.nuoc, "Id", "TenNuoc");

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            quan model = new quan();
            
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;         
            ViewBag.maquocgia = new SelectList(db.nuoc, "Id", "TenNuoc");
            return View(model);
        }     

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(quan model)
        {

            var session = Session["username"];
            
            ViewBag.maquocgia = new SelectList(db.nuoc, "Id", "TenNuoc");
            var dao = new quanDAO();
            try
            {            
                model.nguoitao = Session["userId"].ToString();
                model.ngaytao = DateTime.Now;
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
                    return RedirectToAction("ShowError", "TP");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");
                ModelState.AddModelError("", "Tạo mới không thành công");
                return RedirectToAction("ShowError", "TP");
            }
        }

        public ActionResult Edit(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new quanDAO();
            quan model = dao.Details(id);
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

            ViewBag.maquocgia = new SelectList(db.nuoc, "Id", "TenNuoc", model.maquocgia);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(quan model)
        {
            var session = Session["username"];
            ViewBag.maquocgia = new SelectList(db.nuoc, "Id", "TenNuoc", model.maquocgia);

            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new quanDAO();

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
                    //return RedirectToAction("ShowError", "TP");
                }
            }
            else
            {
                SetAlert("Có lỗi!", "error");
                //return View(model);//de hien len loi
            }


            return RedirectToAction("Index");

        }

        public ActionResult Details(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new quanDAO();
            quan model = dao.Details(id);

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
        public ActionResult Delete(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new quanDAO();
            quan model = dao.Details(id);

            if (model == null)
            {
                // ModelState.AddModelError("", "không tồn tại");
                // return RedirectToAction("ShowError", "TP");
                SetAlert("Không tồn tại dữ liệu này!", "warning");
            }

            //kiem tra du lieu co su dung chua, neu co bao loi
            dmkhachhang dm = db.dmkhachhang.Where(x => x.thanhpho == model.tenquan).FirstOrDefault();
            if (dm != null)
            {
                SetAlert("Dữ liệu này đã sử dụng nên không xóa được!", "warning");                
               // return RedirectToAction("ShowError", "TP");
            }


            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");                
                //return RedirectToAction("ShowError", "TP");
            }

            return RedirectToAction("Index");

        }
    }
}