using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class dmkhachhangDAO
    {
        qlkdtrEntities db = null;
        public dmkhachhangDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<dmkhachhang> model = db.dmkhachhang;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tengiaodich.Contains(searchString));
            }
            //nhom sales: chi thay khach hang minh tao, nhom salemanager thay het kh thuoc chi nhanh, nhom admin/superadmin thay het


            return model.OrderByDescending(x => x.ngaytao).ThenBy(x => x.tengiaodich).ToPagedList(page, pagesize);
        }

        public object ListAllPageList(string searchString, string dlchinhanh, string userid, string role, string schinhanh, int page, int pagesize)
        {
            IQueryable<dmkhachhang> model = db.dmkhachhang;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tengiaodich.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(dlchinhanh))
            {
                model = model.Where(x => x.chinhanh == dlchinhanh);
            }

            //nhom sales: chi thay khach hang minh tao, nhom salemanager thay het kh thuoc chi nhanh, nhom admin/superadmin thay het
            //14/05: sales thấy như nhóm salemanager nhưng không thấy đc nút add/edit/delete
            if (role == "sales" || role == "salemanager")
            {
                model = model.Where(x => x.chinhanh == schinhanh);
            }

            return model.OrderByDescending(x => x.ngaytao).ThenBy(x => x.tengiaodich).ToPagedList(page, pagesize);
        }

        public class DDLDmKH
        {
            public string tengiaodich { get; set; }//ten+ dia chi
            public string makh { get; set; }
        }

        public List<DDLDmKH> GetDDLDmKHByTenGiaoDich(string tengiaodich)
        {
            // List<DDLDmKH> lst = new List<DDLDmKH>();

            if (tengiaodich != null)
            {
                tengiaodich = tengiaodich.Replace("'", "''");
            }

            string sQuery = "  SELECT makh,tengiaodich+'-'+diachi tengiaodich FROM[dbo].[dmkhachhang] where tengiaodich like '%" + tengiaodich + "%' OR tenthuongmai like '%" + tengiaodich + "%' OR makh like '%" + tengiaodich + "%' OR makhold like '%" + tengiaodich + "%'  ";

            IEnumerable<DDLDmKH> model = db.Database.SqlQuery<DDLDmKH>(sQuery);
            return model.ToList();
        }

        public List<DDLDmKH> GetDDLDmKHByTenGiaoDich(string tengiaodich, string suserId, string role, string schinhanh)
        {
            // List<DDLDmKH> lst = new List<DDLDmKH>();

            if (tengiaodich != null)
            {
                tengiaodich = tengiaodich.Replace("'", "''");
            }

            string sQuery = "  SELECT makh,tengiaodich+'-'+isnull(diachi,'') tengiaodich FROM[dbo].[dmkhachhang] where (tengiaodich like '%" + tengiaodich + "%' OR tenthuongmai like '%" + tengiaodich + "%' OR makh like '%" + tengiaodich + "%' OR codecn like '%" + tengiaodich + "%')  ";


            //nhom sales: chi thay khach hang minh tao, nhom salemanager thay het kh thuoc chi nhanh, nhom admin/superadmin thay het
            //if (role == "sales")
            //{
            //    sQuery = sQuery + " AND nguoitao='" + suserId + "'";
            //}
            //else if (role == "salemanager")
            //{
            //    sQuery = sQuery + " AND chinhanh='" + schinhanh + "'";
            //}

            //14/05: sale thay het chi nhanh luon
            if (role == "sales" || role == "salemanager")
            {
                sQuery = sQuery + " AND chinhanh='" + schinhanh + "'";
            }


            IEnumerable<DDLDmKH> model = db.Database.SqlQuery<DDLDmKH>(sQuery);
            return model.ToList();
        }

        /// <summary>
        /// lay danh sach khach hang theo makh hay makhold
        /// </summary>
        /// <param name="makh"></param>
        /// <returns></returns>
        public List<dmkhachhang> GetDmKHTheoMa(string makh)
        {
            List<dmkhachhang> model = db.dmkhachhang.Where(x => x.makh == makh || (x.codecn == makh && x.codecn != null)).ToList();

            return model;
        }

        public List<dmkhachhang> GetAllKH(string makh, string userid, string role, string schinhanh)
        {
            IQueryable<dmkhachhang> model = db.dmkhachhang;

            //nhom sales: chi thay khach hang minh tao, nhom salemanager thay het kh thuoc chi nhanh, nhom admin/superadmin thay het
            //if (role == "sales")
            //{
            //    model = model.Where(x => x.nguoitao == userid);
            //}
            //else if (role == "salemanager")
            //{
            //    model = model.Where(x => x.chinhanh == schinhanh);
            //}
            if (role == "sales" || role == "salemanager")
            {
                model = model.Where(x => x.chinhanh == schinhanh);
            }

            return model.ToList();
        }

        public List<dmkhachhang> GetAllKH(string makh)
        {
            List<dmkhachhang> model = db.dmkhachhang.ToList();

            return model;
        }

        public List<DDLDmKH> GetDDLDmKH(string suserId, string role, string schinhanh)
        {

            string sQuery = "  SELECT makh,tengiaodich+'-'+isnull(diachi,'') tengiaodich FROM[dbo].[dmkhachhang] where 1=1 ";

            //nhom sales: chi thay khach hang minh tao, nhom salemanager thay het kh thuoc chi nhanh, nhom admin/superadmin thay het
            //if (role == "sales")
            //{
            //    sQuery = sQuery + " AND nguoitao='" + suserId + "'";
            //}
            //else if (role == "salemanager")
            //{
            //    sQuery = sQuery + " AND chinhanh='" + schinhanh + "'";
            //}
            //14/05 cho sale thay chi nhanh
            if (role == "sales" || role == "salemanager")
            {
                sQuery = sQuery + " AND chinhanh='" + schinhanh + "'";
            }

            IEnumerable<DDLDmKH> model = db.Database.SqlQuery<DDLDmKH>(sQuery);

            return model.ToList();
        }

        public List<DDLDmKH> GetDDLDmKH()
        {
            // List<DDLDmKH> lst = new List<DDLDmKH>();

            string sQuery = "  SELECT makh,tengiaodich+'-'+diachi tengiaodich FROM[dbo].[dmkhachhang]";

            IEnumerable<DDLDmKH> model = db.Database.SqlQuery<DDLDmKH>(sQuery);
            return model.ToList();
        }

        public object GetDmKh(string[] id)
        {
            IQueryable<dmkhachhang> lst = db.dmkhachhang;

            if (id != null)
            {
                lst = from m in lst where id.Contains(m.makh) select m;
            }

            lst = lst.OrderBy(x => x.tengiaodich);
            return lst;
        }

        public dmkhachhang Details(string id)
        {
            dmkhachhang model = db.dmkhachhang.Find(id);
            return model;
        }

        public string Update(dmkhachhang model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.makh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(dmkhachhang model)
        {
            try
            {
                db.dmkhachhang.Add(model);
                db.SaveChanges();
                return model.makh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(string id)
        {
            dmkhachhang co = db.dmkhachhang.Find(id);
            db.dmkhachhang.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }

        public string GetStrMakh()
        {
            DataSet ds = GetMaxMakh();
            DataTable dt = ds.Tables[0];

            string sRes = "";
            if (dt.Rows.Count > 0)
            {
                sRes = dt.Rows[0]["makh"].ToString();
            }

            return sRes;
        }

        public DataSet GetMaxMakh()
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "spGetMaxMakh";

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }

            return ds;
        }
    }
}
