using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.Admin
{
    public class DonViTinhDao
    {
        TraSuaEntities db = null;
        public DonViTinhDao()
        {
            db = new TraSuaEntities();
        }

        public DonViTinh getByid(int? madonvi)
        {
            return db.DonViTinhs.SingleOrDefault(x => x.Id == madonvi);
        }
        public DonViTinh FindDonVi(int? madonvi)
        {
            return (db.DonViTinhs.Find(madonvi));
        }
        public int AddDonViTinh(DonViTinh dvt)
        {
            var donvitinh = new DonViTinh();

            donvitinh.TenDonViTinh = dvt.TenDonViTinh;

            db.DonViTinhs.Add(donvitinh);
            db.SaveChanges();
            return donvitinh.Id;
        }
        public int updateDonViTinh(DonViTinh dvt)
        {
            var donvitinh = db.DonViTinhs.Find(dvt.Id);

            donvitinh.TenDonViTinh = dvt.TenDonViTinh;

            db.SaveChanges();
            return donvitinh.Id;
        }
        public int Delete(int madonvi)
        {
            var donvitinh = db.DonViTinhs.Find(madonvi);
            db.DonViTinhs.Remove(donvitinh);
            db.SaveChanges();
            return donvitinh.Id;
        }
    }
}