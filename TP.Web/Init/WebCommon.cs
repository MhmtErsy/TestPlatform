using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP.Common;
using TP.Entities;

namespace TP.Web.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUserName()
        {
            if (HttpContext.Current.Session["login"] !=null)
            {
                User user = HttpContext.Current.Session["login"] as User;
                return user.user_name + " " + user.user_surname;
            }
            return null;
        }
    }
}