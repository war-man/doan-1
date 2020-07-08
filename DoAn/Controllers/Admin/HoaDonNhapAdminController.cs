using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.Admin
{
    public class HoaDonNhapAdminController : Controller
    {
        // GET: HoaDonNhapAdmin
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var model = new List<HoaDonNhapModel>();
                var list = db.HoaDonNhaps.ToList();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var itemmodel = new HoaDonNhapModel();
                    itemmodel.Id = item.Id;
                    itemmodel.STT = i;
                    itemmodel.TenNCC = new NhaCungCapDao().getById(item.MaNCC).TenNCC;
                    itemmodel.TenDangNhap = new KhachHangDao().viewDetail(item.MaNhanVien).TenDangNhap;
                    itemmodel.TongTien = item.TongTien;
                    itemmodel.NgayNhap = item.NgayNhap;
                    itemmodel.ChietKhau = String.Format("{0:0,0}", item.ChietKhau);
                    model.Add(itemmodel);

                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        public ActionResult CreateHDN()
        {

            var session_mahdn = (DoAn.Common.Session.MaHoaDonNhap)Session[DoAn.Common.Constants.MAHDN_SESSION];
            var list_cthdn = new List<CTHDNhapModel>();
            var model = new HoaDonNhapModel();
            if (session_mahdn != null)
            {
                var list = db.ChiTietHDNs.Where(x => x.MaHDN == session_mahdn.Id).ToList();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var item_cthdn = new CTHDNhapModel();
                    item_cthdn.Id = item.Id;
                    item_cthdn.STT = i;
                    item_cthdn.TenNguyenLieu = new NguyenLieuDao().getByid(item.MaNguyenLieu).TenNguyenLieu;
                    item_cthdn.TenDonViTinh = new NguyenLieuDao().getByid(item.MaNguyenLieu).DonViTinh;
                    item_cthdn.SoLuong = item.SoLuong.ToString();
                    item_cthdn.GiaNhap = item.GiaNhap.ToString();
                    item_cthdn.ChietKhau = item.ChietKhau.ToString();
                    item_cthdn.ThanhTien = item.ThanhTien;
                    list_cthdn.Add(item_cthdn);
                }
                var tongtienhdn = db.ChiTietHDNs.Where(x => x.MaHDN == session_mahdn.Id).Sum(x => x.ThanhTien);
                model.TongTien = tongtienhdn;
            }
            else
            {
                list_cthdn = null;
            }
            ViewBag.ListChiTietHDN = list_cthdn;
            model.SelectNCC = new SelectList(db.NhaCungCaps, "Id", "TenNCC", 0);
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateHDN(HoaDonNhapModel model)
        {
            if (ModelState.IsValid)
            {
                var session_hdn = (DoAn.Common.Session.MaHoaDonNhap)Session[DoAn.Common.Constants.MAHDN_SESSION];
                var hoadonnhap = new HoaDonNhap();
                hoadonnhap.Id = session_hdn.Id;
                hoadonnhap.MaNCC = model.MaNhaCungCap;
                hoadonnhap.MaNhanVien = 3;
                DateTime now = DateTime.Now;
                hoadonnhap.NgayNhap = now;
                hoadonnhap.TongTien = db.ChiTietHDNs.Where(x => x.MaHDN == session_hdn.Id).Sum(x => x.ThanhTien);
                if (model.ChietKhau == null)
                {
                    hoadonnhap.ChietKhau = 0;
                }
                else
                {
                    hoadonnhap.ChietKhau = new DoAn.Common.Function.ConvertMoney().ConvertTien(model.ChietKhau);
                }
                db.HoaDonNhaps.Add(hoadonnhap);
                Session[DoAn.Common.Constants.MAHDN_SESSION] = null;
                db.SaveChanges();
                return RedirectToAction("Index", "HoaDonNhapAdmin");
            }
            else
            {
                var session_mahdn = (DoAn.Common.Session.MaHoaDonNhap)Session[DoAn.Common.Constants.MAHDN_SESSION];
                var list_cthdn = new List<CTHDNhapModel>();
                var viewmodel = new HoaDonNhapModel();
                if (session_mahdn != null)
                {
                    var list = db.ChiTietHDNs.Where(x => x.MaHDN == session_mahdn.Id).ToList();
                    int i = 0;
                    foreach (var item in list)
                    {
                        i++;
                        var item_cthdn = new CTHDNhapModel();
                        item_cthdn.Id = item.Id;
                        item_cthdn.STT = i;
                        item_cthdn.TenNguyenLieu = new NguyenLieuDao().getByid(item.MaNguyenLieu).TenNguyenLieu;
                        item_cthdn.TenDonViTinh = new NguyenLieuDao().getByid(item.MaNguyenLieu).DonViTinh;
                        
                        item_cthdn.SoLuong = item.SoLuong.ToString();
                        item_cthdn.GiaNhap = item.GiaNhap.ToString();
                        item_cthdn.ChietKhau = item.ChietKhau.ToString();
                        item_cthdn.ThanhTien = item.ThanhTien;
                        list_cthdn.Add(item_cthdn);
                    }
                    var tongtienhdn = db.ChiTietHDNs.Where(x => x.MaHDN == session_mahdn.Id).Sum(x => x.ThanhTien);
                    model.TongTien = tongtienhdn;
                }
                else
                {
                    list_cthdn = null;
                }
                model.SelectNCC = new SelectList(db.NhaCungCaps, "Id", "TenNCC", 0);
                ViewBag.ListChiTietHDN = list_cthdn;
                return View(model);
            }
        }
        public ActionResult XemChiTietHDN(string mahoadon)
        {
            var model = new List<CTHDNhapModel>();
            var list = db.ChiTietHDNs.Where(x => x.MaHDN == mahoadon).ToList();
            int i = 0;
            foreach (var item in list)
            {
                var itemmodel = new CTHDNhapModel();
                i++;
                itemmodel.TenNguyenLieu = new NguyenLieuDao().getByid(item.MaNguyenLieu).TenNguyenLieu;
                itemmodel.TenDonViTinh = new NguyenLieuDao().getByid(item.MaNguyenLieu).DonViTinh;
                //itemmodel.TenDonViTinh = new DonViTinhDao().getByid(item.MaDonViTinh).TenDonViTinh;
                itemmodel.GiaNhap = item.GiaNhap.ToString();
                itemmodel.SoLuong = item.SoLuong.ToString();
                itemmodel.ChietKhau = item.ChietKhau.ToString();
                itemmodel.ThanhTien = item.ThanhTien;
                itemmodel.STT = i;
                model.Add(itemmodel);
            }
            var hoadonnhap = db.HoaDonNhaps.FirstOrDefault(x => x.Id == mahoadon);
            var hdnmodel = new HoaDonNhapModel();
            hdnmodel.TenDangNhap = db.KhachHangs.FirstOrDefault(x => x.Id == hoadonnhap.MaNhanVien).TenDangNhap;
            hdnmodel.NgayNhap = hoadonnhap.NgayNhap;
            hdnmodel.TenNCC = db.NhaCungCaps.FirstOrDefault(x => x.Id == hoadonnhap.MaNCC).TenNCC;
            hdnmodel.TongTien = hoadonnhap.TongTien;
            hdnmodel.ChietKhau = String.Format("{0:0,0}", hoadonnhap.ChietKhau);
            var chietkhau = new DoAn.Common.Function.ConvertMoney().ConvertTien(hdnmodel.ChietKhau);
            hdnmodel.PhaiTra = (hdnmodel.TongTien - chietkhau);
            ViewBag.HoaDonNhap = hdnmodel;
            return View(model);
        }
        public ActionResult CreateCTHDN()
        {
            var model = new CTHDNhapModel();
            model.SelectNguyenLieu = new SelectList(db.NguyenLieux, "Id", "TenNguyenLieu", 0);
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateCTHDN(CTHDNhapModel model)
        {
            if (ModelState.IsValid)
            {
                var cthdn = new ChiTietHDN();
                var session_hdn = (DoAn.Common.Session.MaHoaDonNhap)Session[DoAn.Common.Constants.MAHDN_SESSION];
                if (session_hdn != null)
                {
                    cthdn.MaHDN = session_hdn.Id;
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    var mahdn = dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString();
                    var session_mahdnhap = new DoAn.Common.Session.MaHoaDonNhap();
                    session_mahdnhap.Id = mahdn;
                    Session.Add(DoAn.Common.Constants.MAHDN_SESSION, session_mahdnhap);
                    cthdn.MaHDN = mahdn;
                }

                cthdn.MaDonViTinh = model.MaDonViTinh;
                cthdn.MaNguyenLieu = model.MaNguyenLieu;
                
                cthdn.SoLuong = new DoAn.Common.Function.ConvertMoney().ConvertTien(model.SoLuong);
                cthdn.GiaNhap = new DoAn.Common.Function.ConvertMoney().ConvertTien(model.GiaNhap);
                cthdn.ChietKhau =new DoAn.Common.Function.ConvertMoney().ConvertTien(model.ChietKhau);
                cthdn.ThanhTien = cthdn.GiaNhap * cthdn.SoLuong - cthdn.ChietKhau;
                db.ChiTietHDNs.Add(cthdn);

                
               
                var nguyenlieu = db.NguyenLieux.Find(model.MaNguyenLieu);
                nguyenlieu.GiaNhap =new DoAn.Common.Function.ConvertMoney().ConvertTien(model.GiaNhap);
                nguyenlieu.SoLuong = nguyenlieu.SoLuong + new DoAn.Common.Function.ConvertMoney().ConvertTien( model.SoLuong) ;
                
                db.SaveChanges();

                return RedirectToAction("CreateHDN", "HoaDonNhapAdmin");
            }
            else
            {
                var viewmodel = new CTHDNhapModel();
                viewmodel.SelectNguyenLieu = new SelectList(db.NguyenLieux, "Id", "TenNguyenLieu", model.MaNguyenLieu);
                viewmodel.SoLuong = model.SoLuong;

                return View(viewmodel);
            }

        }
        public ActionResult SuaCTHDN(int id, string soluong, string gianhap, string chietkhau)
        {
            var cthdn = db.ChiTietHDNs.FirstOrDefault(x => x.Id == id);
            var convert = new DoAn.Common.Function.ConvertMoney();
            cthdn.SoLuong = convert.ConvertTien(soluong);
            cthdn.GiaNhap = convert.ConvertTien(gianhap);
            cthdn.ChietKhau = convert.ConvertTien(chietkhau);
            cthdn.ThanhTien = cthdn.SoLuong * cthdn.GiaNhap - cthdn.ChietKhau;
            db.SaveChanges();
            return RedirectToAction("CreateHDN", "HoaDonNhapAdmin");
        }
        public ActionResult XoaCTHDN(int id)
        {
            var chitiethdn = db.ChiTietHDNs.Find(id);
            var hoadonnhap = db.HoaDonNhaps.FirstOrDefault(x => x.Id == chitiethdn.MaHDN);
            var nguyenlieu = db.NguyenLieux.Find(chitiethdn.MaNguyenLieu);
            nguyenlieu.SoLuong = nguyenlieu.SoLuong - chitiethdn.SoLuong;
            db.ChiTietHDNs.Remove(chitiethdn);
            db.SaveChanges();
            return RedirectToAction("CreateHDN", "HoaDonNhapAdmin");
        }
    }
}