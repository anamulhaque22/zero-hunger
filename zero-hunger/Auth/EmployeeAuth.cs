using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zero_hunger.Auth
{
    public class EmployeeAuth: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["EmployeeEmail"] != null) return true;
            return false;
        }
    }
}