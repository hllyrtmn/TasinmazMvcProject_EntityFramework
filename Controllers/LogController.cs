using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TasinmazMvc.Models;

namespace TasinmazMvc.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        private TasinmazDbEntities db = new TasinmazDbEntities();
        public ActionResult Index(string search, int sayfa=1)
        {
            if (search != null)
            {
                return View(db.LOG.Where(x => x.UrlAccessed.ToString() == search|| x.AddedDate.ToString()==search||x.ExecutionMs.ToString()==search||
                                              x.Data==search||x.IPAddress==search||x.UserId.ToString()==search).ToList().ToPagedList(sayfa,10));
            }
            else
            {

                return View(db.LOG.ToList().ToPagedList(sayfa, 100));
            }
        }
    }
}