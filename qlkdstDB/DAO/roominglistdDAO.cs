using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class roominglistdDAO
    {
        qlkdtrEntities db = null;
        public roominglistdDAO()
        {
            db = new qlkdtrEntities();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<roominglistd> model = db.roominglistd;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.sophong.Contains(searchString));
            }
            return model.OrderBy(x => x.sophong).ToPagedList(page, pagesize);
        }


        public roominglistd Details(decimal id)
        {
            roominglistd model = db.roominglistd.Find(id);
            return model;
        }

        public List<roominglistd> GetLstD(decimal id_roominglist)
        {
            List<roominglistd> lst = db.roominglistd.Where(x => x.id_roomlist == id_roominglist).ToList();
            return lst;
        }

        public string Update(roominglistd model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id_roomlistd.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(roominglistd model)
        {
            try
            {
                db.roominglistd.Add(model);
                db.SaveChanges();
                return model.id_roomlistd.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(decimal id)
        {
            roominglistd co = db.roominglistd.Find(id);
            db.roominglistd.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
    }
}
