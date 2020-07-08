using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class CartDao
    {
        TraSuaEntities db = null;
        public CartDao()
        {
            db = new TraSuaEntities();

        }
        public List<GioHang> GetProductsByIdUser(int userid)
        {
            return (db.GioHangs.Where(x => x.MaKhachHang == userid).ToList());
        }
        public int Insert(GioHang cart)
        {
            db.GioHangs.Add(cart);
            db.SaveChanges();
            return (cart.Id);
        }
        public void Update(int cartid, int? soluong)
        {
            var cart = db.GioHangs.Find(cartid);
            cart.SoLuong = soluong;
            db.SaveChanges();

        }
        public void Delete(int cartid)
        {
            var cart = db.GioHangs.Find(cartid);
            db.GioHangs.Remove(cart);
            db.SaveChanges();

        }
        public List<GioHang> layDSSP(int productid, int userid, int sanphamthu)
        {
            return db.GioHangs.Where(x => x.ThuocSanPham == productid && x.MaKhachHang == userid && x.SanPhamThu==sanphamthu).ToList();
        }
       
        public int? Tien1LyTraSua(int userid, int thuocsanpham, int? sanphamthu)
        {
            var list = db.GioHangs.Where(x => x.ThuocSanPham == thuocsanpham && x.MaKhachHang == userid && x.SanPhamThu==sanphamthu).ToList();
            int? tien = 0;
            foreach (var item in list)
            {
                var product = new ProductDao().getByid(item.MaSanPham);
                tien += product.KhuyenMai;
            }
            return tien;
        }
        public string getMoTa(int masanpham, int userid, int? sanphamthu)
        {
            var mota = "";
            var list = db.GioHangs.Where(x => x.ThuocSanPham == masanpham && x.MaKhachHang == userid && x.SanPhamThu==sanphamthu).ToList();
           
            foreach (var item in list)
            {
                var categorydao = new CategoryDao();
                if (categorydao.getSPChinh(item.MaSanPham) != 1)
                {
                    mota += new ProductDao().getByid(item.MaSanPham).TenSanPham.ToString() + ", ";
                }

            }
           
            
            return mota;
        }
    }
}