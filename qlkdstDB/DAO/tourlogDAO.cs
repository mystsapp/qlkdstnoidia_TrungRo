using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class tourlogDAO
    {
        qlkdtrEntities db = null;
        public tourlogDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<tourlog> model = db.tourlog;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.sgtcode.Contains(searchString) || x.nguoithaotac.Contains(searchString) || x.hanhdong.Contains(searchString));
            }
            return model.OrderByDescending(x => x.batdau).ToPagedList(page, pagesize);
        }


        public tourlog Details(int id)
        {
            tourlog model = db.tourlog.Find(id);
            return model;
        }

        public string Update(tourlog model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.idtourlog.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(tourlog model)
        {
            try
            {
                db.tourlog.Add(model);
                db.SaveChanges();
                return model.idtourlog.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            tourlog co = db.tourlog.Find(id);
            db.tourlog.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
