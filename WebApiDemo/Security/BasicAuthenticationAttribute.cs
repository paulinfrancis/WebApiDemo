﻿using System;
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
        //Run before action is run - we inspect the request's header to authenticate the user. If authentication fails, we return http 401 and request credentials.
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Check that the header contains authorization
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = UnauthorizedResponseMessage(); //No authorization in header - return 401
            }
            else //Authentication exists in header
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                var username = decodedToken.Substring(0, decodedToken.IndexOf(":", StringComparison.Ordinal));
                var password = decodedToken.Substring(decodedToken.IndexOf(":", StringComparison.Ordinal) + 1);

                //Super advanced password check - use membership provider, etc irl :)
                if (username == password)
                {
                    var user = new User { Username = username };

                    HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(user), new string[] { });

                    //Authorized - continue
                    base.OnActionExecuting(actionContext);
                }
                else //Invalid credentials
                {
                    actionContext.Response = UnauthorizedResponseMessage(); //return - 401 with WWW-Authenticate: Basic
                }
            }
        }

        private static HttpResponseMessage UnauthorizedResponseMessage()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            //Instructs client to provide credentials using basic authentication. 
            //Browsers understand this and prompt for username and password.
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