using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models.Dao.NguoiDung
{
    public class BillDao
    {
        TraSuaEntities db = null;
        public BillDao()
        {
            db = new TraSuaEntities();
        }
        public string Insert(HoaDonBan hoadonban)
        {
            db.HoaDonBans.Add(hoadonban);
            db.SaveChanges();
            return hoadonban.Id;
        }
        public void Insert_Bill_Detail(ChiTietHDB cthdb)
        {
            db.ChiTietHDBs.Add(cthdb);
            db.SaveChanges();
        }
        public int tongsoluong(string mahoadonban)
        {
            var tongsoluong = 0;
            var list = db.ChiTietHDBs.Where(x => x.MaHDB == mahoadonban);
            foreach (var item in list)
            {
                if (new CategoryDao().getSPChinh(item.MaSanPham) == 1)
                {
                    tongsoluong +=Convert.ToInt32( item.SoLuong);
                }
            }
            return tongsoluong;
        }

    }
}