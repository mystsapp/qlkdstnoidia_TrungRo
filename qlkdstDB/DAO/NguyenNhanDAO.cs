using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class NguyenNhanDAO
    {
        qlkdtrEntities db = null;
        public NguyenNhanDAO()
        {
            db = new qlkdtrEntities();
        }

        public object GetDsNN(string[] id)
        {
            IQueryable<cacnoidunghuytour> lst = db.cacnoidunghuytour;

            if (id != null)
            {
                lst = from m in lst where id.Contains(m.idhuytour.ToString()) select m;
            }

            lst = lst.OrderBy(x => x.noidung);
            return lst;
        }


        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<cacnoidunghuytour> model = db.cacnoidunghuytour;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.noidung.Contains(searchString));
            }
            return model.OrderBy(x => x.idhuytour).ToPagedList(page, pagesize);
        }

        public List<cacnoidunghuytour> GetDSNguyenNhanTheoND(string noidung)
        {
            List<cacnoidunghuytour> model = db.cacnoidunghuytour.Where(x => x.noidung.Contains(noidung)).ToList();
            return model;
        }

        public List<cacnoidunghuytour> GetDSNguyenNhan(int id)
        {
            List<cacnoidunghuytour> model = db.cacnoidunghuytour.Where(x => x.idhuytour == id).ToList();
            return model;
        }
        public cacnoidunghuytour Details(decimal id)
        {
            cacnoidunghuytour model = db.cacnoidunghuytour.Find(id);
            return model;
        }

        public string Update(cacnoidunghuytour model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.idhuytour.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(cacnoidunghuytour model)
        {
            try
            {
                db.cacnoidunghuytour.Add(model);
                db.SaveChanges();
                return model.idhuytour.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(decimal id)
        {
            cacnoidunghuytour co = db.cacnoidunghuytour.Find(id);
            db.cacnoidunghuytour.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
