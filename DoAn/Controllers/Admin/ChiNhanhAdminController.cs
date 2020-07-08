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
    public class ChiNhanhAdminController : Controller
    {
        // GET: ChiNhanhAdmin
        TraSuaEntities db = new TraSuaEntities();
        ChiNhanhDao dao = new ChiNhanhDao();
        
        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if(session != null)
            {
                var list = db.ChiNhanhs.ToList();
                string todocacchinhanh = "";
                foreach(var item in list)
                {
                    todocacchinhanh += item.Id.ToString() + "," + item.Lat.ToString() + "," + item.Lng.ToString() + ",0;";
                }
                ViewBag.ToDoCacChiNhanh = todocacchinhanh;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           

        }
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.ChiNhanhs.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x=> x.TenChiNhanh.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<ChiNhanhModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new ChiNhanhModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.TenChiNhanh = item.TenChiNhanh;
                itemmodel.DiaChi = item.DiaChi;
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
        public JsonResult Add(ChiNhanhModel chinhanh)
        {
            return Json(dao.AddChiNhanh(chinhanh), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var chinhanh = db.ChiNhanhs.FirstOrDefault(x => x.Id == ID);
            return Json(chinhanh, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(ChiNhanhModel chinhanh)
        {
            return Json(dao.updateChiNhanh(chinhanh), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}