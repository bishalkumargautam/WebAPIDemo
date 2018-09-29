using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PragimWebAPIDemo
//for basic authentication we can create a class and have it derived from  AuthorizationFilterAttribute, we
//then need to override the OnAuthorization method 
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.
                    CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                //decoding the paswword and user name from authentication token that is in base 64 fromat.
                string decodedAuthenticationToken = Encoding.UTF8.
                    GetString(Convert.FromBase64String(authenticationToken));

                //username : passwrord ==> this is the foramt that the username and password is in so . it can be user
                //split to retreive username and password.
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                if (EmployeeSecurity.Login(username, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.
                        CreateResponse(HttpStatusCode.Unauthorized);

                }
            };
        }
    }
}