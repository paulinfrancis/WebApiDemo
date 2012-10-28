using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace WebApiDemo.Security
{
    //Basic auth attribute - decorate controller class or individual controller actions
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Check that the header contains authorization
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = UnauthorizedResponseMessage();
            }
            else //Auth exists in header
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                var username = decodedToken.Substring(0, decodedToken.IndexOf(":", StringComparison.Ordinal));
                var password = decodedToken.Substring(decodedToken.IndexOf(":", StringComparison.Ordinal) + 1);

                //Super secret password check
                if (username == password)
                {
                    var user = new User {Username = username};

                    HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(user), new string[] { });

                    base.OnActionExecuting(actionContext);
                }
                else //Invalid credentials
                {
                    actionContext.Response = UnauthorizedResponseMessage();
                }
            }
        }

        private HttpResponseMessage UnauthorizedResponseMessage()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            response.Headers.Add("WWW-Authenticate", "Basic");
            return response;
        }
    }


    public class ApiIdentity : IIdentity
    {
        public User User { get; private set; }

        public ApiIdentity(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            User = user;
        }

        public string Name
        {
            get { return User.Username; }
        }

        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }

    public class User
    {
        public string Username { get; set; }
    }
}