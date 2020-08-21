using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class dmkhachtourDAO
    {
        qlkdtrEntities db = null;
        public dmkhachtourDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<dmkhachtour> model = db.dmkhachtour;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhach.Contains(searchString));
            }
            return model.OrderBy(x => x.tenkhach).ToPagedList(page, pagesize);
        }

        public List<dmkhachtour> GetDSKhachTour(decimal id)
        {
            List<dmkhachtour> model = db.dmkhachtour.Where(x => x.idtour == id).ToList();
            return model;
        }
        public dmkhachtour KhachDetails(decimal id)
        {
            dmkhachtour model = db.dmkhachtour.Find(id);
            return model;
        }

        public string XoaKhachTour(decimal id)
        {
            dmkhachtour co = db.dmkhachtour.Find(id);
            db.dmkhachtour.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }

        public string EditKhachTour(dmkhachtour model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id_dsk.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertKhachTour(dmkhachtour model)
        {
            try
            {
                db.dmkhachtour.Add(model);
                db.SaveChanges();
                return model.id_dsk.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dmkhachtour Details(int id)
        {
            dmkhachtour model = db.dmkhachtour.Find(id);
            return model;
        }


        public string Update(dmkhachtour model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id_dsk.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(dmkhachtour model)
        {
            try
            {
                db.dmkhachtour.Add(model);
                db.SaveChanges();
                return model.id_dsk.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(decimal id)
        {
            dmkhachtour co = db.dmkhachtour.Find(id);
            db.dmkhachtour.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
