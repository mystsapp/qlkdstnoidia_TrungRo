using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class loaitourDAO
    {
        qlkdtrEntities db = null;
        public loaitourDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<loaitour> model = db.loaitour;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenloaitour.Contains(searchString));
            }
            return model.OrderByDescending(x => x.loaitourid).ToPagedList(page, pagesize);
        }


        public loaitour Details(int id)
        {
            loaitour model = db.loaitour.Find(id);
            return model;
        }

        public string Update(loaitour model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.loaitourid.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(loaitour model)
        {
            try
            {
                db.loaitour.Add(model);
                db.SaveChanges();
                return model.loaitourid.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            loaitour co = db.loaitour.Find(id);
            db.loaitour.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
