using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TasinmazMvc.Models
{
    public class LogAttribute : ActionFilterAttribute
    {
        TasinmazDbEntities db = new TasinmazDbEntities();

        private Stopwatch _stopwatch;

        public LogAttribute()
        {
            _stopwatch = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void User(KULLANICI kullanicilar)
        {
            var User = db.KULLANICI.Where(k => k.MAIL == kullanicilar.MAIL).Select(k => k.ID).FirstOrDefault();
            
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();

            var request = filterContext.HttpContext.Request;//gelen isteği al
            

            LOG l = new LOG();
            l.AddedDate = DateTime.Now;
            l.Data = SerializeRequest(request);
            l.ExecutionMs = _stopwatch.ElapsedMilliseconds; //çalışma süresi
            l.IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            l.UrlAccessed = request.RawUrl; //erişilen sayfanın ham url'i

            if (HttpContext.Current.Session["ID"]==null)
            {
                return;
            }
            else
            {
                l.UserId = (int)HttpContext.Current.Session["ID"];
            }

            db.LOG.Add(l);
            db.SaveChanges();
            base.OnActionExecuted(filterContext);
        }

        private string SerializeRequest(HttpRequestBase request)
        {
            string result = string.Empty;

            #region Form

            List<string> formVals = new List<string>();

            if (request.Form.AllKeys != null && request.Form.AllKeys.ToList().Count > 0)
            {
                foreach (string s in request.Form.AllKeys.ToList())
                {
                    formVals.Add(request.Form[s]);
                }
            }

            #endregion

            result = Json.Encode(new
            {
                request.Form,
                formVals,
                request.Browser.Browser,
                request.Browser.IsMobileDevice,
                request.Browser.Version,
                request.UserAgent,
                request.UserLanguages,
                request.UrlReferrer
            });
            return result;

        }

    }
}