using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsMVC.Models;
using System.Web.Helpers;
using System.IO;
using System.Drawing;

namespace NewsMVC.Controllers
{
    public class AdminHaberController : Controller
    {
        NewsMVCdbEntities db = new NewsMVCdbEntities();
        // GET: AdminHaber
        public ActionResult Index()
        {
            var haberler = db.Habers.ToList();
            return View(haberler);
        }

        // GET: AdminHaber/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminHaber/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi");
            return View();
        }

        // POST: AdminHaber/Create
        [HttpPost]
        public ActionResult Create(Haber haber, HttpPostedFileBase resim)
        {
            try
            {
                Image img = Image.FromStream(resim.InputStream);
                Bitmap Resim = new Bitmap(img);
                Resim.Save(Server.MapPath("/Images/" + resim.FileName));

                Resim rsm = new Resim();
                rsm.OrtaBoy = "/Images/" + resim.FileName;

                db.Resims.Add(rsm);
                db.SaveChanges();

                haber.ResimID = rsm.ResimId;
                db.Habers.Add(haber);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminHaber/Edit/5
        public ActionResult Edit(int id)
        {
            var haber = db.Habers.Where(x => x.HaberID == id).SingleOrDefault();
            if(haber==null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriId", "KategoriAdi", haber.KategoriID);
            return View(haber);
        }

        // POST: AdminHaber/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Haber haberler, HttpPostedFileBase resim)
        {
            try
            {
                var haber = db.Habers.Where(x => x.HaberID == id).SingleOrDefault();
                if (resim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(haber.Resim.OrtaBoy)))
                    {
                        System.IO.File.Delete(Server.MapPath(haber.Resim.OrtaBoy));
                    }
                    Image img = Image.FromStream(resim.InputStream);
                    Bitmap Resim = new Bitmap(img);
                    Resim.Save(Server.MapPath("/Images/" + resim.FileName));

                    Resim rsm = new Resim();
                    rsm.OrtaBoy = "/Images/" + resim.FileName;

                    db.Resims.Add(rsm);
                    db.SaveChanges();

                    haber.ResimID = rsm.ResimId;
                }
                    haber.Baslik = haberler.Baslik;
                    haber.Icerik = haberler.Icerik;
                    haber.Giris = haberler.Giris;
                    haber.KategoriID = haberler.KategoriID;
                    haber.EklenmeTarihi = haberler.EklenmeTarihi;
                    db.SaveChanges();
                
                   return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminHaber/Delete/5
        public ActionResult Delete(int id)
        {
            var haber = db.Habers.Where(x => x.HaberID == id).SingleOrDefault();
            if(haber==null)
            {
                return HttpNotFound();
            }
            return View(haber);
        }

        // POST: AdminHaber/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var haber = db.Habers.Where(x => x.HaberID == id).SingleOrDefault();
                if (haber == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(haber.Resim.OrtaBoy)))
                {
                    System.IO.File.Delete(Server.MapPath(haber.Resim.OrtaBoy));
                }
                foreach(var y in haber.Yorums.ToList())
                {
                    db.Yorums.Remove(y);
                }
                db.Habers.Remove(haber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
