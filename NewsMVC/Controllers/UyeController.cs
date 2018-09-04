using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsMVC.Models;
using System.Drawing;

namespace NewsMVC.Controllers
{
    public class UyeController : Controller
    {
        NewsMVCdbEntities db = new NewsMVCdbEntities();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(x => x.UyeId == id).SingleOrDefault();
            if(Convert.ToInt32(Session["uyeId"])!=uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uyes.Where(x => x.KullaniciAdi == uye.KullaniciAdi).SingleOrDefault();
            if (login.KullaniciAdi == uye.KullaniciAdi && login.Sifre == uye.Sifre)
            {
                Session["uyeId"] = login.UyeId;
                Session["kullaniciAdi"] = login.KullaniciAdi;
                Session["yetkiId"] = login.YetkiId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Uyari = "Kullanıcı adı veya şifre hatalı!!!";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["uyeId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Uye uye, HttpPostedFileBase foto)
        {
            try
            {
                Image img = Image.FromStream(foto.InputStream);
                Bitmap Resim = new Bitmap(img);
                Resim.Save(Server.MapPath("/Images/" + foto.FileName));

                Resim rsm = new Resim();
                rsm.KucukBoy = "/Images/" + foto.FileName;

                db.Resims.Add(rsm);
                db.SaveChanges();

                uye.ResimID = rsm.ResimId;
                uye.YetkiId = 2;
                db.Uyes.Add(uye);
                db.SaveChanges();
                Session["uyeId"] = uye.UyeId;
                Session["kullaniciAdi"] = uye.KullaniciAdi;
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(x => x.UyeId == id).SingleOrDefault();
            if(Convert.ToInt32(Session["uyeId"])!=uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        [HttpPost]
        public ActionResult Edit(int id,Uye uye, HttpPostedFileBase resim)
        {
            try
            {
                var uyee = db.Uyes.Where(x => x.UyeId == id).SingleOrDefault();
                if (resim != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uyee.Resim.KucukBoy)))
                    {
                        System.IO.File.Delete(Server.MapPath(uyee.Resim.KucukBoy));
                    }
                    Image img = Image.FromStream(resim.InputStream);
                    Bitmap Resim = new Bitmap(img);
                    Resim.Save(Server.MapPath("/Images/" + resim.FileName));

                    Resim rsm = new Resim();
                    rsm.KucukBoy = "/Images/" + resim.FileName;

                    db.Resims.Add(rsm);
                    db.SaveChanges();
                    uyee.ResimID = rsm.ResimId;
                }
                    
                    uyee.KullaniciAdi = uye.KullaniciAdi;
                    uyee.Email = uye.Email;
                    uyee.Sifre = uye.Sifre;
                    uyee.AdSoyad = uye.AdSoyad;
                    db.SaveChanges();
                    Session["kullaniciAdi"] = uye.KullaniciAdi;
                    return RedirectToAction("Index", "Home", new { id=uyee.UyeId });
            }
            catch
            {
                return View();
            }
        }
    }
}