using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using TP.BusinessLayer.DeviceManagers;
using TP.BusinessLayer.RoleManagers;
using TP.Entities;

namespace TP.Web.Models
{
    public class CacheHelper
    {
        TesterManager TM = new TesterManager();
        DeviceManager dm = new DeviceManager();
        
        public List<Brand> GetBrandsFromCache()
        {
            BrandManager brandManager = new BrandManager();
            var result = brandManager.List();
            return result;
        }
        public List<Model> GetModelsFromCache()
        {
            ModelManager modelManager = new ModelManager();
            var result = modelManager.List();
            return result;
        }
        public List<Browser> GetBrowsersFromCache()
        {
            BrowserManager browserManager = new BrowserManager();
            var result = browserManager.List();
            return result;
        }
        public List<OS> GetOSFromCache()
        {
            OSManager osManager = new OSManager();
            var result = osManager.List();
            return result;
        }
        public List<Entities.Type> GetTypesFromCache()
        {
            TypeManager TypeManager = new TypeManager();
            var result = TypeManager.List();
            return result;
        }
    }
}