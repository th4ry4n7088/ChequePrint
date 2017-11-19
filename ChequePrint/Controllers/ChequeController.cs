using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChequePrint.Controllers
{
    public class ChequeController : Controller
    {
        // GET: Cheque
        public ActionResult Index()
        {
            return View();
        }
    }
}