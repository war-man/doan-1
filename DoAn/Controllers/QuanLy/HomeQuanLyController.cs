using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models.EF;
using DoAn.Models.Dao.QuanLy;

namespace DoAn.Controllers.QuanLy
{
    public class HomeQuanLyController : Controller
    {
        // GET: HomeQuanLy
        TraSuaEntities db = new TraSuaEntities();

        NhanVienDao dao = new NhanVienDao();
        public ActionResult Index()
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            if (session_nhanvien != null)
            {
                var model = new NhanVienModel();
                model.SelectChucVu = new SelectList(db.ChucVus.Where(x => x.Id != 1 && x.Id != 3), "Id", "TenChucVu", 0);
                return View(model);
            }
            else
            {
                return RedirectToAction("DangNhapNhanVien", "Login");
            }
            

        }
        public JsonResult List(string txtSearch, int? page)
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            
                var list = db.NhanViens.Where(x => x.MaChiNhanh == session_nhanvien.MaChiNhanh && x.MaChucVu != 3).OrderByDescending(x => x.Id).ToList();

                int pageSize = 5;
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    ViewBag.txtSearch = txtSearch;
                    list = list.Where(x => x.MaChiNhanh == session_nhanvien.MaChiNhanh && x.MaChucVu != 3 && x.HoTen.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
                }
                var data = new List<NhanVienModel>();
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    var itemmodel = new NhanVienModel();
                    itemmodel.STT = i;
                    itemmodel.Id = item.Id;
                    itemmodel.HoTen = item.HoTen;
                    itemmodel.TenDangNhap = item.TenDangNhap;
                    itemmodel.DiaChi = item.DiaChi;
                    itemmodel.SDT = item.SDT;
                    itemmodel.TenChucVu = db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu).TenChucVu;
                    itemmodel.Luong = String.Format("{0:0,0}", db.ChucVus.FirstOrDefault(x => x.Id == item.MaChucVu).Luong);
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
        public JsonResult Add(NhanVienModel sanpham)
        {
            var session_nhanvien = (DoAn.Common.Session.NhanVienSession)Session[DoAn.Common.Constants.NHANVIEN_SESSION];
            return Json(dao.AddNhanVien(sanpham, session_nhanvien.MaChiNhanh), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var nhanvien = db.NhanViens.FirstOrDefault(x => x.Id == ID);
            return Json(nhanvien, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(NhanVienModel sp)
        {
            return Json(dao.UpdateNhanVien(sp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.DeleteNhanVien(ID), JsonRequestBehavior.AllowGet);
        }
        [ChildActionOnly]
        public PartialViewResult TenDangNhap()
        {

            return PartialView();
        }
    }
}