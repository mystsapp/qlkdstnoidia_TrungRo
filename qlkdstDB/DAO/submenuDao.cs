using PagedList;
using qlkdstDB.EF;
using System;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class submenuDao
    {
        qlkdtrEntities db = null;
        public submenuDao()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, string LoaiND, int page, int pagesize)
        {
            IQueryable<vie_dsmenu> model = db.vie_dsmenu;

            model = model.Where(x => x.show_mk == true);//chi hien menu co show_mk = true

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.menunm.Contains(searchString) || x.role.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(LoaiND))
            {
                model = model.Where(x => x.role.Contains(LoaiND));
            }

            return model.OrderByDescending(x=>x.ngaytao).ThenBy(x => x.menunm).ToPagedList(page, pagesize);
        }

        public submenu Details(int id)
        {
            submenu model = db.submenu.Find(id);
            return model;
        }

        public string Update(submenu model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.menunm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(submenu model)
        {
            try
            {
                db.submenu.Add(model);
                db.SaveChanges();
                return model.menunm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(int id)
        {
            submenu co = db.submenu.Find(id);
            db.submenu.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
