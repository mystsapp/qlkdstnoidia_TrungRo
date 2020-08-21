using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class hoahongDAO
    {
        qlkdtrEntities db = null;
        public hoahongDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<dmhoahong> model = db.dmhoahong;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhach.Contains(searchString));
            }
            return model.OrderByDescending(x => x.tenkhach).ToPagedList(page, pagesize);
        }

        public List<vie_tourvahoahong> GetDSHoaHong(decimal id)
        {
            List<vie_tourvahoahong> model = db.vie_tourvahoahong.Where(x => x.idtour == id).ToList();
            return model;
        }
        public dmhoahong Details(decimal id)
        {
            dmhoahong model = db.dmhoahong.Find(id);
            return model;
        }

        public string Update(dmhoahong model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.idhoahong.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(dmhoahong model)
        {
            try
            {
                db.dmhoahong.Add(model);
                db.SaveChanges();
                return model.idhoahong.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(decimal id)
        {
            dmhoahong co = db.dmhoahong.Find(id);
            db.dmhoahong.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
