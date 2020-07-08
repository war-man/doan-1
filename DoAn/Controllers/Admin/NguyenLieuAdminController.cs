using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.Admin
{
    public class NguyenLieuAdminController : Controller
    {
        // GET: NguyenLieu
        TraSuaEntities db = new TraSuaEntities();
        NguyenLieuDao dao = new NguyenLieuDao();
        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.NguyenLieux.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.TenNguyenLieu.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<NguyenLieuModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new NguyenLieuModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.TenNguyenLieu = item.TenNguyenLieu;
                itemmodel.DonViTinh = item.DonViTinh;
                itemmodel.GiaNhap =String.Format("{0:0,0}",item.GiaNhap) +"đ";
                itemmodel.SoLuong = String.Format("{0:0,0}", item.SoLuong);
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
        public JsonResult Add(NguyenLieu nguyenlieu)
        {
            return Json(dao.AddNguyenLieu(nguyenlieu), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var nguyenlieu = db.NguyenLieux.FirstOrDefault(x => x.Id == ID);
            return Json(nguyenlieu, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(NguyenLieu nguyenlieu)
        {
            return Json(dao.updateNguyenLieu(nguyenlieu), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}