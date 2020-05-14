using DoAn.Common.Function;
using DoAn.Common.Session;
using DoAn.Models.Dao.Admin;
using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class LoginController : Controller
    {
        // GET: Login
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult Index()
        {
            
            return View();

        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.Username, Encrytor.MD5Hash(model.Password));
                if (result)
                {
                    var user = dao.GetById(model.Username);
                    var userSession = new UserLogin();
                    userSession.UserId = user.Id;
                    userSession.UserName = user.TenDangNhap;
                    Session.Add(Common.Constants.USER_SESSION, userSession);
                   
                    if (user.TenDangNhap == "admin")
                    {
                        return RedirectToAction("Index", "HomeAdmin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng");
                }
            }
            return View("Index");
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(Models.Model.NguoiDung.KhachHangModel model)
        {

            var dao = new UserDao();
            if (dao.CheckUserName(model.TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
            }
            else
             if (dao.CheckEmail(model.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại");
            }
            else
            {
                var user = new KhachHang();
                user.HoTen = model.HoTen;
                user.TenDangNhap = model.TenDangNhap;
                user.Email = model.Email;
                user.MatKhau = Encrytor.MD5Hash(model.MatKhau);
                user.SDT = model.SDT;
                user.DiaChi = model.DiaChi;

                var result = dao.Insert(user);

                if (result > 0)
                {
                    ViewBag.Success = "Đăng ký thành công";
                }
            }


            return View();
        }
        public ActionResult Logout()
        {
            Session[DoAn.Common.Constants.USER_SESSION] = null; 
            Session[DoAn.Common.Constants.BILL_SESSION] = null;
            Session[DoAn.Common.Constants.CTTHU_SESSION] = null;
            Session[DoAn.Common.Constants.SANPHAMTHU_SESSION] = null;
            Session[DoAn.Common.Constants.NHANVIEN_SESSION] = null;
            return Redirect("/Home/Index");
        }
        public ActionResult QuenMatKhau()
        {

            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(Models.Model.NguoiDung.KhachHangModel model)
        {

            var dao = new DoAn.Models.Dao.NguoiDung.KhachHangDao();
            if (dao.CheckEmail(model.Email, model.TenDangNhap))
            {
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/resetpassword.html"));
                    content = content.Replace("{{CustomerName}}", model.TenDangNhap);
                    content = content.Replace("{{Password}}", "@123456");
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(model.Email, "Đổi mật khẩu từ Estore", content);
                    var khachhang = dao.getKhachHang(model.Email, model.TenDangNhap);
                    var customer = db.KhachHangs.Find(khachhang.Id);
                    customer.MatKhau = DoAn.Common.Function.Encrytor.MD5Hash("@123456");
                    db.SaveChanges();
                    ViewBag.DoiMatKhau = "Mật khẩu bạn đã được gửi đến gmail, mời bạn kiểm tra email";
                }

                catch
                {
                    ViewBag.Error = "Email bạn nhập không hợp lệ";
                }


                
            }
            else
            {
                ViewBag.SaiEmail = "Tên đăng nhập và email không khớp";
            }

            return View();
        }
        public ActionResult DangNhapNhanVien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapNhanVien(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new Models.Dao.NguoiDung.NhanVienDao();
                var result = dao.DangNhapNhanVien(model.Username, Encrytor.MD5Hash(model.Password));
                if (result)
                {
                    var nhanvien = dao.getByName(model.Username);
                    var nhanvien_Session =new NhanVienSession();
                    nhanvien_Session.Id = nhanvien.Id;
                    nhanvien_Session.UserName = nhanvien.TenDangNhap;
                    nhanvien_Session.MaChiNhanh = nhanvien.MaChiNhanh;
                    Session.Add(Common.Constants.NHANVIEN_SESSION, nhanvien_Session);
                    if(nhanvien.MaChucVu == 3)
                    {
                        return RedirectToAction("Index", "HomeQuanLy");
                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeNhanVien");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng");
                }
            }
            return RedirectToAction("DangNhapNhanVien", "Login");
        }
        public ActionResult QuenMatKhauNhanVien()
        {

            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhauNhanVien(Models.Model.NhanVien.NhanVienModel model)
        {
            var dao = new DoAn.Models.Dao.NhanVien.NhanVienDao();
            if (dao.CheckEmail(model.Email, model.TenDangNhap))
            {
                try
                {
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/resetpassword.html"));
                    content = content.Replace("{{CustomerName}}", model.TenDangNhap);
                    content = content.Replace("{{Password}}", "@123456");
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(model.Email, "Đổi mật khẩu từ Estore", content);
                    var nhanvien = dao.getNhanVien(model.Email, model.TenDangNhap);
                    var nv = db.NhanViens.Find(nhanvien.Id);
                    nv.MatKhau = DoAn.Common.Function.Encrytor.MD5Hash("@123456");
                    db.SaveChanges();
                    ViewBag.DoiMatKhau = "Mật khẩu bạn đã được gửi đến gmail, mời bạn kiểm tra email";
                }

                catch
                {
                    ViewBag.Error = "Email bạn nhập không hợp lệ";
                }



            }
            else
            {
                ViewBag.SaiEmail = "Tên đăng nhập và email không khớp";
            }

            return View();
        }
    }
}