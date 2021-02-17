using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TasinmazMvc.Models;


namespace TasinmazMvc.Controllers
{
    public class LoginController : Controller
    {
        
        // GET: Login
        TasinmazDbEntities db = new TasinmazDbEntities();

        public string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(KULLANICI p1)
        {
            SHA256 parola = new SHA256CryptoServiceProvider();


            p1.PAROLA = Convert.ToBase64String(parola.ComputeHash(Encoding.UTF8.GetBytes(p1.PAROLA)));

            var kullanici = db.KULLANICI.FirstOrDefault(x => x.MAIL == p1.MAIL && x.PAROLA == p1.PAROLA);

            if (kullanici != null)
            {
                ViewBag.mail = Convert.ToString(kullanici.MAIL);
                FormsAuthentication.SetAuthCookie(kullanici.MAIL, false);
                Session["ID"] = kullanici.ID;
                ViewBag.id = Convert.ToInt32(Session["ID"]);
                Session["MAIL"] = kullanici.MAIL;
                Session["DURUM"] = kullanici.DURUM;
                ViewBag.durum = Session["DURUM"].ToString();
                return RedirectToAction("Index", "Tasinmaz");
            }
            else
            {
                Response.Write("<script>alert('Yanlış kullanıcı veya parola girdiniz')</script>");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        
    }
}