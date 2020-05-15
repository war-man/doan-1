using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models.Model.Admin;
using DoAn.Models.EF;
using DoAn.Models.Dao.Admin;
namespace DoAn.Controllers.Admin
{
    public class SanPhamAdminController : Controller
    {
        // GET: SanPhamAdmin
        TraSuaEntities db = new TraSuaEntities();
        ProductDao dao = new ProductDao();
        public string ProcessUpload(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return file.FileName;
        }
        public ActionResult SanPham()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                var model = new SanPhamModel();
                model.SelectMaLoai = new SelectList(db.LoaiSanPhams, "Id", "TenLoaiSanPham", 0);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
            
        }
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.SanPhams.Where(x=>x.MaLoaiSanPham !=12 && x.MaLoaiSanPham != 13 && x.MaLoaiSanPham !=14).OrderByDescending(x => x.Id).ToList();
            
            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.MaLoaiSanPham != 12 && x.MaLoaiSanPham != 13 && x.MaLoaiSanPham != 14 && x.TenSanPham.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<SanPhamModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new SanPhamModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.Ten = item.TenSanPham;
                itemmodel.GiaBan =String.Format("{0:0,0}", item.GiaBan);
                itemmodel.Anh = item.Anh;
                itemmodel.TenLoai = new CategoryDao().getById(item.MaLoaiSanPham).TenLoaiSanPham;
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
        public JsonResult Add(SanPhamModel sanpham)
        {
            return Json(dao.AddSanPham(sanpham), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var sanpham = db.SanPhams.FirstOrDefault(x=>x.Id == ID);
            return Json(sanpham, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(SanPhamModel sp)
        {
            return Json(dao.updateSanPham(sp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}