using DoAn.Common.Function;
using DoAn.Models.Dao.Admin;
using DoAn.Models.EF;
using DoAn.Models.Model.Admin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers.Admin
{
    public class KhuyenMaiAdminController : Controller
    {
        TraSuaEntities db = new TraSuaEntities();
        KhuyenMaiDao dao = new KhuyenMaiDao();

        public ActionResult Index()
        {
            var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
            if (session != null)
            {
                ViewBag.NhomSanPham = db.LoaiSanPhams.Where(x => x.Id == 1 || x.SanPhamChinh == 1).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        

        public JsonResult List(string txtSearch, int? page)
        {
            var list = db.KhuyenMais.OrderByDescending(x => x.Id).ToList();

            int pageSize = 10;
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                list = list.Where(x => x.TenKhuyenMai.Contains(txtSearch)).OrderByDescending(x => x.Id).ToList();
            }
            var data = new List<KhuyenMaiModel>();
            int i = 0;
            foreach (var item in list)
            {
                i++;
                var itemmodel = new KhuyenMaiModel();
                itemmodel.STT = i;
                itemmodel.Id = item.Id;
                itemmodel.Ten = item.TenKhuyenMai;
                itemmodel.NgayBDShow =String.Format("{0:dd/MM/yyyy}", item.NgayBatDau);
                itemmodel.NgayKTShow =String.Format("{0:dd/MM/yyyy}", item.NgayKetThuc);
                itemmodel.PhanTram = item.PhanTram;
                var motasql = item.MoTa;
                string[] lstloaisp = motasql.Split(',');
                string lstloaisanpham = "";
                foreach(var itemlsp in lstloaisp)
                {
                    var maloaisp = Convert.ToInt32(itemlsp);
                    lstloaisanpham += db.LoaiSanPhams.FirstOrDefault(x => x.Id == maloaisp).TenLoaiSanPham +", ";
                }
                itemmodel.MoTa = lstloaisanpham;
                itemmodel.Status = item.Status;
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
        public JsonResult Add(KhuyenMaiModel khuyenmai)
        {
            return Json(dao.AddKhuyenMai(khuyenmai), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(int ID)
        {
            return Json(dao.UpdateKhuyenMai(ID), JsonRequestBehavior.AllowGet);
        }






        // GET: KhuyenMaiAdmin
        //TraSuaEntities db = new TraSuaEntities();
        //public ActionResult Index(int? page)
        //{
        //    var session = (DoAn.Common.Session.UserLogin)Session[DoAn.Common.Constants.USER_SESSION];
        //    if (session != null)
        //    {
        //        int pageSize = 10;
        //        int pageNumber = (page ?? 1);
        //        var model = new List<KhuyenMaiModel>();
        //        var list = db.KhuyenMais.ToList();
        //        var i = 0;
        //        foreach (var item in list)
        //        {
        //            i++;
        //            var khuyenmai = new KhuyenMaiModel();
        //            khuyenmai.STT = i;
        //            khuyenmai.Id = item.Id;
        //            khuyenmai.Ten = item.TenKhuyenMai;
        //            khuyenmai.PhanTram = item.PhanTram;
        //            khuyenmai.NgayBatDau = item.NgayBatDau;
        //            khuyenmai.NgayKetThuc = item.NgayKetThuc;
        //            khuyenmai.Status = item.Status;
        //            model.Add(khuyenmai);
        //        }
        //        return View(model.ToPagedList(pageNumber, pageSize));
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //}
        //public ActionResult CreateKhuyenMai()
        //{

        //    var model = new KhuyenMaiModel();
        //    model.ListLoaiSanPham = db.LoaiSanPhams.Where(x => x.Id == 1 || x.SanPhamChinh == 1).ToList();
        //    model.SelectLoaiKM = new SelectList(db.LoaiKhuyenMais, "Id", "TenLoaiKhuyenMai", 0);
        //    return View(model);

        //}
        //[HttpPost]
        //public ActionResult CreateKhuyenMai(KhuyenMaiModel model)
        //{
        //    var khuyenmai = new KhuyenMai();
        //    var makhuyenmai = new RandomId().MaNgauNhien_SoChu(5);
        //    khuyenmai.Id = makhuyenmai;
        //    khuyenmai.TenKhuyenMai = model.Ten;
        //    khuyenmai.PhanTram = model.PhanTram;
        //    khuyenmai.NgayBatDau = model.NgayBatDau;
        //    khuyenmai.NgayKetThuc = model.NgayKetThuc;
        //    khuyenmai.Status = 1;

        //    var listloaisanpham = Request.Form["listlspdc"];
        //    if (listloaisanpham != "")
        //    {
        //        string[] listlsp = listloaisanpham.Split(',');
        //        foreach (var item in listlsp)
        //        {
        //            var maloaisp = int.Parse(item);
        //            var listsp = db.SanPhams.Where(x => x.MaLoaiSanPham == maloaisp).ToList();
        //            foreach (var itemsp in listsp)
        //            {
        //                var product = db.SanPhams.Find(itemsp.Id);
        //                product.KhuyenMai = product.GiaBan - (product.GiaBan * model.PhanTram / 100);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    else
        //    {

        //    }
        //    khuyenmai.MoTa = listloaisanpham;



        //    db.KhuyenMais.Add(khuyenmai);
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "KhuyenMaiAdmin");
        //}
        //public ActionResult UpdateKhuyenMai(string makhuyenmai)
        //{
        //    var khuyenmai = (KhuyenMai)db.KhuyenMais.FirstOrDefault(x => x.Id == makhuyenmai);
        //    khuyenmai.Status = 0;

        //    var listnhomsp = khuyenmai.MoTa;
        //    string[] listlsp = listnhomsp.Split(',');
        //    foreach (var item in listlsp)
        //    {
        //        var maloaisanpham = int.Parse(item);
        //        var lstsanpham = db.SanPhams.Where(x => x.MaLoaiSanPham == maloaisanpham).ToList();
        //        foreach (var sanpham in lstsanpham)
        //        {
        //            sanpham.KhuyenMai = sanpham.GiaBan;
        //            db.SaveChanges();
        //        }
        //    }

        //    return RedirectToAction("Index", "KhuyenMaiAdmin");
        //}
    }
}