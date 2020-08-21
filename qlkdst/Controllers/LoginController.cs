using qlkdst.Common;
using qlkdst.Models;
using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class LoginController : Controller
    {
        qlkdtrEntities db = new qlkdtrEntities();

        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Title = "Đăng nhập";
            return View();
        }

        public ActionResult Logout()
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
            return RedirectToAction("Index", "Login");
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

        [HttpPost]
        public ActionResult login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userDao = new usersDAO();
                var result = userDao.login(model.username, model.password,"");
                if (result == 0)
                {
                    //  ModelState.AddModelError("", "Tài khoản này không tồn tại");
                    SetAlert("Tài khoản này không tồn tại!", "warning");
                }
                else if (result == 1)
                {
                    var userInfo = userDao.getUserByName(model.username);
                    Session["username"] = userInfo.username;
                    Session["userId"] = userInfo.userId;
                    Session["hoten"] = userInfo.fullName;
                    Session["RoleName"] = userInfo.role;
                    Session["chinhanh"] = userInfo.chinhanh;
                    Session["USER_SESSION"] = userInfo;
                    Session["daily"] = userInfo.daily;
                    //IQueryable<vie_dsmenu> dsmenu = db.vie_dsmenu;



                    IQueryable<vie_dsmenu> dsmenu = db.vie_dsmenu;

                        // get ds menu co quyen
                        List<MenuModels> _menus = dsmenu.Where(x => x.role == userInfo.role).Select(x => new MenuModels
                        {
                            menuid = x.menuid,
                            menunm = x.menunm,
                            menulink = x.menulink,
                            areaid = x.areaid,
                            show_mk = x.show_mk,
                            classcss = x.classcss,
                            role = x.role,
                            areaname = x.areaname,
                            areacss = x.areacss,
                            actionnm = x.actionnm,
                            controllernm = x.controllernm,
                            areamvc = x.areamvc,
                            thutu = x.thutu,
                            tt = x.tt

                        }).OrderBy(x => x.tt).ThenBy(x => x.thutu).ToList();

                        Session["MenuMaster"] = _menus; //Bind the _menus list to MenuMaster session

                  



                    if (Session["RoleName"].Equals("admin") || Session["RoleName"].Equals("superadmin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (result == -1)
                {
                    //ModelState.AddModelError("", "Tài khoản này đã bị khóa");
                    SetAlert("Tài khoản này đã bị khóa!", "warning");
                }
                else if (result == -2)
                {
                    //ModelState.AddModelError("", "Mật khẩu không đúng.");
                    SetAlert("Mật khẩu không đúng.", "warning");
                }
            }
            return View("Index");
        }
    }
}