using DoAn.Common.Function;
using DoAn.Models.Dao.NguoiDung;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Model.NguoiDung;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.NguoiDung
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        TraSuaEntities db = new TraSuaEntities();
        public ActionResult Index(int? page, string key = null, int? maloai = null)
        {
            ViewBag.Key = key;
            var list = db.SanPhams.Where(x => x.MaLoaiSanPham != 1 && x.MaLoaiSanPham != 12 && x.MaLoaiSanPham != 13 && x.MaLoaiSanPham != 14).ToList();
            if (key != null)
            {
                if (maloai == null)
                {
                    list = list.Where(x => x.TenSanPham.Contains(key)).OrderBy(s => s.GiaBan).ToList();
                }
                else
                {
                    list = list.Where(x => x.TenSanPham.Contains(key) && x.MaLoaiSanPham == maloai).OrderBy(s => s.GiaBan).ToList();
                }

            }
            else
            {
                if (maloai == null)
                {
                }
                else
                {
                    list = list.Where(x => x.MaLoaiSanPham == maloai).OrderBy(s => s.GiaBan).ToList();
                }
            }
            int pageSize = 16;
            int pageNumber = (page ?? 1);
            ViewBag.Page = pageNumber;
            var model = new List<SanPhamModel>();
            foreach (var item in list)
            {
                var itemmodel = new SanPhamModel();
                itemmodel.Id = item.Id;
                itemmodel.Ten = item.TenSanPham;
                itemmodel.GiaBan = String.Format("{0:0,0}", item.GiaBan) + "đ";
                itemmodel.KhuyenMaiShow = String.Format("{0:0,0}", item.KhuyenMai) + "đ";
                itemmodel.Anh = item.Anh;
                itemmodel.PhanTramKM = (item.GiaBan * 100 - item.KhuyenMai * 100) / item.GiaBan + "%";
                model.Add(itemmodel);
            }
           
            return View(model.ToPagedList(pageNumber,pageSize));

        }
        
       
        [ChildActionOnly]
        public PartialViewResult Top4SPBanChay()
        {
            var listTopProduct = new List<SanPhamModel>();
            var list = (from sp in db.SanPhams
                        join cthdb in db.ChiTietHDBs on sp.Id equals cthdb.MaSanPham
                        group cthdb by cthdb.MaSanPham into g
                        select new SanPhamModel
                        {
                            Id = g.FirstOrDefault().MaSanPham,
                            TongSL = g.Sum(x => x.SoLuong),

                        }).OrderByDescending(x => x.TongSL).ToList();
            var i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new SanPhamModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                
                if (new CategoryDao().getSPChinh(itemmodel.Id) == 1)
                {
                    var product = new ProductDao().getByid(item.Id);

                    itemmodel.Ten = product.TenSanPham;
                    itemmodel.Anh = product.Anh;
                    itemmodel.GiaBan = String.Format("{0:0,0}", product.GiaBan);
                    itemmodel.KhuyenMai = product.KhuyenMai;
                    itemmodel.TongSL = item.TongSL;
                    int giaban = Convert.ToInt32(product.GiaBan);
                    itemmodel.PhanTramKM = (giaban * 100 - itemmodel.KhuyenMai * 100) / giaban + "%";
                    listTopProduct.Add(itemmodel);
                }

            }

            return PartialView(listTopProduct.Take(4).ToList());
        }
        
        public ActionResult ChiTietSanPham(int? masanpham)
        {
            ViewBag.SanPham = db.SanPhams.FirstOrDefault(x => x.Id == masanpham);
            var session_user = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if(session_user != null)
            {
                ViewBag.Session_User = 1;
            }
            else
            {
                ViewBag.Session_User = 0;
            }
            var model = new List<DanhGiaModel>();
            var list = db.BinhLuans.Where(x => x.MaSanPham == masanpham).OrderByDescending(x => x.Id).ToList();
            foreach (var item in list)
            {
                var itemmodel = new DanhGiaModel();
                itemmodel.Id = item.Id;
                itemmodel.MaKhachHang = item.MaKhachHang;
                itemmodel.MaSanPham = masanpham;
                itemmodel.NoiDung = item.NoiDung;
                itemmodel.TenDangNhap = new KhachHangDao().viewDetail(item.MaKhachHang).TenDangNhap;
                itemmodel.DanhGia = item.DanhGia;
                DateTime dtime = DateTime.Now;
                itemmodel.ThoiGian = new LamTronThoiGian().LamTron(dtime, item.ThoiGian);
                model.Add(itemmodel);
            }
            var tongsao = list.Sum(x => x.DanhGia);
            var tongdanhgia = list.Count;
            if (tongdanhgia == 0)
            {
                ViewBag.Star = 0;
            }
            else
            {
                var chia = tongsao / tongdanhgia;
                ViewBag.Star = chia;
            }


            return View(model);

        }
    }
}