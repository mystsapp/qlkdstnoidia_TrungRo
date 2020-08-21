using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class thongtinDAO
    {
        qlkdtrEntities db = null;
        public thongtinDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(decimal id,string searchString, int page, int pagesize)
        {
            IQueryable<vie_tttour> model = db.vie_tttour.Where(x=>x.idtour==id);


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.noidungtin.Contains(searchString));
            }
            return model.OrderBy(x => x.noidungtin).ToPagedList(page, pagesize);
        }

        public List<vie_tourvathongtin> LayDSTT(decimal id)
        {
            List<vie_tourvathongtin> lst = new List<vie_tourvathongtin>();
            lst = db.vie_tourvathongtin.Where(x => x.idtour == id).ToList();
            return lst;
        }

        public thongtintour Details(decimal id)
        {
            thongtintour model = db.thongtintour.Find(id);
            return model;
        }

        public string Update(thongtintour model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id_nd.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(thongtintour model)
        {
            try
            {
                db.thongtintour.Add(model);
                db.SaveChanges();
                return model.id_nd.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(decimal id)
        {
            thongtintour co = db.thongtintour.Find(id);
            db.thongtintour.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
