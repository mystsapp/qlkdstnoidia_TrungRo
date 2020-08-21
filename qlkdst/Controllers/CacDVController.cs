//using Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Word;
using qlkdst.Common;
using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class CacDVController : Controller
    {
        // GET: CacDV
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CacDVIndex(decimal id)
        {
            qlkdtrEntities db = new qlkdtrEntities();
            if (Session["username"] == null) return RedirectToAction("login", "Login");
            tourDAO dao = new tourDAO();
            tour model = dao.Details(id);
            string sExt = Path.GetExtension(model.dichvu);
            ViewBag.fileext = sExt;
            ViewBag.UploadPath = System.Configuration.ConfigurationManager.AppSettings["DVUploadPath"].ToString();
            ViewBag.ViewUploadPath = System.Configuration.ConfigurationManager.AppSettings["ViewDVUploadPath"].ToString();
            //if (sExt==".doc" || sExt == ".docx")
            //{
            //    ViewBag.WordHtml = getWordDoc(model.dichvu);
            //}else if(sExt==".xls" || sExt == ".xlsx")
            //{
            //    ViewBag.ExcelHtml = getExcelContent(model.dichvu);
            //}

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


        //public string getWordDoc(string noidungtl)
        //{
        //    string sRes = "";
        //    if (noidungtl != null)
        //    {
        //        string sExt = Path.GetExtension(noidungtl);

        //        if (sExt == ".doc" || sExt == ".docx")
        //        {

        //            string sUploadPath = ConfigurationManager.AppSettings["DVUploadPath"].ToString();

        //            string path = sUploadPath + noidungtl;

        //            object documentFormat = 8;
        //            string randomName = DateTime.Now.Ticks.ToString();
        //            object htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";

        //            object fileSavePath = Server.MapPath(path);

        //            //Open the word document in background.
        //            Microsoft.Office.Interop.Word.Application applicationclass = new Microsoft.Office.Interop.Word.Application();
        //            applicationclass.Documents.Open(ref fileSavePath);

        //            applicationclass.Visible = false;
        //            Document document = applicationclass.ActiveDocument;

        //            document.Fields.Update(); // ** this is the new line of code.
        //                                      //Add COM -> Microsoft Office 16 library
        //            document.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
        //            document.SaveAs2(htmlFilePath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML);

        //            //Close the word document.
        //            document.Close();
        //            applicationclass.Quit();

        //            //Read the saved Html File.
        //            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString(), System.Text.Encoding.UTF8);

        //            wordHTML = wordHTML.Replace(randomName + "_files", "/Temp/" + randomName + "_files");
        //            //Delete the Uploaded Word File.
        //            //System.IO.File.Delete(fileSavePath.ToString());
        //            sRes = wordHTML;
        //            //ViewBag.WordHtml = wordHTML;
        //        }

        //    }//END model.noidungthanhlyhd != null

        //    return sRes;
        //}

        //public ActionResult ViewWordFile(string path)
        //{
        //    string sUploadPath = ConfigurationManager.AppSettings["DVUploadPath"].ToString();

        //    path = sUploadPath + path;

        //    object documentFormat = 8;
        //    string randomName = DateTime.Now.Ticks.ToString();
        //    object htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";

        //    object fileSavePath = Server.MapPath(path);

        //    //Open the word document in background.
        //    Microsoft.Office.Interop.Word.Application applicationclass = new Microsoft.Office.Interop.Word.Application();
        //    applicationclass.Documents.Open(ref fileSavePath);

        //    applicationclass.Visible = false;
        //    Document document = applicationclass.ActiveDocument;

        //    document.Fields.Update(); // ** this is the new line of code.
        //    //Add COM -> Microsoft Office 16 library
        //    document.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
        //    document.SaveAs2(htmlFilePath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML);

        //    //Close the word document.
        //    document.Close();

        //    //Read the saved Html File.
        //    string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString(), System.Text.Encoding.UTF8);

        //    wordHTML = wordHTML.Replace(randomName + "_files", "/Temp/" + randomName + "_files");
        //    //Delete the Uploaded Word File.
        //    //System.IO.File.Delete(fileSavePath.ToString());

        //    ViewBag.WordHtml = wordHTML;


        //    return View();
        //}

        //public ActionResult ViewExcelFile(string path)
        //{
        //    string sUploadPath = ConfigurationManager.AppSettings["DVUploadPath"].ToString();

        //    path = sUploadPath + path;

        //    object xlHtml = 44;
        //    //object documentFormat = 8;
        //    string randomName = DateTime.Now.Ticks.ToString();
        //    string htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";           

        //    string fileSavePath = Server.MapPath(path);

        //    Workbook oWB;
        //    Worksheet oSheet;

        //    try
        //    {
        //        //  creat a Application object
        //        Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
        //        oXL.Visible = false;
        //        //   get   WorkBook  object                
        //        oWB = oXL.Workbooks.Open(fileSavePath);

        //        //   get   WorkSheet object 
        //        oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Sheets[1];

        //        oWB.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
        //        oSheet.SaveAs(htmlFilePath, xlHtml);
        //        oWB.Close();

        //        //Read the saved Html File.
        //        string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());
        //        wordHTML = wordHTML.Replace(randomName + "_files", "/Temp/" + randomName + "_files");

        //        ViewBag.ExcelHtml = wordHTML;

        //        //delete tmp file
        //        //System.IO.File.Delete(htmlFilePath.ToString());


        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ExcelHtml = ex.Message;

        //    }

        //    return View();
        //}

        //public string getExcelContent(string path)
        //{
        //    string sRes = "";
        //    string sUploadPath = ConfigurationManager.AppSettings["DVUploadPath"].ToString();

        //    path = sUploadPath + path;

        //    object xlHtml = 44;
        //    //object documentFormat = 8;
        //    string randomName = DateTime.Now.Ticks.ToString();
        //    string htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";

        //    string fileSavePath = Server.MapPath(path);

        //    Workbook oWB;
        //    Worksheet oSheet;

        //    try
        //    {
        //        //  creat a Application object
        //        Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
        //        oXL.Visible = false;
        //        //   get   WorkBook  object                
        //        oWB = oXL.Workbooks.Open(fileSavePath);

        //        //   get   WorkSheet object 
        //        oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Sheets[1];

        //        oWB.WebOptions.Encoding = Microsoft.Office.Core.MsoEncoding.msoEncodingUTF8;
        //        oSheet.SaveAs(htmlFilePath, xlHtml);
        //        oWB.Close();

        //        //Read the saved Html File.
        //        string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());
        //        wordHTML = wordHTML.Replace(randomName + "_files", "/Temp/" + randomName + "_files");

        //        sRes = wordHTML;

        //        //delete tmp file
        //        //System.IO.File.Delete(htmlFilePath.ToString());


        //    }
        //    catch (Exception ex)
        //    {
        //        sRes = ex.Message;

        //    }

        //    return sRes;
        //}

    }
}