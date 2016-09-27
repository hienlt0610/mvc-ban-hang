using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models;

namespace WebBanHang.Core
{
    public class UserManager
    {
        public static Customer User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // The user is authenticated. Return the user from the forms auth ticket.
                    return ((CustomerPrincipal)(HttpContext.Current.User)).UserData;
                }
                else if (HttpContext.Current.Items.Contains("User"))
                {
                    // The user is not authenticated, but has successfully logged in.
                    return (Customer)HttpContext.Current.Items["User"];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}