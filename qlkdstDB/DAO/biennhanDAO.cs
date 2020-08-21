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
    public class biennhanDAO
    {
        qlkdtrEntities db = null;
        public biennhanDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(decimal id,string searchString, int page, int pagesize)
        {
            IQueryable<datcoc> model = db.datcoc.Where(x=>x.idtour==id);

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhach.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ngaytao).ToPagedList(page, pagesize);
        }                     

        public datcoc Details(decimal id)
        {
            datcoc model = db.datcoc.Find(id);
            return model;
        }

        public List<vie_biennhan> GetVieBN(decimal id)
        {
            List<vie_biennhan> model = db.vie_biennhan.Where(x => x.iddatcoc == id).ToList();
            return model;
        }

        public string Update(datcoc model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.iddatcoc.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(datcoc model)
        {
            try
            {
                db.datcoc.Add(model);
                db.SaveChanges();
                return model.iddatcoc.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(decimal id)
        {
            datcoc co = db.datcoc.Find(id);
            db.datcoc.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }

        public string GetStrSoBienNhan()
        {
            DataSet ds = GetSoBienNhan();
            DataTable dt = ds.Tables[0];

            string sRes = "";
            if (dt.Rows.Count > 0)
            {
                sRes = dt.Rows[0]["sobiennhan"].ToString();
            }

            return sRes;
        }

        public DataSet GetSoBienNhan()
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "spGetBiennhan";
//                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));


                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        #region "log"

        public datcoclog DetailsLog(decimal id)
        {
            datcoclog model = db.datcoclog.Find(id);
            return model;
        }
       

        public string InsertLog(datcoclog model)
        {
            try
            {
                db.datcoclog.Add(model);
                db.SaveChanges();
                return model.iddatcoclog.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        #endregion
    }
}
