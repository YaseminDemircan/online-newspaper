using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsMVC.Models;
using PagedList;
using PagedList.Mvc;

namespace NewsMVC.Controllers
{
    public class HomeController : Controller
    {
        NewsMVCdbEntities db = new NewsMVCdbEntities();
        // GET: Home
        public ActionResult Index()
        {
            var haberler = db.Habers.OrderBy(x => x.EklenmeTarihi).Take(18).ToList();
            return View(haberler);
        }

        public PartialViewResult Kategori()
        {
            var kategori = db.Kategoris.Take(9).ToList();
            return PartialView(kategori);
        }

        public PartialViewResult TumKategoriler()
        {
            var tumkategoriler = db.Kategoris.ToList();
            return PartialView(tumkategoriler);
        }

        public PartialViewResult Kategoriler()
        {
            var kategoriler = db.Kategoris.ToList();
            return PartialView(kategoriler);
        }

        public ActionResult HaberDetay(int id)
        {
            var haber = db.Habers.Where(x => x.HaberID == id).SingleOrDefault();
            if(haber==null)
            {
                return HttpNotFound();
            }
            return View(haber);
        }

        public ActionResult KategoriHaber(int id, int? page)
        {
            var kategori = db.Habers.Where(x => x.KategoriID == id).ToList().ToPagedList(page ?? 1, 4);
            return View(kategori);
        }

        public ActionResult Ara(int? page, string Ara = null)
        {
            var aranan = db.Habers.Where(x => (x.Icerik.Contains(Ara)) ||(x.Baslik.Contains(Ara))).ToList().ToPagedList(page ?? 1, 4);
            return View(aranan);
        }

        public PartialViewResult SonHaberler()
        {
            var son = db.Habers.OrderByDescending(x => x.EklenmeTarihi).Take(10).ToList();
            return PartialView(son);
        }

        public PartialViewResult CokOkunanHaberler()
        {
            var cokOkunan = db.Habers.OrderBy(x => x.EklenmeTarihi).Take(10).ToList();
            return PartialView(cokOkunan);
        }

        public PartialViewResult Slider()
        {
            var son = db.Habers.OrderByDescending(x => x.EklenmeTarihi).Take(4).ToList();
            return PartialView(son);
        }

        public PartialViewResult BenzerHaberler()
        {
            var diger = db.Habers.OrderBy(x => x.EklenmeTarihi).Take(6).ToList();
            return PartialView(diger);
        }

       
    }
}