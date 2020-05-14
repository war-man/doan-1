using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using DoAn.Models.Dao;
using DoAn.Models.Dao.Admin;

namespace DoAn.Controllers.Admin
{
    public class NhomSanPhamAdminController : Controller
    {
        // GET: NhomSanPhamAdmin
        TraSuaEntities db = new TraSuaEntities();
        NhomSanPhamDao dao = new NhomSanPhamDao();
        
        public ActionResult Index()
        {
            return View();

        }
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.LoaiSanPhams.Where(x => x.Id != 12 && x.Id != 13 && x.Id != 14).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.Id != 12 && x.Id != 13 && x.Id != 14 && x.TenLoaiSanPham.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<NhomSanPhamModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new NhomSanPhamModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.TenNhomSanPham = item.TenLoaiSanPham;
                
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
        public JsonResult Add(LoaiSanPham loaisanpham)
        {
            return Json(dao.Add(loaisanpham), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var loaisp = db.LoaiSanPhams.FirstOrDefault(x => x.Id == ID);
            return Json(loaisp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(LoaiSanPham loaisp)
        {
            return Json(dao.Update(loaisp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}