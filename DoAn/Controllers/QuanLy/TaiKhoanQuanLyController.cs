using DoAn.Models.EF;
using DoAn.Models.Model.NhanVien;
using DoAn.Models.Model.QuanLy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.QuanLy
{
    public class TaiKhoanQuanLyController : Controller
    {
        // GET: TaiKhoanQuanLy
        TraSuaEntities db = new TraSuaEntities();

        public ActionResult ThongTinTaiKhoan()
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            if (session_nhanvien != null)
            {
                var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == session_nhanvien.Id);
                var model = new NhanVienModel();
                model.Id = nhanvien.Id;
                model.DiaChi = nhanvien.DiaChi;
                model.HoTen = nhanvien.HoTen;
                model.SDT = nhanvien.SDT;

                model.TenDangNhap = nhanvien.TenDangNhap;
                model.Luong = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu).Luong;
                model.TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu).TenChucVu;
                model.TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh).TenChiNhanh;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult ThongTinTaiKhoan(NhanVienModel model)
        {

            if (!ModelState.IsValid)
            {
                var nhanvien = db.NhanViens.Find(model.Id);
                nhanvien.HoTen = model.HoTen;
                nhanvien.DiaChi = model.DiaChi;
                nhanvien.SDT = model.SDT;
                db.SaveChanges();

                var viewmodel = new NhanVienModel();
                viewmodel.TenDangNhap = model.TenDangNhap;
                viewmodel.Id = model.Id;
                viewmodel.DiaChi = model.DiaChi;
                viewmodel.HoTen = model.HoTen;
                viewmodel.SDT = model.SDT;


                viewmodel.Luong = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu).Luong;
                viewmodel.TenDangNhap = nhanvien.TenDangNhap;
                viewmodel.TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu).TenChucVu;
                viewmodel.TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh).TenChiNhanh;

                ViewBag.Success = "Bạn đã thay đổi thông tin thành công";
                return View(viewmodel);
            }
            else
            {
                var nhanvien = db.NhanViens.Find(model.Id);
                var viewmodel = new NhanVienModel();
                viewmodel.Id = model.Id;
                viewmodel.DiaChi = model.DiaChi;
                viewmodel.HoTen = model.HoTen;
                viewmodel.SDT = model.SDT;
                viewmodel.Luong = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu).Luong;
                viewmodel.TenDangNhap = nhanvien.TenDangNhap;
                viewmodel.TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == nhanvien.MaChucVu).TenChucVu;
                viewmodel.TenChiNhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == nhanvien.MaChiNhanh).TenChiNhanh;
                return View(viewmodel);
            }
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(DoiMatKhauQuanLy model)
        {
            if (ModelState.IsValid)
            {
                var session = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
                if (session != null)
                {
                    var nhanvien = db.NhanViens.Find(session.Id);
                    if (nhanvien.MatKhau == Common.Function.Encrytor.MD5Hash(model.MatKhauCu))
                    {
                        nhanvien.MatKhau = DoAn.Common.Function.Encrytor.MD5Hash(model.MatKhau);
                        db.SaveChanges();
                        ViewBag.Success = "Bạn đã đổi mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu không đúng";
                    }

                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session[DoAn.Common.Constants.NHANVIEN_SESSION] = null;
            Session[DoAn.Common.Constants.BILL_SESSION] = null;
            Session[DoAn.Common.Constants.CTTHU_SESSION] = null;
            Session[DoAn.Common.Constants.SANPHAMTHU_SESSION] = null;
            Session[DoAn.Common.Constants.USER_SESSION] = null;
            return Redirect("/Home/Index");
        }
    }
}