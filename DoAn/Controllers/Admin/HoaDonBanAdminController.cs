using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Dao.Admin;
namespace DoAn.Controllers.Admin
{
    public class HoaDonBanAdminController : Controller
    {
        // GET: HoaDonBanAdmin
        TraSuaEntities db = new TraSuaEntities();
        HoaDonBanDao dao = new HoaDonBanDao();
        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var model = new HoaDonBanModel();
                model.SelectChiNhanh = new SelectList(db.ChiNhanhs, "Id", "TenChiNhanh", 0);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }
        public ActionResult IndexDuyet()
        {
            return View();
        }
        public JsonResult List(int? page)
        {
            var list = db.HoaDonBans.Where(x => x.MaKhach != 3 && x.MaKhach != 4 && x.Duyet==0).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            
            var data = new List<HoaDonBanModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new HoaDonBanModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.NgayBanShow =String.Format("{0:d/M/yyyy}", item.NgayBan);
                var khachhang = new KhachHangDao().viewDetail(item.MaKhach);

                itemmodel.TenDangNhap = khachhang.TenDangNhap;
                itemmodel.DiaChi = khachhang.DiaChi;
                itemmodel.SDT = khachhang.SDT;
                itemmodel.TongTienShow =String.Format("{0:0,0}",item.TongTien);
                itemmodel.Status = item.Duyet;
                itemmodel.DaThanhToan = item.DaThanhToan;
                data.Add(itemmodel);

            }
            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            var datamodel = data.Skip(start).Take(pageSize);

            return Json(new { data = datamodel, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ListDuyet(int? page)
        {
            var list = db.HoaDonBans.Where(x => x.MaKhach != 3 && x.MaKhach != 4 && x.Duyet == 1).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;

            var data = new List<HoaDonBanModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new HoaDonBanModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.NgayBanShow = String.Format("{0:d/M/yyyy}", item.NgayBan);
                var khachhang = new KhachHangDao().viewDetail(item.MaKhach);

                itemmodel.TenDangNhap = khachhang.TenDangNhap;
                itemmodel.DiaChi = khachhang.DiaChi;
                itemmodel.SDT = khachhang.SDT;
                itemmodel.TongTienShow = String.Format("{0:0,0}", item.TongTien);
                itemmodel.Status = item.Duyet;
                itemmodel.DaThanhToan = item.DaThanhToan;
                data.Add(itemmodel);

            }
            if (page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            var datamodel = data.Skip(start).Take(pageSize);

            return Json(new { data = datamodel, pageCurrent = page, numSize = numSize }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(string ID)
        {
            var hoadonban = db.HoaDonBans.FirstOrDefault(x => x.Id == ID);
            var model = new HoaDonBanModel();
            model.Id = hoadonban.Id;
            var khachhang = db.KhachHangs.FirstOrDefault(x => x.Id == hoadonban.MaKhach);
            model.TenDangNhap = khachhang.TenDangNhap;
            model.DiaChi = khachhang.DiaChi;
            model.SDT = khachhang.SDT;
            model.TongTien = hoadonban.TongTien;
            model.Email = khachhang.Email;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(HoaDonBanModel hoadonban)
        {
            return Json(dao.UpdateDuyet(hoadonban), JsonRequestBehavior.AllowGet);
        }
    }
}