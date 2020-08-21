using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class khuvucDAO
    {
        qlkdtrEntities db = null;
        public khuvucDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<khuvuc> model = db.khuvuc;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhu.Contains(searchString));
            }
            return model.OrderByDescending(x => x.idkhu).ToPagedList(page, pagesize);
        }
       
        public khuvuc Details(int id)
        {
            khuvuc model = db.khuvuc.Find(id);
            return model;
        }

        public string Update(khuvuc model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.idkhu.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(khuvuc model)
        {
            try
            {
                db.khuvuc.Add(model);
                db.SaveChanges();
                return model.idkhu.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            khuvuc co = db.khuvuc.Find(id);
            db.khuvuc.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
