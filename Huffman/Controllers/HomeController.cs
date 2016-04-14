using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Huffman.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public JsonResult ProcessFile(string txt)
        {
            txt = txt.ToLower() + "ALFACE";
            return Json(new {encodedText=txt}, JsonRequestBehavior.AllowGet);
        }
    }
}