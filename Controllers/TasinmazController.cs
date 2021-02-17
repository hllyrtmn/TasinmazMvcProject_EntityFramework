using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using TasinmazMvc.Models;

namespace TasinmazMvc.Controllers
{
    [Log]
    public class TasinmazController : Controller
    {
        // GET: Tasinmaz
        TasinmazDbEntities db = new TasinmazDbEntities();
        public ActionResult Index(int? MAHID) {
            ViewBag.il = db.IL.ToList();
            ViewBag.mahid = MAHID;
            int id = (int)Session["ID"];
            if (Session["DURUM"].ToString() == "Yonetici")
            {
                var sayi = db.BILGI.Count();
                ViewBag.tasinmazsayi = (int)sayi;
            }
            else
            {
                var sayi = db.BILGI.Where(x => x.KULID == id).Count();
                ViewBag.tasinmazsayi = (int)sayi;
            }
            
            if (MAHID != null)
            {
                if (Session["ID"] != null)
                {
                    if (Session["DURUM"].ToString() =="Kullanici")
                    {
                        var bilgi = db.BILGI.Where(k => k.KULID == id).ToList();
                        var bilgi1 = bilgi.Where(x => x.MAHID == MAHID).ToList();
                        return View(bilgi1);
                    }
                    else
                    {
                        var bilgi = db.BILGI.ToList();
                        return View(bilgi);
                    }   
                }
                return View();
            }
            if (Session["DURUM"]!=null &&Session["DURUM"].ToString() == "Yonetici")
            {
                var bilgi = db.BILGI.ToList();

                return View(bilgi);
            }
            return View();
        }

        public ActionResult SehirGetir(int ILID)
        {
            return Json(db.ILCE.Where(s=>s.ILID==ILID).Select(s=>new
            {
                ID = s.ID,
                AD = s.AD
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SemtGetir(int ILCEID)
        {
            return Json(db.SEMT.Where(s => s.ILCEID == ILCEID).Select(s => new
            {
                ID = s.ID,
                AD = s.AD
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MahGetir(int SEMTID)
        {
            return Json(db.MAHALLE.Where(s => s.SEMTID == SEMTID).Select(s => new
            {
                ID = s.ID,
                AD = s.AD
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sil(int id)
        {
            var tasinmaz = db.BILGI.Find(id);
            if (tasinmaz != null)//Taşınmaz varsa
            {
                db.BILGI.Remove(tasinmaz);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult TasinmazEkle(int id=0)
        {
            if (id==0)
            {
                return View(new BILGI());
            }
            else
            {
                return View(db.BILGI.Where(x => x.ID==id).FirstOrDefault<BILGI>());
            }   
        }
        [HttpPost]
        public ActionResult TasinmazEkle(BILGI p1)
        {
            if (p1.ID==0)
            {
                db.BILGI.Add(p1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                db.Entry(p1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }
}