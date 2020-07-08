using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Common.Session;
using DoAn.Models.Model.NhanVien;

namespace DoAn.Controllers.NguoiDung
{
    public class HomeController : Controller
    {
        // GET: Home
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                if(cart != null)
                {
                    var countitem = cart.Max(x => x.SanPhamThu);
                    var session_sanphamthu = new SanPhamThuSession();
                    session_sanphamthu.SanPham_Thu =Convert.ToInt32( countitem);
                    Session.Add(DoAn.Common.Constants.SANPHAMTHU_SESSION, session_sanphamthu);
                }
            }
            return View();
        }
        public PartialViewResult TenDangNhap()
        {
            return PartialView();
        }
        public ActionResult ThongTinTaiKhoan()
        {
            var model = new KhachHangModel();
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            var khachhang = new KhachHangDao().getById(session.UserId);
            model.Id = khachhang.Id;
            model.HoTen = khachhang.HoTen;
            model.Email = khachhang.Email;
            model.SDT = khachhang.SDT;
            model.DiaChi = khachhang.DiaChi;
            model.UserName = khachhang.TenDangNhap;
            return View(model);
        }
        [HttpPost]
        public ActionResult ThongTinTaiKhoan(KhachHangModel model)
        {
            if (!ModelState.IsValid)
            {
                var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
                var khachhang = db.KhachHangs.Find(session.UserId);
                khachhang.HoTen = model.HoTen;
                khachhang.DiaChi = model.DiaChi;
                khachhang.SDT = model.SDT;
                db.SaveChanges();

                var viewmodel = new KhachHangModel();
                viewmodel.Id = khachhang.Id;
                viewmodel.HoTen = khachhang.HoTen;
                viewmodel.Email = khachhang.Email;
                viewmodel.SDT = khachhang.SDT;
                viewmodel.DiaChi = khachhang.DiaChi;
                viewmodel.UserName = khachhang.TenDangNhap;
                ViewBag.ThongTinTaiKhoan = "Bạn đã thay đổi thông tin thành công";
                return View(viewmodel);
            }
            else
            {
                var viewmodel = new KhachHangModel();
                viewmodel.Id = model.Id;
                viewmodel.HoTen = model.HoTen;
                viewmodel.Email = model.Email;
                viewmodel.SDT = model.SDT;
                viewmodel.DiaChi = model.DiaChi;
                viewmodel.UserName = model.TenDangNhap;
                ViewBag.ThongTinTaiKhoan = "Bạn đã thay đổi thông tin thành công";
                return View(viewmodel);
            }
            
        }
        public ActionResult MuaNgay(int productid)
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                ProductDao dao = new ProductDao();
                ViewBag.Topping = dao.getTopping();
                ViewBag.Duong = dao.getDuong();
                ViewBag.Da = dao.getDa();
                ViewBag.Size = dao.getSize();
                var product = dao.getByid(productid);
                return View(product);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public PartialViewResult GioHang()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                var countitem = cart.Max(x => x.SanPhamThu);
                ViewBag.CountItem = countitem;
                return PartialView(cart);
            }
            return PartialView();

        }
        public ActionResult DoiMatKhau()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            ViewBag.TenDangNhap = session.UserName;
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(DoiMatKhauNguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
                if (session != null)
                {
                    var khachhang = db.KhachHangs.Find(session.UserId);
                    if(khachhang.MatKhau == Common.Function.Encrytor.MD5Hash(model.MatKhauCu))
                    {
                        khachhang.MatKhau = DoAn.Common.Function.Encrytor.MD5Hash(model.MatKhau);
                        db.SaveChanges();
                        ViewBag.DoiMatKhau = "Bạn đã đổi mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu không đúng";
                    }
                    
                }
            }
            return View();
        }
        public ActionResult PhanHoi(string noidung)
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            var phanhoi = new PhanHoi();
            DateTime now = DateTime.Now;
            phanhoi.NoiDung = noidung;
            phanhoi.ThoiGian = now;
            phanhoi.MaKhachHang = session.UserId;
            phanhoi.DaXem = 0;
            db.PhanHois.Add(phanhoi);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DanhSachHoaDon_KhachHang()
        {
            var session = (Common.Session.UserLogin)Session[Common.Constants.USER_SESSION];
            var lstHoaDon = db.HoaDonBans.Where(x => x.MaKhach == session.UserId).ToList();
            var model = new List<Models.Model.NguoiDung.HoaDonBanModel>();
            foreach(var item in lstHoaDon)
            {
                var itemmodel = new Models.Model.NguoiDung.HoaDonBanModel();
                itemmodel.Id = item.Id;
                itemmodel.NgayBan = item.NgayBan;
                itemmodel.TongSoSanPham = new Models.Dao.NguoiDung.BillDao().tongsoluong(item.Id);
                itemmodel.DiaChi = item.DiaChi;
                itemmodel.TongTien = item.TongTien;
                itemmodel.Status = item.Duyet;
                itemmodel.PhiShip = item.PhiShip;
                itemmodel.TongTienHoaDon = item.TongTien_HoaDon;
                itemmodel.DaThanhToan = item.DaThanhToan;
                model.Add(itemmodel);
            }
            return View(model);
        }
        public ActionResult ChiTietHoaDon_NguoiDung(string mahoadon)
        {
            int tinhtrang = 0;
            var hoadonban = db.HoaDonBans.FirstOrDefault(x => x.Id == mahoadon);
            if(hoadonban.Duyet == 0)
            {
                tinhtrang = 1;
            }
            else if(hoadonban.Duyet ==1 && hoadonban.DaThanhToan ==0){
                tinhtrang = 2;
            }
            else if(hoadonban.DaThanhToan ==1 && hoadonban.DaThanhToan==1)
            {
                tinhtrang = 3;
            }
            ViewBag.TinhTrang = tinhtrang;
            ViewBag.PhiShip = hoadonban.PhiShip;
            ViewBag.TongTien_HoaDonBan = hoadonban.TongTien_HoaDon;
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
            return View(model);
        }
    }
}