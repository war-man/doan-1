using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class KhuyenMaiDao
    {
        TraSuaEntities db = null;
        public KhuyenMaiDao()
        {
            db = new TraSuaEntities();
        }

        public DoAn.Models.EF.KhuyenMai getByid(int makhuyenmai)
        {
            return db.KhuyenMais.SingleOrDefault(x => x.Id == makhuyenmai);
        }
        public DoAn.Models.EF.KhuyenMai FindSanPham(int makhuyenmai)
        {
            return (db.KhuyenMais.Find(makhuyenmai));
        }
        public int AddKhuyenMai(KhuyenMaiModel km)
        {
            var khuyenmai = new DoAn.Models.EF.KhuyenMai();
            khuyenmai.NgayBatDau = km.NgayBatDau;
            khuyenmai.NgayKetThuc = km.NgayKetThuc;
            khuyenmai.TenKhuyenMai = km.Ten;
            khuyenmai.MoTa = km.MoTa;
            khuyenmai.PhanTram = km.PhanTram;
            db.KhuyenMais.Add(khuyenmai);

            var motamodel = km.MoTa;
            string[] lstloaisp = motamodel.Split(',');
            foreach (var itemlsp in lstloaisp)
            {
                var maloaisp = Convert.ToInt32(itemlsp);
                var lstsanpham = db.SanPhams.Where(x => x.MaLoaiSanPham == maloaisp).ToList();
                foreach(var item in lstsanpham)
                {
                    var sanpham = db.SanPhams.Find(item.Id);
                    sanpham.KhuyenMai = sanpham.GiaBan - sanpham.GiaBan * khuyenmai.PhanTram/100;
                    db.SaveChanges();
                }
            }


            db.SaveChanges();
            return khuyenmai.Id;
        }
        public int UpdateKhuyenMai(int makhuyenmai)
        {
            var khuyenmai = db.KhuyenMais.Find(makhuyenmai);
            khuyenmai.Status = 0;

            var motamodel = khuyenmai.MoTa;
            string[] lstloaisp = motamodel.Split(',');
            foreach (var itemlsp in lstloaisp)
            {
                var maloaisp = Convert.ToInt32(itemlsp);
                var lstsanpham = db.SanPhams.Where(x => x.MaLoaiSanPham == maloaisp).ToList();
                foreach (var item in lstsanpham)
                {
                    var sanpham = db.SanPhams.Find(item.Id);
                    sanpham.KhuyenMai = sanpham.GiaBan ;
                    db.SaveChanges();
                }
            }

            db.SaveChanges();
            return khuyenmai.Id;
        }
    }
}