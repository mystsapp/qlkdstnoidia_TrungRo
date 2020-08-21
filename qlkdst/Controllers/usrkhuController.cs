using qlkdst.Common;
using qlkdst.Models;
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
    public class usrkhuController : BaseController
    {
        // GET: usrkhu
        qlkdtrEntities db = new qlkdtrEntities();

        public ActionResult Index(string searchString, int page = 1, int pagesize = 10)
        {
            var session = Session["username"];
            var dao = new usrkhucnDAO();

            var model = dao.ListAllPageList(searchString, page, pagesize);
            ViewBag.searchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            usrkhucn model = new usrkhucn();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;
            return View(model);
        }

        #region "Multi checkbox"

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMulti(QuyenCtyViewModel model)
        {
            var dao = new usrkhucnDAO();
            var modelIns = new usrkhucn();

            try
            {
                modelIns.ngaytao = DateTime.Now;
                modelIns.nguoitao = Session["userId"].ToString();

                //duyet cac phan user chon
                foreach (KhuCNViewModel cn in model.listKhuCN)
                {
                    if (cn.Checked)
                    {
                        //kiem tra trung
                        List<usrkhucn> model1 = db.usrkhucn.Where(x => x.idkhucn == cn.idkhucn && x.userId == model.userId).ToList();

                        if (model1.Count > 0)// da co thi bo qua khong them quyen nua
                        {
                            SetAlert("Đã tồn tại quyền này!", "error");
                            ModelState.AddModelError("", "Đã tồn tại quyền này!");
                            //return RedirectToAction("ShowError");

                        }
                        else
                        {
                            modelIns.idkhucn = cn.idkhucn;
                            modelIns.userId = model.userId;
                            string id = dao.Insert(modelIns);


                            if (id != null)
                            {
                                SetAlert("Thêm Thành Công", "success");

                            }
                            else
                            {
                                SetAlert("Thêm Không Thành Công", "warning");
                                ModelState.AddModelError("", "Tạo mới không thành công");
                                return RedirectToAction("ShowError");
                            }
                        }
                    }

                }//end foreach


                string sUserName = Session["username"].ToString();
                string suserId = Session["userId"].ToString();
                List<phankhucn> ds = db.phankhucn.ToList();
                 model.listKhuCN = new List<KhuCNViewModel>();

                foreach (phankhucn c in ds)
                {
                    KhuCNViewModel m = new KhuCNViewModel();
                    m.idkhucn = c.idkhucn;
                    m.tenkhucn = c.tenkhucn;
                    model.listKhuCN.Add(m);
                }
          
                //an user admin
                List<users> lstUsers = new List<users>();
                lstUsers = db.users.Where(x=>x.role != "sales").ToList();//chi phan quyen cho user cap quan ly
                //if (!sUserName.ToLower().Equals("admin"))
                //{
                //    lstUsers = lstUsers.Where(x => x.username != "admin").ToList();
                //}

                ViewBag.userId = new SelectList(lstUsers.OrderBy(x=>x.username), "userId", "username");
                return RedirectToAction("Index", "usrkhu");



            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi:" + ex.InnerException, "warning");
                ModelState.AddModelError("", "Tạo mới không thành công");
                return RedirectToAction("ShowError");
            }
        }

        [HttpGet]
        public ActionResult CreateMulti()
        {

            var model = new QuyenCtyViewModel();
            model.ngaytao = DateTime.Now;

            string sUserName = Session["username"].ToString();
            string sRole = Session["RoleName"].ToString();
            string sChinhanh = Session["chinhanh"].ToString();
            model.nguoitao = sUserName;

            string suserId = Session["userId"].ToString();
            List<phankhucn> ds = new List<phankhucn>();
            ds = db.phankhucn.ToList();
            model.listKhuCN = new List<KhuCNViewModel>();

            foreach (phankhucn c in ds)
            {
                KhuCNViewModel m = new KhuCNViewModel();
                m.idkhucn = c.idkhucn;
                m.tenkhucn = c.tenkhucn;
                model.listKhuCN.Add(m);
            }
            
            //an user admin va chi thay user cung rolenm va cung chi nhanh
            List<users> lstUsers = new List<users>();
            lstUsers = db.users.Where(x => x.role != "sales").ToList();//chi phan quyen cho user cap quan ly          

            //chi hien user dang su dung
            lstUsers = lstUsers.Where(x => x.trangthai == true).ToList();

            ViewBag.userId = new SelectList(lstUsers.OrderBy(x => x.username), "userId", "username");
            return View(model);
        }

        #endregion

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(usrkhucn model)
        {

            var session = Session["username"];

            var dao = new usrkhucnDAO();
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

            var dao = new usrkhucnDAO();
            usrkhucn model = dao.Details(id);
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


            //an user admin
            List<users> lstUsers = new List<users>();
            lstUsers = db.users.Where(x => x.role != "sales").ToList();//chi phan quyen cho user cap quan ly

            //string sUserName = Session["username"].ToString();

            //if (!sUserName.ToLower().Equals("admin"))
            //{
            //    lstUsers = lstUsers.Where(x => x.username != "admin").ToList();
            //}

            ViewBag.userId = new SelectList(lstUsers.OrderBy(x => x.username), "userId", "username", model.userId);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(usrkhucn model)
        {
            var session = Session["username"];

            //an user admin
            List<users> lstUsers = new List<users>();
            lstUsers = db.users.Where(x => x.role != "sales").ToList();//chi phan quyen cho user cap quan ly

            //string sUserName = Session["username"].ToString();

            //if (!sUserName.ToLower().Equals("admin"))
            //{
            //    lstUsers = lstUsers.Where(x => x.username != "admin").ToList();
            //}

            ViewBag.userId = new SelectList(lstUsers.OrderBy(x => x.username), "userId", "username", model.userId);

            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new usrkhucnDAO();

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

                    //kiem tra trung
                    List<usrkhucn> model1 = db.usrkhucn.Where(x => x.idkhucn == model.idkhucn && x.userId == model.userId).ToList();

                    if (model1.Count > 0)// da co thi bo qua khong them quyen nua
                    {
                        SetAlert("Đã tồn tại quyền này!", "error");
                        ModelState.AddModelError("", "Đã tồn tại quyền này!");                      
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



            ViewBag.idkhucn = new SelectList(db.phankhucn, "idkhucn", "tenkhucn", model.idkhucn);
            return RedirectToAction("Index");

        }

        public ActionResult Details(int id)
        {
            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new usrkhucnDAO();
            usrkhucn model = dao.Details(id);

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
            var dao = new usrkhucnDAO();
            usrkhucn model = dao.Details(id);

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