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
    public class baocaoDAO
    {
        qlkdtrEntities db = null;
        public baocaoDAO()
        {
            db = new qlkdtrEntities();
        }

        public DataSet BCDoanhThuTheoTuyen2Nam(DateTime bdate, DateTime edate, DateTime bdate1, DateTime edate1, string nguoitao, string roles, string chinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDoanhThuTheoTuyen2Nam";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("bdate1", bdate1));
                cmdReport.Parameters.Add(new SqlParameter("edate1", edate1));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", chinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCSKTheoTuyen2Nam(DateTime bdate, DateTime edate, DateTime bdate1, DateTime edate1, string nguoitao, string roles, string chinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCSKTheoTuyen2Nam";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("bdate1", bdate1));
                cmdReport.Parameters.Add(new SqlParameter("edate1", edate1));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", chinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCDoanhThuTheoPhongKDKD(DateTime bdate, DateTime edate, DateTime bdate1, DateTime edate1, string nguoitao, string roles, string chinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDoanhThuPhongKDKD";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("bdate1", bdate1));
                cmdReport.Parameters.Add(new SqlParameter("edate1", edate1));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", chinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCTheoNhomTT(DateTime bdate, DateTime edate, string nguoitao, string roles, string sChinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDoanhThuTheoNhomThiTruong";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }
        public DataSet BCDoanhThuTheoSales(DateTime bdate, DateTime edate, string nguoitao, string roles, string sChinhanh, string sCongTyPre,string sHoTen)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDoanhThuTheoSales";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));
                cmdReport.Parameters.Add(new SqlParameter("hoten", sHoTen));
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCDTSKGiaTourBQ(DateTime bdate, DateTime edate, DateTime bdate1, DateTime edate1, string nguoitao, string roles, string sChinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDTSKGiaTourBQ";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("bdate1", bdate1));
                cmdReport.Parameters.Add(new SqlParameter("edate1", edate1));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }

            return ds;
        }


        public DataSet BCDTSKTheoTuyen(DateTime bdate, DateTime edate, DateTime bdate1, DateTime edate1, string nguoitao, string roles, string sChinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDTSKTheoTuyen";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("bdate1", bdate1));
                cmdReport.Parameters.Add(new SqlParameter("edate1", edate1));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }


        public DataSet BCDoanhThuTheoTuyen(DateTime bdate, DateTime edate, string nguoitao, string roles, string sChinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDoanhThuTheoTuyen";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCTOUR(DateTime bdate, DateTime edate, string nguoitao, string roles, string sChinhanh, string trangthai, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCTOUR";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("trangthai", trangthai));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCCPHH(DateTime bdate, DateTime edate, string nguoitao, string roles, string sChinhanh, string sCongTyPre)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCCPHH";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", sChinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }

        public DataSet BCDTTheoTuyenND(DateTime bdate, DateTime edate, string nguoitao, string roles, string chinhanh, string sCongTyPre,string loaitour,string nguontour)
        {
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["strkhi_goiprocedure"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(constr))
            {
                SqlCommand cmdReport = sqlConn.CreateCommand();
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.CommandText = "pro_BCDTTheoTuyenND";
                cmdReport.Parameters.Add(new SqlParameter("bdate", bdate));
                cmdReport.Parameters.Add(new SqlParameter("edate", edate));
                //cmdReport.Parameters.Add(new SqlParameter("bdate1", bdate1));
                //cmdReport.Parameters.Add(new SqlParameter("edate1", edate1));
                cmdReport.Parameters.Add(new SqlParameter("nguoitao", nguoitao));
                cmdReport.Parameters.Add(new SqlParameter("role", roles));
                cmdReport.Parameters.Add(new SqlParameter("chinhanh", chinhanh));
                cmdReport.Parameters.Add(new SqlParameter("mactpre", sCongTyPre));
                cmdReport.Parameters.Add(new SqlParameter("loaitourid", loaitour));
                cmdReport.Parameters.Add(new SqlParameter("nguontour", nguontour));

                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    daReport.Fill(ds);
                }
            }


            return ds;
        }
    }
}
