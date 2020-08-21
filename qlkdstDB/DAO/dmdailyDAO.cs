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
    public class dmdailyDAO
    {
        qlkdtrEntities db = null;
        public dmdailyDAO()
        {
            db = new qlkdtrEntities();
        }



        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<dmdaily> model = db.dmdaily;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Daily.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ngaytao).ThenBy(x => x.Daily).ToPagedList(page, pagesize);
        }

        public dmdaily Details(int id)
        {
            dmdaily model = db.dmdaily.Find(id);
            return model;
        }

        public string Update(dmdaily model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(dmdaily model)
        {
            try
            {
                db.dmdaily.Add(model);
                db.SaveChanges();
                return model.Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            dmdaily co = db.dmdaily.Find(id);
            db.dmdaily.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
