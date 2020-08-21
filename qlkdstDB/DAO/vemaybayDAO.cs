using PagedList;
using qlkdstDB.EF;
using System;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class vemaybayDAO
    {
        qlkdtrEntities db = null;
        public vemaybayDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(decimal id,string searchString, int page, int pagesize)
        {
            IQueryable<vemaybay> model = db.vemaybay.Where(x=>x.idtour==id);

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.chuyenbay.Contains(searchString));
            }
            return model.OrderBy(x => x.chuyenbay).ToPagedList(page, pagesize);
        }

        public vemaybay Details(decimal id)
        {
            vemaybay model = db.vemaybay.Find(id);
            return model;
        }

        public string Update(vemaybay model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id_vmb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(vemaybay model)
        {
            try
            {
                db.vemaybay.Add(model);
                db.SaveChanges();
                return model.id_vmb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(decimal id)
        {
            vemaybay co = db.vemaybay.Find(id);
            db.vemaybay.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
