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
            byte[] bytes = new Domain.Codec(txt).CodificarTexto();
            return Json(new {encodedText=bytes}, JsonRequestBehavior.AllowGet);
        }
    }
}