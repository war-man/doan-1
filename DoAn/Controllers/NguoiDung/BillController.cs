using DoAn.Common.Function;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Models.Dao.NguoiDung
{
    public class BillController : Controller
    {
        // GET: Bill
        TraSuaEntities db = new TraSuaEntities();
        // GET: Bill
        public ActionResult CreateBill(int tongtien)
        {
            ViewBag.TotalMoney = tongtien;
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            var khachhang = new KhachHangDao().getById(session.UserId);
            var model = new DoAn.Models.Model.NguoiDung.HoaDonBanModel();
            model.MaKhach = khachhang.Id;
            model.TongTien = tongtien;
            model.DiaChi = khachhang.DiaChi;
            model.HoTen = khachhang.HoTen;
            model.SDT = khachhang.SDT;
            model.Email = khachhang.Email;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBill(HoaDonBanModel model)
        {
            if (ModelState.IsValid)
            {
                var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
                if (session != null)
                {

                    new KhachHangDao().Update(session.UserId, model.HoTen, model.DiaChi, model.SDT, model.Email);
                    // tạo idbill
                    DateTime now = DateTime.Now;
                    var idbill = session.UserId.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
                    //insert order
                    var dao = new CartDao();
                    var cart = dao.GetProductsByIdUser(session.UserId);
                    var item = new HoaDonBan();
                    item.Id = idbill.ToString();
                    item.MaKhach = session.UserId;
                    item.MaNhanVien = 1;
                    item.DaThanhToan = 0;
                    item.Duyet = 0;
                    item.TongTien = model.TongTien;


                    item.NgayBan = now;

                    var result = new BillDao().Insert(item);

                    foreach (var item_hdb in cart)
                    {
                        //insert orderdetail
                        var ct_hdb = new ChiTietHDB();
                        ct_hdb.MaHDB = idbill.ToString();
                        var product = new ProductDao().viewDetail(item_hdb.MaSanPham);
                        ct_hdb.MaSanPham = item_hdb.MaSanPham;
                        ct_hdb.SoLuong = item_hdb.SoLuong;
                        ct_hdb.GiamGia = 0;
                        ct_hdb.ThanhTien = product.KhuyenMai * item_hdb.SoLuong;
                        ct_hdb.ThuocSanPham = item_hdb.ThuocSanPham;
                        ct_hdb.ChiTietThu = item_hdb.SanPhamThu;
                        new BillDao().Insert_Bill_Detail(ct_hdb);
                        //delete cart
                        dao.Delete(item_hdb.Id);
                    }


                    var khachhang = new KhachHangDao().getById(session.UserId);
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/neworder.html"));
                    content = content.Replace("{{CustomerName}}", khachhang.HoTen);
                    content = content.Replace("{{Phone}}", khachhang.SDT);
                    content = content.Replace("{{Email}}", khachhang.Email);
                    content = content.Replace("{{Address}}", khachhang.DiaChi);
                    content = content.Replace("{{Total}}", String.Format("{0:0,0}", model.TongTien));

                    try
                    {
                        //var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                        //new MailHelper().SendMail(khachhang.Email, "Đơn hàng mới từ Estore", content);
                        //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Estore", content);


                        ViewBag.TotalMoney = model.TongTien;

                        Session[DoAn.Common.Constants.SANPHAMTHU_SESSION] = null;
                       
                        var viewmodel = new DoAn.Models.Model.NguoiDung.HoaDonBanModel();
                        viewmodel.MaKhach = khachhang.Id;
                        viewmodel.TongTien = model.TongTien;
                        viewmodel.DiaChi = khachhang.DiaChi;
                        viewmodel.HoTen = khachhang.HoTen;
                        viewmodel.SDT = khachhang.SDT;
                        viewmodel.Email = khachhang.Email;
                       
                        ViewBag.Success = "Bạn vừa đặt hàng thành công, kiếm tra email của bạn";
                        return View(viewmodel);
                    }
                    catch (Exception e)
                    {
                        ViewBag.TotalMoney = model.TongTien;

                        var viewmodel = new DoAn.Models.Model.NguoiDung.HoaDonBanModel();
                        viewmodel.MaKhach = khachhang.Id;
                        viewmodel.TongTien = model.TongTien;
                        viewmodel.DiaChi = khachhang.DiaChi;
                        viewmodel.HoTen = khachhang.HoTen;
                        viewmodel.SDT = khachhang.SDT;
                        viewmodel.Email = khachhang.Email;
                        ViewBag.Error = "Gmail bạn nhập không chính xác, vui lòng kiểm tra lại";
                        return View(viewmodel);
                    }
                    
                }
                else
                {
                    return Redirect("/Login/Login");
                }
            }
            else
            {
                ViewBag.TotalMoney = model.TongTien;
                var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
                var khachhang = new KhachHangDao().getById(session.UserId);
                var modelview = new DoAn.Models.Model.NguoiDung.HoaDonBanModel();
                modelview.MaKhach = khachhang.Id;
                modelview.TongTien = model.TongTien;
                modelview.DiaChi = khachhang.DiaChi;
                modelview.HoTen = khachhang.HoTen;
                modelview.SDT = khachhang.SDT;
                modelview.Email = khachhang.Email;
                return View(modelview);
                
            }
            
        }
    }
}