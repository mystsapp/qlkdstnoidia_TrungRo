using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class phankhucnDAO
    {
        qlkdtrEntities db = null;
        public phankhucnDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<phankhucn> model = db.phankhucn;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhucn.Contains(searchString));
            }
            return model.OrderBy(x => x.idkhucn).ToPagedList(page, pagesize);
        }

        public phankhucn Details(int id)
        {
            phankhucn model = db.phankhucn.Find(id);
            return model;
        }

        public string Update(phankhucn model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.idkhucn.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(phankhucn model)
        {
            try
            {
                db.phankhucn.Add(model);
                db.SaveChanges();
                return model.idkhucn.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            phankhucn co = db.phankhucn.Find(id);
            db.phankhucn.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
