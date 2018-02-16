using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP.Entities;

namespace TP.Web.Models
{
    public class CurrentSession
    {

        public static User User
        {
            get
            {
                if (HttpContext.Current.Session["login"] != null)
                {
                    if (HttpContext.Current.Session["login"] is Tester)
                    {
                        return HttpContext.Current.Session["login"] as Tester;
                    }
                    if (HttpContext.Current.Session["login"] is Test_Master)
                    {
                        return HttpContext.Current.Session["login"] as Test_Master;
                    }
                    if (HttpContext.Current.Session["login"] is Admin)
                    {
                        return HttpContext.Current.Session["login"] as Admin;
                    }
                    else
                    {
                        return HttpContext.Current.Session["login"] as Customer;
                    }
                }
                else
                    return null;
            }
        }
        public static void Set<T>(string key,T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }
        
        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T)HttpContext.Current.Session[key];
            }
            return default(T);
        }
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}