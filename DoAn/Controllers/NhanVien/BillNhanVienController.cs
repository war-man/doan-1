﻿using DoAn.Common.Session;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Dao.NhanVien;
using DoAn.Models.EF;
using DoAn.Models.Model.NhanVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.NhanVien
{
    public class BillNhanVienController : Controller
    {
        // GET: BillNhanVien
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult CreateBill()
        {
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult HoaDonTheoId()
        {
            var session_billid = (BillSession)Session[DoAn.Common.Constants.BILL_SESSION];
            var list = new List<CTHDBanModel>();
            if (session_billid != null)
            {
                var list_bill = db.ChiTietHDBs.Where(x => x.MaHDB == session_billid.Id.ToString()).ToList();
                var chitietthu_session = (BillSession)Session[DoAn.Common.Constants.CTTHU_SESSION];
                foreach (var item in list_bill)
                {
                    var model = new CTHDBanModel();
                    var product = new ProductDao().getByid(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        model.MaSanPham = item.MaSanPham;
                        model.Id = item.Id;
                        model.TenSanPham = product.TenSanPham;
                        model.Anh = product.Anh;
                        model.SoLuong = item.SoLuong;
                        ChiTietHDB cthdb = new CTHDBanDao().getById(item.Id);
                        model.CTThu = cthdb.ChiTietThu;
                        model.GiaBan = new CTHDBanDao().Tien1LyTraSua(session_billid.Id, product.Id, cthdb.ChiTietThu);
                        model.ThanhTien = model.GiaBan * model.SoLuong;
                        model.MoTa = new CTHDBanDao().getMoTa(session_billid.Id.ToString(), cthdb.ChiTietThu);
                        list.Add(model);
                    }
                }

            }
            return PartialView(list);
        }

        public ActionResult CreateChiTietHD(string listproduct)
        {

            var session_billid = (BillSession)Session[DoAn.Common.Constants.BILL_SESSION];
            var chitietthu_session = (BillSession)Session[Common.Constants.CTTHU_SESSION];

            if (session_billid != null)
            {
                var chitietthu = chitietthu_session.ChiTietThu + 1;
                string[] productsid = listproduct.Split(',');
                foreach (var item in productsid)
                {

                    ChiTietHDB cthdb = new ChiTietHDB();
                    cthdb.MaHDB = session_billid.Id;
                    cthdb.MaSanPham = int.Parse(item);
                    var product = new ProductDao().getByid(cthdb.MaSanPham);
                    cthdb.SoLuong = 1;
                    cthdb.ThanhTien = product.KhuyenMai * cthdb.SoLuong;
                    cthdb.ThuocSanPham = int.Parse(productsid[0]);
                    cthdb.ChiTietThu = chitietthu;
                    new CTHDBanDao().Insert_CT(cthdb);

                }
                chitietthu_session.ChiTietThu = chitietthu;
                Session.Add(Common.Constants.CTTHU_SESSION, chitietthu_session);
            }
            else
            {
                // tạo mã hóa đơn



                var session_nhanvien = (NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
                DateTime now = DateTime.Now;
                var idbill = session_nhanvien.Id.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
                var BillSession = new BillSession();
                BillSession.Id = idbill;
                Session.Add(Common.Constants.BILL_SESSION, BillSession);

                BillSession.ChiTietThu = 1;
                Session.Add(Common.Constants.CTTHU_SESSION, BillSession);

                var hoadonban = new HoaDonBan();
                hoadonban.Id = idbill;
                hoadonban.MaNhanVien = session_nhanvien.Id;
                hoadonban.MaKhach = 4;
                hoadonban.TongTien = 0;
                hoadonban.NgayBan = now;
                hoadonban.DaThanhToan = 1;
                hoadonban.Duyet = 1;

                var machinhanh = new Models.Dao.Admin.NhanVienDao().getByid(session_nhanvien.Id).MaChiNhanh;
                hoadonban.MaChiNhanh = machinhanh;
                db.HoaDonBans.Add(hoadonban);
                db.SaveChanges();

                string[] productsid = listproduct.Split(',');
                foreach (var item in productsid)
                {
                    session_billid = (BillSession)Session[DoAn.Common.Constants.BILL_SESSION];
                    ChiTietHDB cthdb = new ChiTietHDB();

                    cthdb.MaHDB = session_billid.Id;
                    cthdb.MaSanPham = int.Parse(item);
                    var product = new ProductDao().getByid(cthdb.MaSanPham);
                    var giaban = product.KhuyenMai;
                    cthdb.SoLuong = 1;
                    cthdb.ThuocSanPham = int.Parse(productsid[0]);
                    cthdb.ThanhTien = cthdb.SoLuong * giaban;
                    cthdb.ChiTietThu = 1;
                    new CTHDBanDao().Insert_CT(cthdb);

                }
            }
            var tongtienhd = db.HoaDonBans.Where(x => x.Id == session_billid.Id).Sum(x => x.TongTien);
            var hoadonban1 = db.HoaDonBans.Find(session_billid.Id);
            hoadonban1.TongTien_HoaDon = tongtienhd;
            db.SaveChanges();
            return Redirect("/HomeNhanVien/Index");
        }
        public ActionResult UpdateChiTietHD(int productid, int? soluong, int? chitietthu)
        {
            var session_billid = (DoAn.Common.Session.BillSession)Session[DoAn.Common.Constants.BILL_SESSION];

            if (session_billid != null)
            {
                var billid = session_billid.Id;
                var list = new CTHDBanDao().layDSSP(productid, session_billid.Id, chitietthu);
                foreach (var item in list)
                {
                    var cart = new ChiTietHDB();
                    new CTHDBanDao().Update(item.Id, soluong);
                }
            }
            var tongtienhd = db.HoaDonBans.Where(x => x.Id == session_billid.Id).Sum(x => x.TongTien);
            var hoadonban = db.HoaDonBans.Find(session_billid.Id);
            hoadonban.TongTien = tongtienhd;
            db.SaveChanges();
            return RedirectToAction("Index", "HomeNhanVien");
        }
        public ActionResult DeleteChiTietHD(int productid, int? chitietthu)
        {
            var session_billid = (DoAn.Common.Session.BillSession)Session[DoAn.Common.Constants.BILL_SESSION];
            if (session_billid != null)
            {
                var billid = session_billid.Id;
                var list = new CTHDBanDao().layDSSP(productid, session_billid.Id, chitietthu);
                foreach (var item in list)
                {

                    new CTHDBanDao().Delete(item.Id);
                }
            }
            var tongtienhd = db.HoaDonBans.Where(x => x.Id == session_billid.Id).Sum(x => x.TongTien);
            var hoadonban = db.HoaDonBans.Find(session_billid.Id);
            hoadonban.TongTien = tongtienhd;
            db.SaveChanges();

            return RedirectToAction("Index", "HomeNhanVien");
        }
        [ChildActionOnly]
        public PartialViewResult TongTienSanPham()
        {
            var session_billid = (BillSession)Session[DoAn.Common.Constants.BILL_SESSION];
            if (session_billid != null)
            {
                var list = db.ChiTietHDBs.Where(x => x.MaHDB == session_billid.Id).ToList();
                ViewBag.TongTienSanPham = list.Sum(x => x.ThanhTien);
            }
            else
            {
                ViewBag.TongTienSanPham = 0;
            }


            return PartialView();
        }
        public ActionResult ThanhToan()
        {
            var model = new List<CTHDBanModel>();
            var session_billid = (DoAn.Common.Session.BillSession)Session[DoAn.Common.Constants.BILL_SESSION];
            var list = db.ChiTietHDBs.Where(x => x.MaHDB == session_billid.Id).ToList();
            var hoadonban = db.HoaDonBans.Find(session_billid.Id);
            hoadonban.TongTien = list.Sum(x => x.ThanhTien);
            hoadonban.TongTien_HoaDon = hoadonban.TongTien;
            db.SaveChanges();
            var i = 0;
            foreach (var item in list)
            {
                var itemmodel = new CTHDBanModel();
                var loaispdao = new CategoryDao();
                var product = new ProductDao().getByid(item.MaSanPham);
                itemmodel.TenSanPham = product.TenSanPham;
                itemmodel.GiaBan = product.KhuyenMai;
                itemmodel.SoLuong = item.SoLuong;
                itemmodel.ThanhTien = item.ThanhTien;
                if (loaispdao.getSPChinh(item.MaSanPham) == 1)
                {
                    i++;
                    itemmodel.STT = i;
                }
                if (product.MaLoaiSanPham != 12 && product.MaLoaiSanPham != 13)
                {
                    model.Add(itemmodel);
                }

            }
            ViewBag.MaHoaDon = session_billid.Id;
            return View(model);
        }
        public ActionResult DanhSachHoaDon_NhanVien()
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            var model = new List<DoAn.Models.Model.NhanVien.HoaDonBanModel>();
            var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == session_nhanvien.Id);
            var list = db.HoaDonBans.Where(x => x.MaChiNhanh == nhanvien.MaChiNhanh  && x.DaThanhToan == 0).ToList();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new DoAn.Models.Model.NhanVien.HoaDonBanModel();
                itemmodel.Id = item.Id;
                var khachhang = db.KhachHangs.FirstOrDefault(x => x.Id == item.MaKhach);
                itemmodel.DiaChi = item.DiaChi;
                itemmodel.SDT = khachhang.SDT;
                itemmodel.HoTen = khachhang.HoTen;
                itemmodel.STT = i;
                itemmodel.TongTien = item.TongTien;
                itemmodel.Status = item.Duyet;
                itemmodel.DaThanhToan = item.DaThanhToan;
                model.Add(itemmodel);
            }
            return View(model);
        }
        public ActionResult ChiTietHoaDon_Online(string mahoadon)
        {
            var model = new List<CTHDBanModel>();

            var list = db.ChiTietHDBs.Where(x => x.MaHDB == mahoadon).ToList();
            var i = 0;
            foreach (var item in list)
            {
                var itemmodel = new CTHDBanModel();
                var loaispdao = new CategoryDao();
                var product = new ProductDao().getByid(item.MaSanPham);
                itemmodel.TenSanPham = product.TenSanPham;
                itemmodel.GiaBan = product.KhuyenMai;
                itemmodel.SoLuong = item.SoLuong;
                itemmodel.ThanhTien = item.ThanhTien;
                if (loaispdao.getSPChinh(item.MaSanPham) == 1)
                {
                    i++;
                    itemmodel.STT = i;
                }
                if (product.MaLoaiSanPham != 12 && product.MaLoaiSanPham != 13)
                {
                    model.Add(itemmodel);
                }

            }
            var hoadonban = db.HoaDonBans.FirstOrDefault(x => x.Id == mahoadon);
            var khachhang = db.KhachHangs.FirstOrDefault(x => x.Id == hoadonban.MaKhach);
            ViewBag.KhachHang = khachhang;
            ViewBag.MaHoaDon = mahoadon;
            ViewBag.TongTienHoaDon = hoadonban.TongTien_HoaDon;
            ViewBag.PhiShip = hoadonban.PhiShip;
            return View(model);
        }
        public ActionResult SuaThanhToanOffline_NhanVien(string mahdb)
        {
            var hoadonban = db.HoaDonBans.Find(mahdb);
            hoadonban.DaThanhToan = 1;
            db.SaveChanges();
            Session[DoAn.Common.Constants.MAHDN_SESSION] = null;
            Session[DoAn.Common.Constants.CTTHU_SESSION] = null;
            Session[DoAn.Common.Constants.SANPHAMTHU_SESSION] = null;
            Session[DoAn.Common.Constants.BILL_SESSION] = null;
            return RedirectToAction("Index", "HomeNhanVien");
        }
        public ActionResult SuaDuyetOnline_NhanVien(string mahdb)
        {
            var hoadonban = db.HoaDonBans.Find(mahdb);
            hoadonban.Duyet = 1;
            db.SaveChanges();
            Session[DoAn.Common.Constants.MAHDN_SESSION] = null;
            Session[DoAn.Common.Constants.CTTHU_SESSION] = null;
            Session[DoAn.Common.Constants.SANPHAMTHU_SESSION] = null;
            Session[DoAn.Common.Constants.BILL_SESSION] = null;
            return RedirectToAction("DanhSachHoaDon_NhanVien", "BillNhanVien");  
        }
        public ActionResult SuaThanhToanOnline_NhanVien(string mahdb)
        {
            var hoadonban = db.HoaDonBans.Find(mahdb);
            hoadonban.DaThanhToan = 1;
            db.SaveChanges();
            return RedirectToAction("DanhSachHoaDon_NhanVien", "BillNhanVien");
        }
    }
}