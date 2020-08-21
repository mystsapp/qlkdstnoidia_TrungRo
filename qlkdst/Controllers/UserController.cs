using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using qlkdst.Models;

namespace qlkdst.Controllers
{
    public class UserController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();
        // GET: quanly/user
        public ActionResult Index(string searchString, int page = 1, int pagesize = 10)
        {
            var dao = new usersDAO();

            //an user admin di
            string sUserId = Session["userId"].ToString();
            string sUserNm = Session["username"].ToString();
            string sRoleName = Session["RoleName"].ToString();

            ViewBag.rolename = sRoleName;

            var cq = dao.ListAllPageList(searchString, page, pagesize, sUserId, sRoleName);
            ViewBag.searchString = searchString;
            return View(cq);
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }

        public ActionResult ChangePass(string id)
        {
            id = id.Replace('~', '.');
            UserUpdPass model = new UserUpdPass();
            model.username = id;
            return View(model);
        }
        public void Logout()
        {
            Session["username"] = null;
            Session["userId"] = null;
            Session["hoten"] = null;
            Session["loainguoidung"] = null;
            Session["chinhanh"] = null;
            Session["USER_SESSION"] = null;
            Session["MenuMaster"] = null;
            Session["RoleName"] = null;
            Session["daily"] = null;            
        }

        [HttpPost]
        public ActionResult ChangePass(UserUpdPass model)
        {           
            //kiem tra co ton tai user va pass nay           
            var userDao = new usersDAO();
            var result = userDao.login(model.username, model.oldpassword, "");
            if (result == 0)
            {
                SetAlert("Tài khoản này không tồn tại!", "warning");
                return View();
            }
            else if (result == -2)
            {
                SetAlert("Mật khẩu không đúng.", "warning");
                return View();
            }
            else if (result == 1)
            {
                if (model.newpassword == null)
                {
                    SetAlert("Mật khẩu mới không được để trống.", "warning");
                    return View();
                }
                else
                {
                    if (model.newpassword != model.repeatnewpassword)
                    {
                        SetAlert("Mật khẩu mới không giống nhau.", "warning");
                        return View();
                    }
                    else
                    {
                        users model1 = userDao.DetailsByUsrName(model.username);
                        model1.password = model.newpassword;
                        string sResult = userDao.UpdatePass(model1);
                        SetAlert("Đổi mật khẩu thành công!", "warning");
                        //logout truoc
                        Logout();

                    }
                }

              
            }
            return RedirectToAction("Index","Login");
        }


        //public ActionResult ThongTinUser()
        //{
        //    string sUserId = "0";
        //    if (Session["userId"] != null)
        //    {
        //        sUserId = Session["userId"].ToString();
        //    }

        //    IEnumerable<users> model = db.users.Where(x => x.userId == sUserId);
        //    return View(model);
        //}

        public ActionResult Edit(string id)
        {
            string sUserId = Session["userId"].ToString();
            string sRoleName = Session["RoleName"].ToString();

            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dao = new usersDAO();
            users cq = dao.Details(id);

            cq.ngaycapnhat = DateTime.Now;
            cq.nguoicapnhat = Session["username"].ToString();
            if (cq == null)
            {
                return HttpNotFound();
            }

            List<roles> lstND = DungChung.GetLstLoaiNDTuUser(sUserId, sRoleName);
            ViewBag.role = new SelectList(lstND, "roleName", "roleName", cq.role);
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn", cq.chinhanh);
            ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily", cq.daily);
            return View(cq);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(users cq)
        {
            string sUserId = Session["userId"].ToString();
            string sRoleName = Session["RoleName"].ToString();
            List<roles> lstND = DungChung.GetLstLoaiNDTuUser(sUserId, sRoleName);
            ViewBag.role = new SelectList(lstND, "roleName", "roleName", cq.role);
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn", cq.chinhanh);
            ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily", cq.daily);

            if (ModelState.IsValid)
            {
                var session = Session["username"];

                try
                {
                    var dao = new usersDAO();
                    cq.ngaycapnhat = DateTime.Now;
                    cq.nguoicapnhat = sUserId;
                    cq.trangthai = true;

                    cq.taotour = false;
                    cq.banve = false;
                    cq.suave = false;
                    cq.dongtour = false;
                    cq.dcdanhmuc = false;
                    cq.suatour = false;
                    cq.adminkl = false;
                    cq.adminkd = false;
                    cq.doimk = false;

                    string id = dao.Update(cq);

                    SetAlert("Sửa Thành Công", "success");
                }
                catch (Exception ex)
                {
                    SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");
                   
                }
            }
          
         
            return RedirectToAction("Index", "User");
        }

        public string GenUserID()
        {
            int stt = 0;
            string iduser = "";
            var yc = (from y in db.users orderby y.ngaytao descending select y).FirstOrDefault();

            //neu da co ma phieu voi phan firstPart
            if (yc != null)
            {
                string stt_ = "";
                stt = int.Parse(yc.userId) + 1;
                switch (stt.ToString().Length)
                {
                    case 1:
                        stt_ = "0000" + stt.ToString();
                        break;
                    case 2:
                        stt_ = "000" + stt.ToString();
                        break;
                    case 3:
                        stt_ = "00" + stt.ToString();
                        break;
                    case 4:
                        stt_ = "0" + stt.ToString();
                        break;
                    case 5:
                        stt_ = stt.ToString();
                        break;
                    default:
                        break;
                }
                iduser = stt_;
            }
            else
                iduser = "00001";
            return iduser;
        }

        public ActionResult Create()
        {
            string sUserId = Session["userId"].ToString();
            IQueryable<roles> io = DungChung.GetLoaiNDTuUser(sUserId);
            string sRoleName = Session["RoleName"].ToString();
            List<roles> lstND = DungChung.GetLstLoaiNDTuUser(sUserId, sRoleName);
            ViewBag.role = new SelectList(lstND, "roleName", "roleName");
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn");
            ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily");
            var model = new users();
            model.userId = GenUserID();
            model.nguoitao = Session["username"].ToString();
            model.ngaytao = DateTime.Now;
            model.username = "";
            model.password = "";
            return View(model);
        }

        public users getUserByName(string username)
        {
            var dao = new usersDAO();
            users model = dao.getUserByName(username);
            return model;
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(users cq)
        {
            string sUserId = Session["userId"].ToString();

            if (ModelState.IsValid)
            {
                var session = Session["username"];
                var dao = new usersDAO();
                try
                {
                    cq.trangthai = true;//mac dinh la hien user len
                    cq.userId = GenUserID();
                    cq.nguoitao = sUserId;
                    cq.ngaytao = DateTime.Now;

                    /*
                     *  ,[taotour]
                      ,[banve]
                      ,[suave]
                      ,[dongtour]
                      ,[dcdanhmuc]
                      ,[suatour]
                      ,[adminkl]
                      ,[adminkd]
                     */
                    cq.taotour = false;
                    cq.banve = false;
                    cq.suave = false;
                    cq.dongtour = false;
                    cq.dcdanhmuc = false;
                    cq.suatour = false;
                    cq.adminkl = false;
                    cq.adminkd = false;
                    cq.doimk = false;

                    if (getUserByName(cq.username) != null)//da co user roi
                    {
                        SetAlert("Đã tồn tại user này rồi!", "error");                        
                        //return RedirectToAction("ShowError");
                    }
                    else
                    {
                        string id = dao.Insert(cq, session.ToString());

                        if (id != null)
                        {
                            SetAlert("Thêm  Thành Công", "success");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            SetAlert("Thêm  Không Thành Công", "warning");                            
                          //  return RedirectToAction("ShowError");
                        }
                    }



                }
                catch (Exception ex)
                {
                    SetAlert("Thêm  Không Thành Công, lỗi:" + ex.Message, "warning");                    
                   // return RedirectToAction("ShowError");
                }

            }

            string sRoleName = Session["RoleName"].ToString();
            List<roles> lstND = DungChung.GetLstLoaiNDTuUser(sUserId, sRoleName);
            ViewBag.role = new SelectList(lstND, "roleName", "roleName");
            ViewBag.chinhanh = new SelectList(db.chinhanh, "chinhanh1", "tencn");
            ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily");
            return RedirectToAction("Index", "User");
        }        

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new usersDAO();
            users model = dao.Details(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: quanly/dmxes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new usersDAO();
            users model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult SetHideUser(string id)
        {
            var session = Session["username"];


            if (id.Equals(String.Empty))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new usersDAO();
            users model = dao.Details(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                model.trangthai = !model.trangthai;//an user di
                string idd = dao.Update(model);

                SetAlert("Sửa  Thành Công", "success");                
            }
            catch (Exception ex)
            {
                SetAlert("Sửa  Không Thành Công: Lí do:" + ex.Message, "warning");                
            }

            return RedirectToAction("Index");

        }

        public ActionResult ShowError()
        {
            return View();
        }


        // POST: quanly/dmxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var dao = new usersDAO();

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");                
            }

            return RedirectToAction("Index");
        }
    }
}