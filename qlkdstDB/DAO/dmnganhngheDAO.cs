using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class dmnganhngheDAO
    {
        qlkdtrEntities db = null;
        public dmnganhngheDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<dmnghanhnghe> model = db.dmnghanhnghe;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tennghanhnghe.Contains(searchString));
            }
            return model.OrderByDescending(x => x.idnghe).ToPagedList(page, pagesize);
        }

        public dmnghanhnghe Details(int id)
        {
            dmnghanhnghe model = db.dmnghanhnghe.Find(id);
            return model;
        }

        public string Update(dmnghanhnghe model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.idnghe.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(dmnghanhnghe model)
        {
            try
            {
                db.dmnghanhnghe.Add(model);
                db.SaveChanges();
                return model.idnghe.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            dmnghanhnghe co = db.dmnghanhnghe.Find(id);
            db.dmnghanhnghe.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
