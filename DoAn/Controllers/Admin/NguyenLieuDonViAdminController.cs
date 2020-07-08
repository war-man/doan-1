using DoAn.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models.Model.Admin;
namespace DoAn.Controllers.Admin
{
    public class NguyenLieuDonViAdminController : Controller
    {
        // GET: NguyenLieuDonViAdmin
        TraSuaEntities db = new TraSuaEntities();
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
            

            int pageSize = 10;
            
            var data = new List<NguyenLieuDonViModel>();
            int i = 0;
            //foreach (var item in list)
            //{
            //    i++;
            //    var itemmodel = new NguyenLieuDonViModel();
            //    itemmodel.STT = i;
            //    itemmodel.Id = item.Id;
            //    itemmodel.TenDonVi = db.DonViTinhs.FirstOrDefault(x => x.Id == item.MaDonViTinh).TenDonViTinh;
            //    itemmodel.TenNguyenLieu = db.NguyenLieux.FirstOrDefault(x => x.Id == item.MaNguyenLieu).TenNguyenLieu;

            //    itemmodel.SoLuong = item.SoLuong;
            //    itemmodel.GiaNhap =String.Format("{0:0,0}", item.GiaNhap);
            //    data.Add(itemmodel);

            //}
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
    }
}