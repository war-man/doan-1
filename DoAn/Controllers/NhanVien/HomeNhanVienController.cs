﻿using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.NhanVien
{
    public class HomeNhanVienController : Controller
    {
        // GET: HomeNhanVien
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult Index(int maloai = 0)
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            if (session_nhanvien != null)
            {
                var list = new List<SanPhamModel>();
                var listproduct = (from sp in db.SanPhams
                                   join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                   where lsp.SanPhamChinh == 1
                                   select new
                                   {
                                       Id = sp.Id,
                                       TenSanPham = sp.TenSanPham,
                                       Anh = sp.Anh,
                                       Gia = sp.KhuyenMai
                                   }).OrderByDescending(x => x.Id);
                if (maloai == 0)
                {
                    
                }
                else
                {
                    listproduct = (from sp in db.SanPhams
                                       join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                       where lsp.SanPhamChinh == 1 & sp.MaLoaiSanPham == maloai
                                       select new
                                       {
                                           Id = sp.Id,
                                           TenSanPham = sp.TenSanPham,
                                           Anh = sp.Anh,
                                           Gia = sp.KhuyenMai
                                       }).OrderByDescending(x => x.Id);
                }
                


                int i = 0;

                foreach (var item in listproduct)
                {
                    i++;
                    var model = new SanPhamModel();
                    model.Id = item.Id;
                    model.STT = i;
                    model.Ten = item.TenSanPham;
                    model.Anh = item.Anh;
                    model.KhuyenMai = item.Gia;
                    list.Add(model);

                }
                ViewBag.MaLoaiSP = maloai;
                return View(list);
            }
            else
            {
                return RedirectToAction("DangNhapNhanVien", "Login");
            }

        }
        [ChildActionOnly]
        public PartialViewResult DSSP_HoaDon(int maloaisanpham = 0)
        {
            if (maloaisanpham == 0)
            {
                var list = new List<SanPhamModel>();

                var listproduct = (from sp in db.SanPhams
                                   join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                   where lsp.SanPhamChinh == 1
                                   select new
                                   {
                                       Id = sp.Id,
                                       TenSanPham = sp.TenSanPham,
                                       Anh = sp.Anh,
                                       Gia = sp.KhuyenMai
                                   }).OrderByDescending(x => x.Id);


                int i = 0;

                foreach (var item in listproduct)
                {
                    i++;
                    var model = new SanPhamModel();
                    model.Id = item.Id;
                    model.STT = i;
                    model.Ten = item.TenSanPham;
                    model.Anh = item.Anh;
                    model.KhuyenMai = item.Gia;
                    list.Add(model);

                }
                return PartialView(list);
            }
            else
            {
                ViewBag.MaLoaiSanPham = maloaisanpham;

                var list = new List<SanPhamModel>();

                var listproduct = (from sp in db.SanPhams
                                   join lsp in db.LoaiSanPhams on sp.MaLoaiSanPham equals lsp.Id
                                   where sp.MaLoaiSanPham == maloaisanpham
                                   select new
                                   {
                                       Id = sp.Id,
                                       TenSanPham = sp.TenSanPham,
                                       Anh = sp.Anh,
                                       Gia = sp.KhuyenMai
                                   }).OrderByDescending(x => x.Id);


                int i = 0;

                foreach (var item in listproduct)
                {
                    i++;
                    var model = new SanPhamModel();
                    model.Id = item.Id;
                    model.STT = i;
                    model.Ten = item.TenSanPham;
                    model.Anh = item.Anh;
                    model.MaLoai = maloaisanpham;
                    model.KhuyenMai = item.Gia;
                    list.Add(model);

                }
                return PartialView(list);

            }


        }
        [ChildActionOnly]
        public PartialViewResult SelectLoaiSanPham()
        {
            var model = new SanPhamModel();
            model.SelectMaLoai = new SelectList(db.LoaiSanPhams.Where(x => x.SanPhamChinh == 1), "Id", "TenLoaiSanPham", 1);
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult SelectLoaiSanPham_Loc(int? maloaisanpham)
        {
            var model = new SanPhamModel();
            model.SelectMaLoai = new SelectList(db.LoaiSanPhams.Where(x => x.SanPhamChinh == 1), "Id", "TenLoaiSanPham", maloaisanpham);
            return PartialView(model);
        }
       
        [ChildActionOnly]
        public PartialViewResult SanPham_Size()
        {
            return PartialView(db.SanPhams.Where(x => x.MaLoaiSanPham == 14).ToList());
        }
        [ChildActionOnly]
        public PartialViewResult SanPham_Duong()
        {
            return PartialView(db.SanPhams.Where(x => x.MaLoaiSanPham == 13).ToList());
        }
        [ChildActionOnly]
        public PartialViewResult SanPham_Da()
        {
            return PartialView(db.SanPhams.Where(x => x.MaLoaiSanPham == 12).ToList());
        }
        [ChildActionOnly]
        public PartialViewResult SanPham_Topping()
        {
            return PartialView(db.SanPhams.Where(x => x.MaLoaiSanPham == 1).ToList());
        }
        [ChildActionOnly]
        public PartialViewResult TenDangNhap()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public PartialViewResult TongHoaDon()
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == session_nhanvien.Id);
            var model = db.HoaDonBans.Where(x => x.MaChiNhanh == nhanvien.MaChiNhanh  && x.DaThanhToan == 0).ToList();
            ViewBag.TongHoaDon = model.Count;
            return PartialView();
        }
        [ChildActionOnly]
        public PartialViewResult TenChiNhanh()
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == session_nhanvien.Id);

            ViewBag.TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh).TenChiNhanh;
            return PartialView();
        }
    }
}