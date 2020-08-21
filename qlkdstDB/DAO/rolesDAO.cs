using PagedList;
using qlkdstDB.EF;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class rolesDAO
    {
        qlkdtrEntities db = null;
        public rolesDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<roles> model = db.roles;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.roleName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.roleName).ToPagedList(page, pagesize);
        }


        public roles Details(int id)
        {
            roles model = db.roles.Find(id);
            return model;
        }

        public string Update(roles model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(roles model)
        {
            try
            {
                db.roles.Add(model);
                db.SaveChanges();
                return model.id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            roles co = db.roles.Find(id);
            db.roles.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
