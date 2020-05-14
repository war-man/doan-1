using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAn.Models.Model.Admin;

namespace DoAn.Models.Dao.Admin
{
    public class ProductDao
    {
        TraSuaEntities db = null;
        public ProductDao()
        {
            db = new TraSuaEntities();
        }
        
        public SanPham getByid(int? productid)
        {
            return db.SanPhams.SingleOrDefault(x => x.Id == productid);
        }
        public SanPham FindSanPham(int? productid)
        {
            return (db.SanPhams.Find(productid));
        }
        public int AddSanPham(SanPhamModel sp)
        {
            var sanpham = new SanPham();
            var convert = new DoAn.Common.Function.ConvertMoney();
            sanpham.TenSanPham = sp.Ten;
            sanpham.MaLoaiSanPham = sp.MaLoai;
            sanpham.GiaBan = convert.ConvertTien(sp.GiaBan);
            sanpham.KhuyenMai = sanpham.GiaBan;
            sanpham.Anh = sp.Anh;
            db.SanPhams.Add(sanpham);
            db.SaveChanges();
            return sanpham.Id;
        }
        public int updateSanPham(SanPhamModel sp)
        {
            var sanpham = db.SanPhams.Find(sp.Id);
            var convert = new DoAn.Common.Function.ConvertMoney();
            sanpham.TenSanPham = sp.Ten;
            sanpham.Anh = sp.Anh;
            sanpham.GiaBan = convert.ConvertTien(sp.GiaBan);
            sanpham.MaLoaiSanPham = sp.MaLoai;
            db.SaveChanges();
            return sanpham.Id;
        }
        public int Delete(int masanpham)
        {
            var product = db.SanPhams.Find(masanpham);
            db.SanPhams.Remove(product);
            db.SaveChanges();
            return product.Id;
        }
    }
}