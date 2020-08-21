using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;


namespace qlkdstDB.DAO
{
    public class usrkhucnDAO
    {
        qlkdtrEntities db = null;
        public usrkhucnDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<vie_usrkhucn> model = db.vie_usrkhucn;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhucn.Contains(searchString));
            }
            return model.OrderBy(x => x.id_usrkhu).ToPagedList(page, pagesize);
        }

        public usrkhucn Details(int id)
        {
            usrkhucn model = db.usrkhucn.Find(id);
            return model;
        }

        public string Update(usrkhucn model)
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

        public string Insert(usrkhucn model)
        {
            try
            {
                db.usrkhucn.Add(model);
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
            usrkhucn co = db.usrkhucn.Find(id);
            db.usrkhucn.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
