using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApiDemoCommon;

namespace WebApiDemo.Controllers
{
    public class MvcDemoController : Controller //Inherits from Controller
    {
        //Returns result serialized as json
        public JsonResult AJsonResult()
        {
            return Json(new ExampleClass {ExampleProperty = "Something..."}, JsonRequestBehavior.AllowGet);
        }

        //Returns result serialized as xml
        public ActionResult AnXmlresult()
        {
            var obj = new ExampleClass { ExampleProperty = "Something..." };
            var xdoc = new XDocument();

            using (var writer = xdoc.CreateWriter())
            {
                var serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(writer, obj); //Serialize object to xml
            }

            return Content(xdoc.ToString(), "text/xml");
        }
    }
}
