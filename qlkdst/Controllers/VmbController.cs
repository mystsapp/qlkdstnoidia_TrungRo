using qlkdst.Common;
using qlkdstDB.EF;
using qlkdstDB.DAO;
using System;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace qlkdst.Controllers
{
    public class VmbController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();
        // GET: BienNhan
        public ActionResult Index()
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            return View();
        }

        //public ActionResult VmbIndex(decimal id/*,string searchString*/, int page = 1, int pagesize = 10)
        //{
        //    if (Session["username"] == null) return RedirectToAction("login", "Login");
        //    vemaybayDAO dao = new vemaybayDAO();
        //    var model = dao.ListAllPageList(id,"", page, pagesize);
        //    ViewBag.searchString = "";
        //    ViewBag.idtour = id;
        //    return View(model);
        //}
        public ActionResult VmbIndex(decimal id)
        {
            var dao = new tourDAO();
            var model = dao.Details(id);
            ViewBag.idtour = id;
            return View(model);
        }

        private long GetFTPFileSize(string fullftpfilepath, string user, string pwd)
        {
            long size = 0;

            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(fullftpfilepath));
                request.Proxy = null;
                request.Credentials = new NetworkCredential(user, pwd);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                size = response.ContentLength;
                response.Close();
            }
            catch
            {

            }


            return size;
        }

        /// <summary>
        /// lay file tu ftp va view online/download
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult ShowSoftwareNewJsTreeFTP(string path)
        {
            string strFileName = "";
            //For test
            //Session["userid"] = mUsrTest;
            //Session["pass"] = mPwd;
            path = path.Replace("aabbccddee", ":");
            path = path.Replace("asdfghjkl", "/");
            path = path.Replace("aqwsedrf", "+");
            path = HttpUtility.UrlDecode(path);
            int iIndex = path.IndexOf('/');

            if (iIndex > -1)
            {
                path = path.Substring(iIndex + 1);

                iIndex = path.IndexOf('/');

                if (iIndex == 0)
                {
                    path = path.TrimStart('/');
                }

                int iLastIndex = path.LastIndexOf('/');
                if (iLastIndex > -1)
                {
                    strFileName = path.Substring(iLastIndex + 1);
                }


                path = "ftp://" + path;
            }


            //http://stackoverflow.com/questions/1176022/unknown-file-type-mime
            //return base.File(ftpPath, "application/octet-stream");

            //LARGE FILE              
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(DungChung.USER, DungChung.PASS);
                FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();

                //LARGE FILE

                // **************************************************
                Response.Buffer = false;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName);

                // **************************************************
                // 8KB

                //---------------------------------------------------
                long lngFileLength = GetFTPFileSize(path, DungChung.USER, DungChung.PASS);
                Response.AddHeader("Content-Length", lngFileLength.ToString());

                using (Stream responseStream = ftpResponse.GetResponseStream())
                {
                    int intBufferSize = 8 * 1024;

                    // Create buffer for reading [intBufferSize] bytes from file
                    byte[] bytBuffers = new System.Byte[intBufferSize];

                    // Total bytes that should be read
                    long lngDataToRead = lngFileLength;

                    // Read the bytes of file
                    while (lngDataToRead > 0)
                    {
                        // Verify that the client is connected or not?
                        if (Response.IsClientConnected)
                        {
                            // Read the data and put it in the buffer.
                            int intTheBytesThatReallyHasBeenReadFromTheStream =
                                responseStream.Read(buffer: bytBuffers, offset: 0, count: intBufferSize);

                            // Write the data from buffer to the current output stream.
                            Response.OutputStream.Write(buffer: bytBuffers, offset: 0, count: intTheBytesThatReallyHasBeenReadFromTheStream);

                            // Flush (Send) the data to output
                            // (Don't buffer in server's RAM!)
                            Response.Flush();

                            lngDataToRead =
                                lngDataToRead - intTheBytesThatReallyHasBeenReadFromTheStream;
                        }
                        else
                        {
                            // Prevent infinite loop if user disconnected!
                            lngDataToRead = -1;
                        }
                    }

                    if (responseStream != null)
                    {
                        //Close the file.
                        responseStream.Close();
                        responseStream.Dispose();
                    }
                }

                //END LARGER FILE



            }
            catch (WebException we)
            {
                FtpWebResponse response = (FtpWebResponse)we.Response;
                return Content("Lỗi: " + response.StatusDescription);
            }
            catch (Exception ex)
            {
                return Content("Lỗi không đọc được file: " + ex.Message + " !");
            }
            finally
            {
                //Response.Close();
                //nen dung
                this.HttpContext.ApplicationInstance.CompleteRequest();
            }
            //END LARGER FILE                                  

            return View();

        }

        public ActionResult Insert(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            ViewBag.idtour = id;
            vemaybay model = new vemaybay();
            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["username"].ToString();
            tourDAO daotour = new tourDAO();

            tour t = daotour.Details(id);

            string sSgtCode = "";
            if (t != null) sSgtCode = t.sgtcode;

            ViewBag.sgtcode = sSgtCode;
            ViewBag.luotdive = new SelectList(TaoListLuotdive(), "Value", "Text");
            return View(model);
        }

        /// <summary>
        /// sua thong tin ve, id: id ve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(decimal id)
        {
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            vemaybayDAO dao = new vemaybayDAO();
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            vemaybay model = new vemaybay();
            model = dao.Details(id);
            model.ngaysua = DateTime.Now;
            model.nguoisua = Session["username"].ToString();
            Session["nguoitao"] = model.nguoitao;
            Session["ngaytao"] = model.ngaytao;
            tourDAO daotour = new tourDAO();

            tour t = daotour.Details((decimal)model.idtour);

            string sSgtCode = "";
            if (t != null) sSgtCode = t.sgtcode;

            ViewBag.sgtcode = sSgtCode;
            ViewBag.luotdive = new SelectList(TaoListLuotdive(), "Value", "Text",model.luotdive);
           // ViewBag.daily = new SelectList(db.dmdaily, "Daily", "Daily", model.daily);
            return View(model);
        }

        /// <summary>
        /// Sua thong tin bien nhan, id: id  bien nhan
        /// </summary>
     
        /// <returns></returns>
        public ActionResult SuaVmb(decimal id, decimal idk, string chuyenbay, string ngaybay, string diemdi, string diemden, string giodi, string gioden,string luotdive, int? adl_d, int? chl_d, int? inf_d, int? adl_v, int? chl_v, int? inf_v)
        {
            vemaybayDAO dao = new vemaybayDAO();

            if (idk == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            vemaybay model = dao.Details(idk);

            //model.idtour = id;
            //model.id_vmb = idk;

            if (Session["ngaytao"] != null)
            {
                try
                {
                    model.ngaytao = DateTime.Parse(Session["ngaytao"].ToString());
                }
                catch
                {

                }

            }

            model.nguoitao = Session["nguoitao"].ToString();
            model.nguoisua = Session["userId"].ToString();
            model.ngaysua = DateTime.Now;
            model.chuyenbay = chuyenbay;
            if (ngaybay != null)
            {
                try
                {
                    model.ngaybay = DateTime.Parse(ngaybay);
                }
                catch
                {

                }

            }
            model.diemdi = diemdi;
            model.diemden = diemden;
            model.giodi = giodi;
            model.gioden = gioden;
            model.luotdive = luotdive;
            model.sove_adl_d = checkIntNull(adl_d);
            model.sove_chl_d = checkIntNull(chl_d);
            model.sove_inf_d = checkIntNull(inf_d);
            model.sove_adl_v = checkIntNull(adl_v);
            model.sove_chl_v = checkIntNull(chl_v);
            model.sove_inf_v = checkIntNull(inf_v);

            try
            {
                string s = dao.Update(model);

                if (s != "")
                {
                    SetAlert("Sửa Thành Công", "success");
                }
                else
                {
                    SetAlert("Sửa Không Thành Công", "warning");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Sửa Không Thành Công, lỗi: " + ex.Message, "error");
            }
            return RedirectToAction("Index", "QLKhachDoan");
        }

        public List<SelectListItem> TaoListLuotdive()
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Chặng đi", Value = "1" },
                new SelectListItem() { Text = "Chặng về", Value = "2" }                
            };

            return items;
        }

        private int checkIntNull(int? val)
        {
            if (val == null)
                return 0;
            else return (int)val;
        }
        /// <summary>
        /// them vemaybay , id: idtour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateInput(true)]
        //[ValidateAntiForgeryToken]
        public ActionResult ThemVmb(decimal id, string chuyenbay,string ngaybay,string diemdi,string diemden,string giodi,string gioden, string luotdive, int? adl_d, int? chl_d, int? inf_d, int? adl_v, int? chl_v, int? inf_v)
        {
            vemaybayDAO dao = new vemaybayDAO();
            vemaybay model = new vemaybay();
            model.idtour = id;

            if (ngaybay != null)
            {
                try
                {
                    model.ngaybay = DateTime.Parse(ngaybay);
                }
                catch
                {

                }

            }

            

            model.chuyenbay = chuyenbay;
            model.diemdi = diemdi;
            model.diemden = diemden;
            model.giodi = giodi;
            model.gioden = gioden;
            model.luotdive = luotdive;
            model.sove_adl_d = checkIntNull(adl_d);
            model.sove_chl_d = checkIntNull(chl_d);
            model.sove_inf_d = checkIntNull(inf_d);
            model.sove_adl_v = checkIntNull(adl_v);
            model.sove_chl_v = checkIntNull(chl_v);
            model.sove_inf_v = checkIntNull(inf_v);

            model.ngaytao = DateTime.Now;
            model.nguoitao = Session["userId"].ToString();        

            try
            {
                string s = dao.Insert(model);

                if (s != "")
                {
                    SetAlert("Thêm Thành Công", "success");
                }
                else
                {
                    SetAlert("Thêm Không Thành Công", "warning");
                }

            }
            catch (Exception ex)
            {
                SetAlert("Thêm Không Thành Công, lỗi: " + ex.Message, "error");
            }
            return RedirectToAction("Index", "QLKhachDoan");
        }

        public ActionResult Xoa(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new vemaybayDAO();

            try
            {
                string res = dao.Delete(id);
            }
            catch (Exception ex)
            {
                SetAlert("Xóa Không Thành Công" + ex.Message, "warning");
            }

            return RedirectToAction("Index", "QLKhachDoan");
        }
    }
}