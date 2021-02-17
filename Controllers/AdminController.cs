using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TasinmazMvc.Models;
using System.Data;
using System.Data.Entity;

namespace TasinmazMvc.Controllers
{
    [Log]
    [Authorize(Roles = "Yonetici")]
    public class AdminController : Controller
    {
        TasinmazDbEntities db = new TasinmazDbEntities();

        // GET: Admin

        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Yonetici", Value = "0" });

            items.Add(new SelectListItem { Text = "Kullanici", Value = "1" });

            ViewBag.dgr = items;

            var deger = db.KULLANICI.ToList();
            return View(deger);
        }
        [HttpGet]
        public ActionResult KullaniciEkle()
        {

            return View();
        }
        [HttpPost]
        public ActionResult KullaniciEkle([Bind(Include = "MAIL,PAROLA,DURUM,BILGIID")] KULLANICI p1)
        {
            if (ModelState.IsValid)
            {
                var mailkontrol = db.KULLANICI.Where(x => x.MAIL == p1.MAIL).FirstOrDefault();

                int sayac = 0;
                int Sayi_varmi = 0;
                int Sembol_varmi = 0;
                int Buyuk_varmi = 0;
                int Kucuk_varmi = 0;
                string et = "@";
                string com = ".com";

                char[] karakter = p1.PAROLA.ToCharArray();
                for (int i = 0; i < p1.PAROLA.Length; i++)
                {
                    if (char.IsNumber(karakter[i]))//sayı ise tru
                    {
                        Sayi_varmi++;
                    }
                    if (char.IsSymbol(karakter[i]))//sembol ise tru
                    {
                        Sembol_varmi++;
                    }
                    if (char.IsUpper(karakter[i]))// harf büyükse tru
                    {
                        Buyuk_varmi++;
                    }
                    if (char.IsLower(karakter[i]))//küçükse tru
                    {
                        Kucuk_varmi++;
                    }

                }
                if (mailkontrol == null)
                {
                    if (p1.PAROLA.Length > 8 && p1.MAIL.IndexOf(et) != -1 && p1.MAIL.IndexOf(com) != -1 && p1.MAIL.Length > 8)
                    {



                        if (Sayi_varmi > 0 && Sembol_varmi > 0 && Buyuk_varmi > 0 && Kucuk_varmi > 0)
                        {
                            SHA256 parola = new SHA256CryptoServiceProvider();

                            p1.PAROLA = Convert.ToBase64String(parola.ComputeHash(Encoding.UTF8.GetBytes(p1.PAROLA)));
                            db.KULLANICI.Add(p1);

                            db.SaveChanges();
                            return RedirectToAction("Index");
                        } 
                        else
                        {
                            Response.Write("<script>alert('Parolanız Uygun Değildir.');</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Parolanız Uygun Değildir.');</script>");
                    }
                }

                else
                {
                    Response.Write("<script>alert('Kayıtlı bir hesabınız bulunmaktadır.');</script>");

                }
            }
            return View("Index");
        }
        public ActionResult KullaniciSil(int id)
        {
            var kullanici = db.KULLANICI.Find(id);

            db.KULLANICI.Remove(kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SehirListele()
        {

            var il = db.IL.ToList();

            return View(il);

        }
        public ActionResult IlceListele(int ID)
        {
            var ilce = db.ILCE.Where(x => x.ILID == ID).ToList();
            return View(ilce);
        }
        public ActionResult SemtListele(int ID)
        {
            var semt = db.SEMT.Where(x => x.ILCEID == ID).ToList();
            return View(semt);
        }
        public ActionResult MahListele(int ID)
        {
            var mah = db.MAHALLE.Where(x => x.SEMTID == ID).ToList();
            return View(mah);
        }
        public ActionResult SehirSil(int ID)
        {
            var il = db.IL.Find(ID);

            //if (il != null)
            //{
            //    foreach (var ilce in il)
            //    {
            //        if (ilce != null)
            //        {
            //            foreach (var semt in ilce)
            //            {
            //                if (semt != null)
            //                {
            //                    foreach (var mahalle in semt)
            //                    {
            //                        db.MAHALLE.Remove(mahalle);
            //                    }
            //                }
            //                db.MAHALLE.Remove(semt);
            //            }
            //        }
            //        db.MAHALLE.Remove(ilce);
            //    }
            //}

            if (il != null)
            {

                var ilceler = db.ILCE.Where(x => x.ILID == ID).ToList();
                var ilceler1 = db.ILCE.Where(x => x.ILID == ID).Select(s => s.ID).ToList();

                if (ilceler.Count != 0)
                {
                    for (int i = 0; i < ilceler1.Count; i++)
                    {
                        int ilceid = ilceler1[i];

                        var semtler = db.SEMT.Where(x => x.ILCEID == ilceid).ToList();

                        if (semtler.Count != 0)
                        {

                            var semt1 = db.SEMT.Where(x => x.ILCEID == ilceid).Select(x => x.ID).ToList();
                            for (int j = 0; j < semt1.Count; j++)
                            {
                                int semtid = semt1[j];

                                var mahalleler = db.MAHALLE.Where(x => x.SEMTID == semtid).ToList();

                                if (mahalleler.Count != 0)
                                {

                                    foreach (var mahalle in mahalleler)
                                    {
                                        db.MAHALLE.Remove(mahalle);
                                    }
                                    db.SEMT.Remove(semtler[j]);
                                }
                            }
                            foreach (var semt in semtler)
                            {
                                db.SEMT.Remove(semt);
                            }

                        }
                    }
                    foreach (var ilce in ilceler)
                    {
                        db.ILCE.Remove(ilce);
                    }
                    db.IL.Remove(il);
                    db.SaveChanges();
                    return RedirectToAction("SehirListele");
                }

                db.IL.Remove(il);
                db.SaveChanges();
                return RedirectToAction("SehirListele");
            }
            return RedirectToAction("SehirListele");
        }

        [HttpGet]
        public ActionResult SehirEkle(int id = 0)
        {
            if (id == 0)
            {
                return View(new IL());
            }
            else
            {
                return View(db.IL.Where(x => x.ID == id).FirstOrDefault<IL>());
            }
        }
        [HttpPost]
        public ActionResult SehirEkle(IL p1)
        {
            if (p1.ID == 0)
            {
                db.IL.Add(p1);
                db.SaveChanges();
                return RedirectToAction("SehirListele");
            }
            else
            {
                db.Entry(p1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SehirListele");
            }

        }
        [HttpGet]
        public ActionResult IlceEkle(int id = 0)
        {
            if (id == 0)
            {
                return View(new IL());
            }
            else
            {
                return View(db.IL.Where(x => x.ID == id).FirstOrDefault<IL>());
            }
        }
        [HttpPost]
        public ActionResult IlceEkle(IL p1)
        {
            if (p1.ID == 0)
            {
                db.IL.Add(p1);
                db.SaveChanges();
                return RedirectToAction("SehirListele");
            }
            else
            {
                db.Entry(p1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SehirListele");
            }

        }



    }
}