using OfficeOpenXml;
using qlkdst.Common;
using qlkdst.Models;
using qlkdstDB.DAO;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qlkdst.Controllers
{
    public class DmKhachController : BaseController
    {
        qlkdtrEntities db = new qlkdtrEntities();
        // GET: DmKhach
        public ActionResult Index(decimal id)
        {
            ViewBag.idtour = id;
            return View();
        }

        public ActionResult ImportFromExcel(HttpPostedFileBase uploadExcel)
        {
            if (Request.Files.Count > 0)
            {

                //var file = Request.Files[0];

                if (uploadExcel != null && uploadExcel.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(uploadExcel.FileName);
                    var path = Path.Combine(Server.MapPath("~/ExcelFiles/DmKhach/"), fileName);

                    if (Path.GetExtension(fileName) == ".xlsx" || Path.GetExtension(fileName) == ".xls")
                    {                    

                        ExcelPackage package = new ExcelPackage(uploadExcel.InputStream);
                        DataTable dt = EPPLusExtensions.ExcelToDataTable(package);
                        UploadMessage message = new UploadMessage();
                        try
                        {
                            decimal dID = decimal.Parse(ViewBag.idtour);
                            message = ThemKhach(dID, dt);

                            if (message.count > 0)
                            {
                                SetAlert("Nhập dữ liệu thành công : " + message.message, "success");                              
                            }
                            else if (message.errorCount > 0)
                            {
                                SetAlert("Nhập dữ liệu không được!" + message.message, "error");                             
                            }

                        }
                        catch (Exception ex)
                        {
                            SetAlert("Nhập dữ liệu không được: " + ex.Message, "error");                       
                        }


                    }
                    else
                    {                      
                        SetAlert("Loại file không đúng!", "error");
                    }

                }
            }

            return View();

        }

        private string ConvertPhai(string phai)
        {
            switch (phai.ToLower())
            {
                case "nam":
                    return "1";
                case "nữ":
                    return "0";
                default:
                    return "1";
            }
        }

        public UploadMessage ThemKhach(decimal idtour,DataTable dt)
        {

            UploadMessage msg = new UploadMessage();
            if (dt.Rows.Count > 0)
            {
                //du lieu file Excel có các cột sau
                //Họ tên	Hộ chiếu	Ngày hết hạn HC	ngày sinh	CMND	Ngày làm CMND	Nơi cấp CMND	Phái	Điện thoại 	Quốc tịch

                dmkhachtour model = new dmkhachtour();
                

                foreach (DataRow row in dt.Rows)
                {
                    //if (row[0].ToString() != "")//ngay dung dinh dang moi xu ly
                   // {                       
                        //string loaiphi
                        string sHoTen = row[1].ToString();
                        string sHoChieu = row[2].ToString();
                        string sNgayHetHan = row[3].ToString();
                        string sNgaySinh = row[4].ToString();
                        string sCMND = row[5].ToString();
                        string sNgayLam = row[6].ToString();
                        string sNoiCap = row[7].ToString();
                        string sPhai = ConvertPhai(row[8].ToString());
                        string sDienThoai = row[8].ToString();
                        string sQuocTich = row[9].ToString();                        

                        IQueryable<dmkhachtour> oj = db.dmkhachtour.Where(x => x.idtour == idtour && x.tenkhach == sHoTen);

                        if (oj.FirstOrDefault() != null)  //da nhap  roi , bao loi
                        {
                            msg.message = "Dữ liệu đã có rồi";                            
                            msg.errorCount += 1;
                        }
                        else
                        {
                            model.tenkhach = sHoTen;
                            model.hochieu = sHoChieu;

                            if (sNgayHetHan != null)
                            {
                                try
                                {
                                    model.hieuluchochieu = DateTime.Parse(sNgayHetHan);
                                }
                                catch(Exception ex)
                                {
                                    msg.errorCount += 1;
                                    msg.message += ex.Message;
                                }
                                
                            }

                            if (sNgaySinh != null)
                            {
                                try
                                {
                                    model.ngaysinh = DateTime.Parse(sNgaySinh);
                                }
                                catch (Exception ex)
                                {
                                    msg.errorCount += 1;
                                    msg.message += ex.Message;
                                }

                            }
                            model.socmnd = sCMND;
                            model.noicapcmnd = sNoiCap;


                            if (sNgayLam != null)
                            {
                                try
                                {
                                    model.ngaycmnd = DateTime.Parse(sNgayLam);
                                }
                                catch (Exception ex)
                                {
                                    msg.errorCount += 1;
                                    msg.message += ex.Message;
                                }

                            }

                            model.phai = sPhai;
                            model.dienthoai = sDienThoai;
                            model.quoctich = sQuocTich;

                            //model.macn = row[3] == null ? "" : row[3].ToString();                            
                            //model.sotien = row[5] == null ? 0 : decimal.Parse(row[5].ToString());


                            //model.machitietphi = TaoMaCP(); tu dong tang

                            var dao = new dmkhachtourDAO();
                            string id = dao.Insert(model);
                        
                            if (id != null)
                            {                        
                                msg.count += 1;
                            }
                            else
                            {
                                msg.message +=  "<br/>";
                                msg.errorCount += 1;
                            }
                        }


                    //}

                }
            }

            return msg;
        }
    }
}