using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace qlkdstDB.DAO
{
    public class chinhanhDAO
    {
        qlkdtrEntities db = null;
        public chinhanhDAO()
        {
            db = new qlkdtrEntities();
        }

        public List<chinhanh> Chinhanhs()
        {
            return db.chinhanh.ToList();
        }

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            IQueryable<vie_chinhanh> model = db.vie_chinhanh;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tencn.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ngaytao).ThenBy(x => x.tencn).ToPagedList(page, pagesize);
        }     

        public chinhanh Details(int id)
        {
            chinhanh model = db.chinhanh.Find(id);
            return model;
        }

        public string Update(chinhanh model)
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

        public string Insert(chinhanh model)
        {
            try
            {
                db.chinhanh.Add(model);
                db.SaveChanges();
                return model.Id.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string Delete(int id)
        {
            chinhanh co = db.chinhanh.Find(id);
            db.chinhanh.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }
      
    }
}
