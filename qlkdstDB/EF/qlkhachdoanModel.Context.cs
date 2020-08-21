﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace qlkdstDB.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class qlkdtrEntities : DbContext
    {
        public qlkdtrEntities()
            : base("name=qlkdtrEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<chinhanh> chinhanh { get; set; }
        public virtual DbSet<dmdaily> dmdaily { get; set; }
        public virtual DbSet<mainmenu> mainmenu { get; set; }
        public virtual DbSet<submenu> submenu { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<vie_dsmenu> vie_dsmenu { get; set; }
        public virtual DbSet<roles> roles { get; set; }
        public virtual DbSet<dmkhachtour> dmkhachtour { get; set; }
        public virtual DbSet<nuoc> nuoc { get; set; }
        public virtual DbSet<roominglist> roominglist { get; set; }
        public virtual DbSet<roominglistd> roominglistd { get; set; }
        public virtual DbSet<tour> tour { get; set; }
        public virtual DbSet<thongtintour> thongtintour { get; set; }
        public virtual DbSet<loaitour> loaitour { get; set; }
        public virtual DbSet<quan> quan { get; set; }
        public virtual DbSet<dmkhachhang> dmkhachhang { get; set; }
        public virtual DbSet<vie_roomtour> vie_roomtour { get; set; }
        public virtual DbSet<vie_roominglist> vie_roominglist { get; set; }
        public virtual DbSet<vie_khachNotInRoominglist> vie_khachNotInRoominglist { get; set; }
        public virtual DbSet<datcoc> datcoc { get; set; }
        public virtual DbSet<vemaybay> vemaybay { get; set; }
        public virtual DbSet<vie_tttour> vie_tttour { get; set; }
        public virtual DbSet<vie_tourvathongtin> vie_tourvathongtin { get; set; }
        public virtual DbSet<vie_biennhan> vie_biennhan { get; set; }
        public virtual DbSet<vie_tpvanuoc> vie_tpvanuoc { get; set; }
        public virtual DbSet<dmhoahong> dmhoahong { get; set; }
        public virtual DbSet<vie_tourvahoahong> vie_tourvahoahong { get; set; }
        public virtual DbSet<dmnghanhnghe> dmnghanhnghe { get; set; }
        public virtual DbSet<khuvuc> khuvuc { get; set; }
        public virtual DbSet<vie_nuoc> vie_nuoc { get; set; }
        public virtual DbSet<vie_tour> vie_tour { get; set; }
        public virtual DbSet<cacnoidunghuytour> cacnoidunghuytour { get; set; }
        public virtual DbSet<tourlog> tourlog { get; set; }
        public virtual DbSet<tourtmp> tourtmp { get; set; }
        public virtual DbSet<datcoclog> datcoclog { get; set; }
        public virtual DbSet<phankhucn> phankhucn { get; set; }
        public virtual DbSet<vie_chinhanh> vie_chinhanh { get; set; }
        public virtual DbSet<usrkhucn> usrkhucn { get; set; }
        public virtual DbSet<vie_usrkhucn> vie_usrkhucn { get; set; }
        public virtual DbSet<vie_quyenusrtheocn> vie_quyenusrtheocn { get; set; }
    
        public virtual int spInsertUser(string userId, string username, string password, string fullName, string daily, string dienthoai, Nullable<bool> taotour, Nullable<bool> banve, Nullable<bool> suave, Nullable<bool> dongtour, Nullable<bool> dcdanhmuc, Nullable<bool> suatour, Nullable<bool> adminkl, Nullable<bool> adminkd, string email, string emailcc, string chinhanh, string bantour, string role, Nullable<bool> trangthai, string nguoitao)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            var fullNameParameter = fullName != null ?
                new ObjectParameter("fullName", fullName) :
                new ObjectParameter("fullName", typeof(string));
    
            var dailyParameter = daily != null ?
                new ObjectParameter("daily", daily) :
                new ObjectParameter("daily", typeof(string));
    
            var dienthoaiParameter = dienthoai != null ?
                new ObjectParameter("dienthoai", dienthoai) :
                new ObjectParameter("dienthoai", typeof(string));
    
            var taotourParameter = taotour.HasValue ?
                new ObjectParameter("taotour", taotour) :
                new ObjectParameter("taotour", typeof(bool));
    
            var banveParameter = banve.HasValue ?
                new ObjectParameter("banve", banve) :
                new ObjectParameter("banve", typeof(bool));
    
            var suaveParameter = suave.HasValue ?
                new ObjectParameter("suave", suave) :
                new ObjectParameter("suave", typeof(bool));
    
            var dongtourParameter = dongtour.HasValue ?
                new ObjectParameter("dongtour", dongtour) :
                new ObjectParameter("dongtour", typeof(bool));
    
            var dcdanhmucParameter = dcdanhmuc.HasValue ?
                new ObjectParameter("dcdanhmuc", dcdanhmuc) :
                new ObjectParameter("dcdanhmuc", typeof(bool));
    
            var suatourParameter = suatour.HasValue ?
                new ObjectParameter("suatour", suatour) :
                new ObjectParameter("suatour", typeof(bool));
    
            var adminklParameter = adminkl.HasValue ?
                new ObjectParameter("adminkl", adminkl) :
                new ObjectParameter("adminkl", typeof(bool));
    
            var adminkdParameter = adminkd.HasValue ?
                new ObjectParameter("adminkd", adminkd) :
                new ObjectParameter("adminkd", typeof(bool));
    
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var emailccParameter = emailcc != null ?
                new ObjectParameter("emailcc", emailcc) :
                new ObjectParameter("emailcc", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var bantourParameter = bantour != null ?
                new ObjectParameter("bantour", bantour) :
                new ObjectParameter("bantour", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var trangthaiParameter = trangthai.HasValue ?
                new ObjectParameter("trangthai", trangthai) :
                new ObjectParameter("trangthai", typeof(bool));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spInsertUser", userIdParameter, usernameParameter, passwordParameter, fullNameParameter, dailyParameter, dienthoaiParameter, taotourParameter, banveParameter, suaveParameter, dongtourParameter, dcdanhmucParameter, suatourParameter, adminklParameter, adminkdParameter, emailParameter, emailccParameter, chinhanhParameter, bantourParameter, roleParameter, trangthaiParameter, nguoitaoParameter);
        }
    
        public virtual ObjectResult<spLogin_Result> spLogin(string username)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spLogin_Result>("spLogin", usernameParameter);
        }
    
        public virtual ObjectResult<string> spGetBiennhan()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("spGetBiennhan");
        }
    
        public virtual ObjectResult<string> spGetSoHopDong(Nullable<int> nam, Nullable<int> thang)
        {
            var namParameter = nam.HasValue ?
                new ObjectParameter("nam", nam) :
                new ObjectParameter("nam", typeof(int));
    
            var thangParameter = thang.HasValue ?
                new ObjectParameter("thang", thang) :
                new ObjectParameter("thang", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("spGetSoHopDong", namParameter, thangParameter);
        }
    
        public virtual int pro_BCCPHH(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCCPHH", bdateParameter, edateParameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDoanhThuPhongKDKD(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, Nullable<System.DateTime> bdate1, Nullable<System.DateTime> edate1, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var bdate1Parameter = bdate1.HasValue ?
                new ObjectParameter("bdate1", bdate1) :
                new ObjectParameter("bdate1", typeof(System.DateTime));
    
            var edate1Parameter = edate1.HasValue ?
                new ObjectParameter("edate1", edate1) :
                new ObjectParameter("edate1", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDoanhThuPhongKDKD", bdateParameter, edateParameter, bdate1Parameter, edate1Parameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDoanhThuTheoNhomThiTruong(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDoanhThuTheoNhomThiTruong", bdateParameter, edateParameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDoanhThuTheoSales(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, string nguoitao, string role, string chinhanh, string mactpre, string hoten)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            var hotenParameter = hoten != null ?
                new ObjectParameter("hoten", hoten) :
                new ObjectParameter("hoten", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDoanhThuTheoSales", bdateParameter, edateParameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter, hotenParameter);
        }
    
        public virtual int pro_BCDoanhThuTheoTuyen(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDoanhThuTheoTuyen", bdateParameter, edateParameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDoanhThuTheoTuyen2Nam(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, Nullable<System.DateTime> bdate1, Nullable<System.DateTime> edate1, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var bdate1Parameter = bdate1.HasValue ?
                new ObjectParameter("bdate1", bdate1) :
                new ObjectParameter("bdate1", typeof(System.DateTime));
    
            var edate1Parameter = edate1.HasValue ?
                new ObjectParameter("edate1", edate1) :
                new ObjectParameter("edate1", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDoanhThuTheoTuyen2Nam", bdateParameter, edateParameter, bdate1Parameter, edate1Parameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDTSKGiaTourBQ(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, Nullable<System.DateTime> bdate1, Nullable<System.DateTime> edate1, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var bdate1Parameter = bdate1.HasValue ?
                new ObjectParameter("bdate1", bdate1) :
                new ObjectParameter("bdate1", typeof(System.DateTime));
    
            var edate1Parameter = edate1.HasValue ?
                new ObjectParameter("edate1", edate1) :
                new ObjectParameter("edate1", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDTSKGiaTourBQ", bdateParameter, edateParameter, bdate1Parameter, edate1Parameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDTSKTheoTuyen(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, Nullable<System.DateTime> bdate1, Nullable<System.DateTime> edate1, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var bdate1Parameter = bdate1.HasValue ?
                new ObjectParameter("bdate1", bdate1) :
                new ObjectParameter("bdate1", typeof(System.DateTime));
    
            var edate1Parameter = edate1.HasValue ?
                new ObjectParameter("edate1", edate1) :
                new ObjectParameter("edate1", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDTSKTheoTuyen", bdateParameter, edateParameter, bdate1Parameter, edate1Parameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCDTTheoTuyenND(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, string nguoitao, string role, string chinhanh, string mactpre, string loaitourid, string nguontour)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            var loaitouridParameter = loaitourid != null ?
                new ObjectParameter("loaitourid", loaitourid) :
                new ObjectParameter("loaitourid", typeof(string));
    
            var nguontourParameter = nguontour != null ?
                new ObjectParameter("nguontour", nguontour) :
                new ObjectParameter("nguontour", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCDTTheoTuyenND", bdateParameter, edateParameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter, loaitouridParameter, nguontourParameter);
        }
    
        public virtual int pro_BCSKTheoTuyen2Nam(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, Nullable<System.DateTime> bdate1, Nullable<System.DateTime> edate1, string nguoitao, string role, string chinhanh, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var bdate1Parameter = bdate1.HasValue ?
                new ObjectParameter("bdate1", bdate1) :
                new ObjectParameter("bdate1", typeof(System.DateTime));
    
            var edate1Parameter = edate1.HasValue ?
                new ObjectParameter("edate1", edate1) :
                new ObjectParameter("edate1", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCSKTheoTuyen2Nam", bdateParameter, edateParameter, bdate1Parameter, edate1Parameter, nguoitaoParameter, roleParameter, chinhanhParameter, mactpreParameter);
        }
    
        public virtual int pro_BCTOUR(Nullable<System.DateTime> bdate, Nullable<System.DateTime> edate, string nguoitao, string role, string chinhanh, string trangthai, string mactpre)
        {
            var bdateParameter = bdate.HasValue ?
                new ObjectParameter("bdate", bdate) :
                new ObjectParameter("bdate", typeof(System.DateTime));
    
            var edateParameter = edate.HasValue ?
                new ObjectParameter("edate", edate) :
                new ObjectParameter("edate", typeof(System.DateTime));
    
            var nguoitaoParameter = nguoitao != null ?
                new ObjectParameter("nguoitao", nguoitao) :
                new ObjectParameter("nguoitao", typeof(string));
    
            var roleParameter = role != null ?
                new ObjectParameter("role", role) :
                new ObjectParameter("role", typeof(string));
    
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var trangthaiParameter = trangthai != null ?
                new ObjectParameter("trangthai", trangthai) :
                new ObjectParameter("trangthai", typeof(string));
    
            var mactpreParameter = mactpre != null ?
                new ObjectParameter("mactpre", mactpre) :
                new ObjectParameter("mactpre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pro_BCTOUR", bdateParameter, edateParameter, nguoitaoParameter, roleParameter, chinhanhParameter, trangthaiParameter, mactpreParameter);
        }
    
        public virtual ObjectResult<string> spGetMaxMakh()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("spGetMaxMakh");
        }
    
        [DbFunction("qlkdtrEntities", "Split")]
        public virtual IQueryable<string> Split(string list)
        {
            var listParameter = list != null ?
                new ObjectParameter("List", list) :
                new ObjectParameter("List", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<string>("[qlkdtrEntities].[Split](@List)", listParameter);
        }
    
        public virtual int spTaoCodeDoan(string chinhanh, Nullable<System.DateTime> batdau, Nullable<System.DateTime> ketthuc, Nullable<int> sokhach, string tuyentq, string chudetour, string makh, string nguoitaotour, ObjectParameter sgtcode)
        {
            var chinhanhParameter = chinhanh != null ?
                new ObjectParameter("chinhanh", chinhanh) :
                new ObjectParameter("chinhanh", typeof(string));
    
            var batdauParameter = batdau.HasValue ?
                new ObjectParameter("batdau", batdau) :
                new ObjectParameter("batdau", typeof(System.DateTime));
    
            var ketthucParameter = ketthuc.HasValue ?
                new ObjectParameter("ketthuc", ketthuc) :
                new ObjectParameter("ketthuc", typeof(System.DateTime));
    
            var sokhachParameter = sokhach.HasValue ?
                new ObjectParameter("sokhach", sokhach) :
                new ObjectParameter("sokhach", typeof(int));
    
            var tuyentqParameter = tuyentq != null ?
                new ObjectParameter("tuyentq", tuyentq) :
                new ObjectParameter("tuyentq", typeof(string));
    
            var chudetourParameter = chudetour != null ?
                new ObjectParameter("chudetour", chudetour) :
                new ObjectParameter("chudetour", typeof(string));
    
            var makhParameter = makh != null ?
                new ObjectParameter("makh", makh) :
                new ObjectParameter("makh", typeof(string));
    
            var nguoitaotourParameter = nguoitaotour != null ?
                new ObjectParameter("nguoitaotour", nguoitaotour) :
                new ObjectParameter("nguoitaotour", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spTaoCodeDoan", chinhanhParameter, batdauParameter, ketthucParameter, sokhachParameter, tuyentqParameter, chudetourParameter, makhParameter, nguoitaotourParameter, sgtcode);
        }
    }
}
