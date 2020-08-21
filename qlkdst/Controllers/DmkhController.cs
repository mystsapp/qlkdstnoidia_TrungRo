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
    public class DmkhController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, string dlcn, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];

            string sUserId = Session["userId"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            string sRole = Session["RoleName"].ToString();

            var dao = new dmkhachhangDAO();

            ViewBag.Role = new SelectList(db.roles, "roleName", "roleName");

            List<chinhanh> lst = DungChung.LayChiNhanhTheoUser(sUserId);
            ViewBag.dlcn = new SelectList(DungChung.LayDSChiNhanhTheoUser(sUserId), "Value", "Text");


            if (dlcn == null)
            {
                dlcn = "";
            }

            var model = dao.ListAllPageList(searchString, dlcn, sUserId, sRole, sChinhanh, page, pagesize);
            ViewBag.searchString = searchString;
            ViewBag.dlcnSelected = dlcn;
            return View(model);
        }

        /// <summary>
        /// id: tenquocgia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTpTheoQg(string id)
        {
            var dao = new quanDAO();

            if (!string.IsNullOrWhiteSpace(id))
            {
                List<quan> regions = dao.GetQuanTheoTenQG(id);
                return Json(regions, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Create()
        {
            dmkhachhang model = new dmkhachhang();
            model.makh = TaoMakh();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;
            ViewBag.thanhpho = new SelectList(db.quan.OrderBy(x=>x.tenquan), "tenquan", "tenquan");
            ViewBag.quocgia = new SelectList(db.nuoc.OrderBy(x=>x.TenNuoc), "TenNuoc", "TenNuoc");
            ViewBag.nganhnghe = new SelectList(db.dmnghanhnghe, "tennghanhnghe", "tennghanhnghe");
            ViewBag.chinhanh = new SelectList(db.chinhanh.OrderBy(x=>x.tencn), "chinhanh1", "tencn");
            return View(model);
        }

        private string TaoMakh()
        {
            dmkhachhangDAO dao = new dmkhachhangDAO();
            string sSoThuTuBNHienTai = dao.GetStrMakh();

            string sMakh = "";
            if (sSoThuTuBNHienTai == "")
            {
                sMakh = "00001";
            }
            else
            {
                //cong 1 cho so bien nhan hien tai
                int iSBN = int.Parse(sSoThuTuBNHienTai) + 1;
                string sSTT = "";
                switch (iSBN.ToString().Length)
                {
                    case 4:
                        sSTT = "0" + iSBN.ToString();
                        break;
                    case 3:
                        sSTT = "00" + iSBN.ToString();
                        break;
                    case 2:
                        sSTT = "000" + iSBN.ToString();
                        break;
                    case 1:
                        sSTT = "0000" + iSBN.ToString();
                        break;
                }
                sMakh = sSTT;
            }

            return sMakh;
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(dmkhachhang model)
        {

            var session = Session["username"];
          
            ViewBag.thanhpho = new SelectList(db.quan.OrderBy(x => x.tenquan), "tenquan", "tenquan");
            ViewBag.quocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "TenNuoc", "TenNuoc");
            ViewBag.nganhnghe = new SelectList(db.dmnghanhnghe, "tennghanhnghe", "tennghanhnghe");
            ViewBag.chinhanh = new SelectList(db.chinhanh.OrderBy(x => x.tencn), "chinhanh1", "tencn");

            var dao = new dmkhachhangDAO();
            try
            {
                model.makh = TaoMakh();
                model.nguoitao = Session["userId"].ToString();
                model.ngaytao = DateTime.Now;

                if(model.tenthuongmai!=null)
                    model.tenthuongmai = model.tenthuongmai.ToUpper();

                if (model.tengiaodich != null)
                    model.tengiaodich = model.tengiaodich.ToUpper();


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

        public ActionResult Edit(string id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new dmkhachhangDAO();
            dmkhachhang model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.nganhnghe = new SelectList(db.dmnghanhnghe, "tennghanhnghe", "tennghanhnghe", model.nganhnghe);

            Session["nguoitao"] = model.nguoitao;
            Session["ngaytao"] = model.ngaytao;


            if (model.nguoitao != null)
            {
                string sUserNm = db.users.Where(x => x.userId == model.nguoitao).FirstOrDefault().username;
                model.nguoitao = sUserNm;
            }
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();

            ViewBag.thanhpho = new SelectList(db.quan.OrderBy(x=>x.tenquan), "tenquan", "tenquan", model.thanhpho);
            if (model.quocgia != "")
            {
                //lay id quocgia
                nuoc nc = db.nuoc.Where(x => x.TenNuoc.Contains(model.quocgia)).FirstOrDefault();
                if (nc != null)
                {
                    ViewBag.thanhpho = new SelectList(db.quan.Where(x => x.maquocgia == nc.Id).ToList(), "tenquan", "tenquan", model.thanhpho);
                }

            }

            ViewBag.quocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "TenNuoc", "TenNuoc", model.quocgia);
            ViewBag.chinhanh = new SelectList(db.chinhanh.OrderBy(x => x.tencn), "chinhanh1", "tencn", model.chinhanh);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(dmkhachhang model)
        {
            var session = Session["username"];
            ViewBag.nganhnghe = new SelectList(db.dmnghanhnghe, "tennghanhnghe", "tennghanhnghe", model.nganhnghe);
            ViewBag.chinhanh = new SelectList(db.chinhanh.OrderBy(x=>x.tencn), "chinhanh1", "tencn", model.chinhanh);
            ViewBag.thanhpho = new SelectList(db.quan.OrderBy(x => x.tenquan), "tenquan", "tenquan", model.thanhpho);
            ViewBag.quocgia = new SelectList(db.nuoc.OrderBy(x => x.TenNuoc), "TenNuoc", "TenNuoc", model.quocgia);

            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new dmkhachhangDAO();

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

                    if (model.tenthuongmai != null)
                        model.tenthuongmai = model.tenthuongmai.ToUpper();

                    if (model.tengiaodich != null)
                        model.tengiaodich = model.tengiaodich.ToUpper();


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

        public ActionResult Details(string id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmkhachhangDAO();
            dmkhachhang model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult SetShowMk(string id, bool showmk)
        {
            var session = Session["username"];


            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmkhachhangDAO();
            dmkhachhang model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                //model.show_mk = showmk;
                string idd = dao.Update(model);

                SetAlert("Sửa  Thành Công", "success");
                ModelState.AddModelError("", "Sửa thành công");

            }
            catch (Exception ex)
            {
                SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
                ModelState.AddModelError("", "Sửa không thành công");
                return RedirectToAction("ShowError", "Dmkh");
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
        public ActionResult Delete(string id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new dmkhachhangDAO();
            dmkhachhang model = dao.Details(id);

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