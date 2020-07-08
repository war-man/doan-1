using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.NguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class CartController : Controller
    {
        // GET: Cart
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            var list = new List<GioHangModel>();
            if (session != null)
            {
                var cart = new CartDao().GetProductsByIdUser(session.UserId);
                foreach (var item in cart)
                {
                    var cartitem = new GioHangModel();

                    var product = new ProductDao().viewDetail(item.MaSanPham);
                    var categorydao = new CategoryDao();
                    if (categorydao.getSPChinh(product.Id) == 1)
                    {
                        cartitem.MaSanPham = item.MaSanPham;
                        cartitem.Id = item.Id;
                        cartitem.TenSanPham = product.TenSanPham;
                        cartitem.Anh = product.Anh;
                        cartitem.SoLuong = item.SoLuong;
                        cartitem.SanPhamThu = item.SanPhamThu;
                        cartitem.GiaBan = new CartDao().Tien1LyTraSua(session.UserId, product.Id, item.SanPhamThu);
                        cartitem.ThanhTien = cartitem.GiaBan * cartitem.SoLuong;
                        cartitem.MoTa = new CartDao().getMoTa(product.Id, session.UserId, item.SanPhamThu);
                        list.Add(cartitem);
                    }

                }
                return View(list);
            }
            else
            {
                return View("/SanPham/Index");
            }

        }
        public ActionResult CreateCart(string listproduct)
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            string[] productsid = listproduct.Split(',');
            var session_spthu = (DoAn.Common.Session.SanPhamThuSession)Session[DoAn.Common.Constants.SANPHAMTHU_SESSION];
            int sp_thu = 0;
            if (session_spthu != null)
            {
                sp_thu = session_spthu.SanPham_Thu + 1;
            }
            else
            {
                sp_thu = 1;

            }
            var sanpham_thu = new DoAn.Common.Session.SanPhamThuSession();
            sanpham_thu.SanPham_Thu = sp_thu;
            Session.Add(DoAn.Common.Constants.SANPHAMTHU_SESSION, sanpham_thu);

            foreach (var item in productsid)
            {

                GioHang giohang = new GioHang();
                giohang.MaKhachHang = session.UserId;
                giohang.MaSanPham = int.Parse(item);
                giohang.SoLuong = 1;
                giohang.ThuocSanPham = int.Parse(productsid[0]);
                giohang.SanPhamThu = sp_thu;
                new CartDao().Insert(giohang);

            }
            return Redirect("/SanPham/Index");
        }
        public ActionResult UpdateCart(int productid, int? soluong, int sanphamthu)
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var userid = session.UserId;
                var list = new CartDao().layDSSP(productid, userid, sanphamthu);
                foreach (var item in list)
                {
                    var cart = new GioHang();
                    new CartDao().Update(item.Id, soluong);
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult DeleteCart(int productid, int sanphamthu)
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var userid = session.UserId;
                var list = new CartDao().layDSSP(productid, userid, sanphamthu);
                int? spthu = 0;
                foreach (var item in list)
                {
                    new CartDao().Delete(item.Id);
                    spthu = item.SanPhamThu;
                }
                var listconlai = new CartDao().GetProductsByIdUser(userid);
                foreach(var item in listconlai)
                {
                    if(spthu < item.SanPhamThu)
                    {
                        var sanpham_giohang = db.GioHangs.Find(item.Id);
                        sanpham_giohang.SanPhamThu = item.SanPhamThu - 1;
                        db.SaveChanges();
                    }
                    
                }
                var session_sanphamthu = (Common.Session.SanPhamThuSession)Session[Common.Constants.SANPHAMTHU_SESSION];
                var sp_thu = new Common.Session.SanPhamThuSession();
                sp_thu.SanPham_Thu = session_sanphamthu.SanPham_Thu - 1;
                Session.Add(Common.Constants.SANPHAMTHU_SESSION, sp_thu);
            }
            return RedirectToAction("Index");
        }
    }
}