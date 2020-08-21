using qlkdst.Common;
using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class NCController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString,int idkhus=0, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new nuocDAO();
        
            var model = dao.ListAllPageList(searchString, idkhus, page, pagesize);
            ViewBag.searchString = searchString;
            ViewBag.idkhus = new SelectList(db.khuvuc, "idkhu", "tenkhu");
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            nuoc model = new nuoc();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;
            ViewBag.idkhu = new SelectList(db.khuvuc, "idkhu", "tenkhu");
            ViewBag.phamvi = new SelectList(DungChung.ListPhamViTuyen(), "Value", "Text");
            return View(model);
        }

      
        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(nuoc model)
        {

            var session = Session["username"];
            ViewBag.idkhu = new SelectList(db.khuvuc, "idkhu", "tenkhu");
            ViewBag.phamvi = new SelectList(DungChung.ListPhamViTuyen(), "Value", "Text");
            var dao = new nuocDAO();
            try
            {
              
                model.nguoitao = Session["userId"].ToString();
                model.ngaytao = DateTime.Now;

                //kiem tra trung ten truoc khi them moi
                nuoc n = dao.DetailsByName(model.TenNuoc);

                if (n != null)
                {
                    SetAlert("Đã tồn tại quốc gia này!", "error");

                }else
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
                        //return RedirectToAction("ShowError", "NC");
                    }
                }

               

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");                
                //return RedirectToAction("ShowError", "NC");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new nuocDAO();
            nuoc model = dao.Details(id);
            
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.idkhu = new SelectList(db.khuvuc, "idkhu", "tenkhu",model.idkhu);
            Session["nguoitao"] = model.nguoitao;
            Session["ngaytao"] = model.ngaytao;


            if (model.nguoitao != null)
            {
                string sUserNm = db.users.Where(x => x.userId == model.nguoitao).FirstOrDefault().username;
                model.nguoitao = sUserNm;
            }
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();
            ViewBag.phamvi =new SelectList(DungChung.ListPhamViTuyen(),"Value", "Text",model.phamvi);
            return View(model);
        }

        //public List<SelectListItem> ListPhamViTuyen()
        //{
        //    List<SelectListItem> lst = new List<SelectListItem>();
        //    SelectListItem item = new SelectListItem();
        //    item.Text = "Tuyến gần";
        //    item.Value = "1";
        //    lst.Add(item);

        //    item = new SelectListItem();
        //    item.Text = "Tuyến xa";
        //    item.Value = "2";

        //    lst.Add(item);   

        //    return lst;
        //}

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(nuoc model)
        {
            var session = Session["username"];
            ViewBag.idkhu = new SelectList(db.khuvuc, "idkhu", "tenkhu", model.idkhu);
            ViewBag.phamvi = new SelectList(DungChung.ListPhamViTuyen(), "Value", "Text", model.phamvi);
            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new nuocDAO();

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

                    //kiem tra trung ten truoc khi them moi
                    bool b = dao.KTNuocTheoTen(model.TenNuoc, model.Id);

                    if (b)
                    {
                        SetAlert("Đã tồn tại quốc gia này!", "error");

                    }
                    else
                    {
                        string id = dao.Update(model);
                        SetAlert("Sửa  Thành Công", "success");
                    }


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

        public ActionResult Details(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new nuocDAO();
            nuoc model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult SetShowMk(decimal id, bool showmk)
        {
            var session = Session["username"];


            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new nuocDAO();
            nuoc model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                //model.show_mk = showmk;
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
        public ActionResult Delete(decimal id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new nuocDAO();
            nuoc model = dao.Details(id);

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