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

        public JsonResult EncodeFile(string txt)
        {
            byte[] bytes = new Domain.Codec(txt).CodificarTexto();
            return Json(new {encodedText=bytes}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DecodeFile(string txt)
        {
            string resultTxt = new Domain.Codec(txt).Decodificar();
            return Json(new { resultText=resultTxt }, JsonRequestBehavior.AllowGet);
        }
    }
}