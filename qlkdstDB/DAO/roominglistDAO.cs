using PagedList;
using qlkdstDB.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace qlkdstDB.DAO
{
  
    public class roominglistDAO
    {
        qlkdtrEntities db = null;
        public roominglistDAO()
        {
            db = new qlkdtrEntities();
        }      

       

        public object ListAllPageList(string searchString, int page, int pagesize)
        {
            //IQueryable<roominglist> model = db.roominglist;

            var model = from r in db.roominglist join s in db.tour on r.idtour equals s.idtour
                        select new  
                        {
                            r.id_roomlist,
                            r.tenkhachsan,
                            r.idtour,
                            s.sgtcode
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.tenkhachsan.Contains(searchString));
            }
            return model.OrderBy(x => x.tenkhachsan).ToPagedList(page, pagesize).ToList();
        }     
      

        public roominglist Details(decimal id)
        {
            roominglist model = db.roominglist.Find(id);
            return model;
        }

        public List<roominglist> LayRoomingList(decimal id)
        {
            List<roominglist> model = db.roominglist.Where(x => x.idtour == id).ToList();
            return model;
        }

        public List<vie_roominglist> LayVieRoomingList(decimal id)
        {
            List<vie_roominglist> model = db.vie_roominglist.Where(x => x.idtour == id).ToList();
            return model.OrderBy(x=>x.sophong).ToList();
        }

        public dmkhachtour GetKhachTourByid(decimal? id)
        {
            dmkhachtour model = db.dmkhachtour.Where(x => x.id_dsk == id).FirstOrDefault();
            return model;
        }

        public List<vie_roomtour> LayDSKS(decimal id)
        {
            List<vie_roomtour> model = db.vie_roomtour.Where(x=>x.idtour==id).ToList();
            return model;
        }

        public List<vie_roominglist> Getvie_roominglist(decimal id)
        {
            List<vie_roominglist> model = db.vie_roominglist.Where(x => x.id_roomlist == id).OrderBy(x=>x.sophong).ToList();
            return model;
        }

        public string Update(roominglist model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return model.id_roomlist.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert(roominglist model)
        {
            try
            {
                db.roominglist.Add(model);
                db.SaveChanges();
                return model.id_roomlist.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

        public string Delete(decimal id)
        {
            roominglist co = db.roominglist.Find(id);
            db.roominglist.Remove(co);
            db.SaveChanges();
            return id.ToString();
        }

        public List<vie_khachNotInRoominglist> ListKhachChuaDuocChon(decimal? idtour)
        {
            List<vie_khachNotInRoominglist> lst = db.vie_khachNotInRoominglist.Where(x => x.idtour == idtour).ToList();
            return lst;
        }
    }
}
