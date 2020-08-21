using PagedList;
using qlkdstDB.DAO;
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
    public class usersDAO
    {
        qlkdtrEntities db = null;
        MaHoaSHA1 sha1 = null;
        public usersDAO()
        {
            db = new qlkdtrEntities();
            sha1 = new MaHoaSHA1();
        }
        public IEnumerable<users> ListAllPaging(string searchString, string chinhanh, int page, int pageSize)
        {
            IQueryable<users> model = db.users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.username.Contains(searchString));
            }

            return model.Where(x => x.chinhanh == chinhanh).OrderBy(x => x.username).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// kiem tra username co phai la user thuoc nhom admin
        /// true: admin  false: user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool GetAdminUsers(string username)
        {
            bool b = false;//user

            string sAdminUsersKey = System.Configuration.ConfigurationManager.AppSettings["AdminUsers"].ToString();

            //lay ra tung user
            string[] arrs = sAdminUsersKey.Split(';');

            foreach (string s in arrs)
            {
                if (s.ToLower() == username.ToLower())
                {
                    b = true;
                    break;
                }
            }

            return b;
        }

        public object ListAllPageList(string searchString, int page, int pagesize, string userId, string sRoleName)
        {
            IQueryable<users> model = db.users;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.username.Contains(searchString) || x.chinhanh.Contains(searchString) || x.role.Contains(searchString) || x.fullName.Contains(searchString) || x.dienthoai.Contains(searchString));
            }

            if (!sRoleName.ToLower().Equals("superadmin"))
            {//an user co quyen superadmin di
                if (sRoleName == "admin") //thay tat ca role tru role superadmin
                {
                    //user không phải role superadmin thì  không thấy được role superadmin
                    model = model.Where(x => x.role.ToLower() != "superadmin");
                }
                else if (sRoleName == "salemanager") //user khac thi khong duoc thay nhom superadmin admin
                {
                    model = model.Where(x => x.role.ToLower() != "superadmin" && x.role.ToLower() != "admin");
                }
                else //nhóm sale chỉ sửa thông tin của chính mình
                {
                    //model = model.Where(x => x.role.ToLower() != "superadmin" && x.role.ToLower() != "admin" && x.role.ToLower()!= "salemanager");
                    model = model.Where(x => x.userId == userId);
                }
            }


            return model.OrderBy(x => x.chinhanh).ThenBy(x => x.username).ToPagedList(page, pagesize);
        }

        public class vie_dsmenu1
        {
            public int menuid { get; set; }
            public string menunm { get; set; }
            public string menulink { get; set; }
            public Nullable<int> areaid { get; set; }
            public bool show_mk { get; set; }
            public string classcss { get; set; }
            public string role { get; set; }
            public string actionnm { get; set; }
            public string controllernm { get; set; }
            public string areamvc { get; set; }
            public Nullable<int> thutu { get; set; }
            public string areaname { get; set; }
            public string areacss { get; set; }
            public int tt { get; set; }
        }

        public List<vie_dsmenu1> GetDSMenu(string role, string ob)
        {
            DataSet ds = spBCGetDSMenu(role, ob);
            DataTable dt = ds.Tables[0];
            List<vie_dsmenu1> lst = new List<vie_dsmenu1>();
            foreach (DataRow row in dt.Rows)
            {
                vie_dsmenu1 m = new vie_dsmenu1();
                m.menuid = int.Parse(row["menuid"].ToString());
                m.menunm = row["menunm"].ToString();
                m.menulink = row["menulink"].ToString();
                m.areaid = int.Parse(row["areaid"].ToString());
                m.show_mk = (bool)row["show_mk"];
                m.classcss = row["classcss"].ToString();
                m.role = row["role"].ToString();
                m.areaname = row["areaname"].ToString();
                m.areacss = row["areacss"].ToString();
                m.actionnm = row["actionnm"].ToString();
                m.controllernm = row["controllernm"].ToString();
                m.areamvc = row["areamvc"].ToString();
                //m.thutu = int.Parse(row["thutu"]); 
                m.tt = int.Parse(row["tt"].ToString());
                lst.Add(m);
            }
            return lst;
        }

        public DataSet spBCGetDSMenu(string role, string ob)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["streportsysconnect"].ConnectionString;
            if (ob == null) ob = "";
            if (ob.ToLower() == "ib") //neu la bao bieu ib
            {
                constr = ConfigurationManager.ConnectionStrings["streportsysconnectIB"].ConnectionString;
            }
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "spBCGetDSMenu";
                cmdReport.Parameters.Add(new SqlParameter("role", role));
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet spBCGetUsers(string username, string ob)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["streportsysconnect"].ConnectionString;
            if (ob == null) ob = "";
            if (ob.ToLower() == "ib") //neu la bao bieu ib
            {
                constr = ConfigurationManager.ConnectionStrings["streportsysconnectIB"].ConnectionString;
            }
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "spBCGetUsers";
                cmdReport.Parameters.Add(new SqlParameter("username", username));
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }
        public int login(string username, string password, string grp)
        {
            var result = db.users.Where(x => x.username == username).SingleOrDefault();         

            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.trangthai == false)
                {
                    return -1;
                }
                else
                {
                    if (result.password == sha1.EncodeSHA1(password))
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }
        public users getUserByName(string username)
        {
            var result = db.users.SingleOrDefault(x => x.username.Equals(username));           
            return result;
            //return db.users.SingleOrDefault(x => x.username.Equals(username));
        }
        public bool insertUser(users entity)
        {
            try
            {
                entity.password = sha1.EncodeSHA1(entity.password);
                db.users.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Insert(users model, string nguoitao)
        {
            try
            {
                model.password = sha1.EncodeSHA1(model.password);
                model.ngaytao = System.DateTime.Now;
                model.nguoitao = nguoitao;
                db.users.Add(model);
                db.SaveChanges();
                return model.username;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public users Details(string id)
        {
            users model = db.users.Find(id);
            return model;
        }

        public users DetailsByUsrName(string username)
        {
            users model = db.users.Where(x => x.username == username).FirstOrDefault();
            return model;
        }


        public string UpdatePass(users entity)
        {
            try
            {
                entity.password = sha1.EncodeSHA1(entity.password);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return entity.username;
            }
            catch(Exception ex)
            {
                throw ex;
            }       


        }

        public string Update(users entity)
        {
            try
            {
                var result = db.users.Find(entity.userId);
                if (!string.IsNullOrEmpty(entity.password))
                {
                    //kiem tra neu giong pass cu thi khong ma hoa nua
                    if (entity.password != result.password)
                    {                 
                        result.password = sha1.EncodeSHA1(entity.password);
                    }                    
                }               

                result.username = entity.username;
                result.fullName = entity.fullName;                
                result.email = entity.email;
                result.dienthoai = entity.dienthoai;
                result.chinhanh = entity.chinhanh;
                result.daily = entity.daily;
                result.role = entity.role;
                result.nguoicapnhat = entity.nguoicapnhat;
                result.ngaycapnhat = System.DateTime.Now;
                result.trangthai = entity.trangthai;

                result.taotour = entity.taotour;
                result.banve = entity.banve;
                result.suave = entity.suave;
                result.dongtour = entity.dongtour;
                result.dcdanhmuc = entity.dcdanhmuc;
                result.suatour = entity.suatour;
                result.adminkl = entity.adminkl;
                result.adminkd = entity.adminkd;
                result.doimk = entity.doimk;

                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return result.username;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Delete(string id)
        {
            users co = db.users.Find(id);
            db.users.Remove(co);
            db.SaveChanges();
            return co.username;
        }

   
    }
}
