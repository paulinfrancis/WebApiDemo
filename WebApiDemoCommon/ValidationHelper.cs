using System;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace WebApiDemoCommon
{
    public class ValidationHelper
    {
        public static string GetModelErrors(ModelStateDictionary modelstate)
        {
            return string.Join(Environment.NewLine, modelstate.Select(e => e.Value.Errors.First().ErrorMessage));
        }
    }
}
