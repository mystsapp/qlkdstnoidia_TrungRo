using PagedList;
using qlkdstDB.EF;
using System;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class mainmenuDao
    {
        qlkdtrEntities db = null;
        public mainmenuDao()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<mainmenu> model = db.mainmenu;

            model = model.Where(x => x.show_mk == true);//chi hien khu co show_mk = true

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.areaname.Contains(searchString));
            }
            return model.OrderBy(x => x.areaname).ToPagedList(page, pagesize);
        }

        public mainmenu Details(int id)
        {
            mainmenu model = db.mainmenu.Find(id);
            return model;
        }

        public string Update(mainmenu model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.areaname;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(mainmenu model)
        {
            try
            {
                db.mainmenu.Add(model);
                db.SaveChanges();
                return model.areaname;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(int id)
        {
            mainmenu co = db.mainmenu.Find(id);
            db.mainmenu.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
