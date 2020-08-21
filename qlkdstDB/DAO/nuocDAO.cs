using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class nuocDAO
    {
        qlkdtrEntities db = null;
        public nuocDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString,int idkhus, int page, int pagesize)
        {
            IQueryable<vie_nuoc> model = db.vie_nuoc;
            if (idkhus > 0)
            {
                model = model.Where(x => x.idkhu == idkhus);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TenNuoc.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ngaytao).ThenBy(x => x.TenNuoc).ToPagedList(page, pagesize);
        }


        public nuoc Details(decimal id)
        {
            nuoc model = db.nuoc.Find(id);
            return model;
        }

        public nuoc DetailsByName(string tennuoc)
        {
            tennuoc = tennuoc.Trim();
            nuoc model = db.nuoc.Where(x => x.TenNuoc==tennuoc).FirstOrDefault();
            return model;
        }

        public bool KTNuocTheoTen(string tennuoc, decimal id)
        {
            bool b = false;//chua co
            int count = db.nuoc.Where(x => x.TenNuoc == tennuoc && x.Id != id).Count();
            b = (count > 0);
            return b;
        }

        public string Update(nuoc model)
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

        public string Insert(nuoc model)
        {
            try
            {
                db.nuoc.Add(model);
                db.SaveChanges();
                return model.Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(decimal id)
        {
            nuoc co = db.nuoc.Find(id);
            db.nuoc.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
