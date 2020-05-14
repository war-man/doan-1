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
    public class KhachHangOnlineAdminController : Controller
    {
        // GET: KhachHangOnlineAdmin
        TraSuaEntities db = new TraSuaEntities();
        KhachHangDao dao = new KhachHangDao();
       public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.KhachHangs.Where(x=>x.Id !=3 && x.Id !=4).OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.Id != 3 && x.Id != 4 && x.TenDangNhap.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<KhachHangModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new KhachHangModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.TenDangNhap = item.TenDangNhap;
                itemmodel.Email = item.Email;
                itemmodel.SDT = item.SDT;
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
        
        public JsonResult Delete(int ID)
        {
            return Json(dao.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}