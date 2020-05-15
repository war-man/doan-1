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
    public class DonViTinhAdminController : Controller
    {
        // GET: DonViTinhAdmin
        TraSuaEntities db = new TraSuaEntities();
        DonViTinhDao dao = new DonViTinhDao();
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
            var list = db.DonViTinhs.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.TenDonViTinh.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<DonViTinhModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new DonViTinhModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.TenDonVi = item.TenDonViTinh;
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
        public JsonResult Add(DonViTinh donvitinh)
        {
            return Json(dao.AddDonViTinh(donvitinh), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var donvitinh = db.DonViTinhs.FirstOrDefault(x => x.Id == ID);
            return Json(donvitinh, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(DonViTinh donvitinh)
        {
            return Json(dao.updateDonViTinh(donvitinh), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}