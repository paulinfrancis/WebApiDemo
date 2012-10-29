using System.Web.Http;
using WebApiDemoCommon;

namespace WebApiDemo.Controllers
{
    public class ApiDemoController : ApiController //Inherits from ApiController
    {
        //Returns in specified format (accept: ), or defaults to json
        public ExampleClass Get()
        {
            return new ExampleClass { ExampleProperty = "Something..." };
        }
    }
}
