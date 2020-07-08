using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class NguyenLieuDao
    {
        TraSuaEntities db = null;
        public NguyenLieuDao()
        {
            db = new TraSuaEntities();
        }

        public NguyenLieu getByid(int? manguyenlieu)
        {
            return db.NguyenLieux.SingleOrDefault(x => x.Id == manguyenlieu);
        }
        public NguyenLieu FindNguyenLieu(int? manguyenlieu)
        {
            return (db.NguyenLieux.Find(manguyenlieu));
        }
        public int AddNguyenLieu(NguyenLieu nl)
        {
            var nguyenlieu = new NguyenLieu();

            nguyenlieu.TenNguyenLieu = nl.TenNguyenLieu;
            nguyenlieu.DonViTinh = nl.DonViTinh;
            db.NguyenLieux.Add(nguyenlieu);
            db.SaveChanges();
            return nguyenlieu.Id;
        }
        public int updateNguyenLieu(NguyenLieu nl)
        {
            var nguyenlieu = db.NguyenLieux.Find(nl.Id);

            nguyenlieu.TenNguyenLieu = nl.TenNguyenLieu;
            nguyenlieu.DonViTinh = nl.DonViTinh;
            db.SaveChanges();
            return nguyenlieu.Id;
        }
        public int Delete(int manguyenlieu)
        {
            var nguyenlieu = db.NguyenLieux.Find(manguyenlieu);
            db.NguyenLieux.Remove(nguyenlieu);
            db.SaveChanges();
            return nguyenlieu.Id;
        }
    }
}