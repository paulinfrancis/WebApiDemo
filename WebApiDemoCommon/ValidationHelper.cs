using System;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace WebApiDemoCommon
{
    public static class ValidationHelper
    {
        public static string GetModelErrors(this ModelStateDictionary modelstate)
        {
            return string.Join(Environment.NewLine, modelstate.Select(e => e.Value.Errors.First().ErrorMessage));
        }
    }
}
