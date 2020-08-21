using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class quanDAO
    {
        qlkdtrEntities db = null;
        public quanDAO()
        {
            db = new qlkdtrEntities();
        }
        
        public object ListAllPageList(string searchString,decimal maquocgia, int page, int pagesize)
        {
            IQueryable<vie_tpvanuoc> model = db.vie_tpvanuoc;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenquan.Contains(searchString));
            }

            if (maquocgia!=0)
            {
                model = model.Where(x => x.maquocgia==maquocgia);
            }

            //return model.OrderByDescending(x => x.ngaytao).ThenBy(x => x.tenquan).ToPagedList(page, pagesize);
            return model.OrderBy(x => x.maquocgia).ThenBy(x => x.tenquan).ToPagedList(page, pagesize);
        }

        public List<quan> GetQuanTheoTenQG(string tenqg)
        {
            //lay id quocgia
            nuoc nc = db.nuoc.Where(x => x.TenNuoc.Contains(tenqg)).FirstOrDefault();

            List<quan> model = db.quan.ToList();
            if (nc != null)
            {
                model = db.quan.Where(x => x.maquocgia == nc.Id).ToList();
            }

            return model.OrderBy(x => x.tenquan).ToList();
        }
        public quan Details(decimal id)
        {
            quan model = db.quan.Find(id);
            return model;
        }

        public string Update(quan model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.maquan.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(quan model)
        {
            try
            {
                db.quan.Add(model);
                db.SaveChanges();
                return model.maquan.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(decimal id)
        {
            quan co = db.quan.Find(id);
            db.quan.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }

    
        //public DataSet GetMaxMakh()
        //{
        //    DataSet ds = new DataSet();
        //    string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
        //    using (SqlConnection sqlConn = new SqlConnection(constr))
        //    {
        //        SqlCommand cmdReport = sqlConn.CreateCommand();
        //        cmdReport.CommandType = CommandType.StoredProcedure;
        //        cmdReport.CommandText = "spGetMaxMakh";

        //        SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
        //        using (cmdReport)
        //        {
        //            daReport.Fill(ds);
        //        }
        //    }

        //    return ds;
        //}
    }
}
