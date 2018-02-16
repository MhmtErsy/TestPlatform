using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TP.BusinessLayer;
using TP.BusinessLayer.DeviceManagers;
using TP.BusinessLayer.JobManagers;
using TP.BusinessLayer.Results;
using TP.BusinessLayer.RoleManagers;
using TP.DataAccessLayer.EntityFramwork;
using TP.Entities;
using TP.Entities.Messages;
using TP.Entities.ValueObjects.LoginViewModels;
using TP.Entities.ValueObjects.UserViewModels;
using TP.Web.Models;
using TP.Web.ViewModels;
using System.Web.Script.Serialization;

namespace TP.Web.Controllers
{
    public class HomeController : Controller
    {


        DeviceManager DM = new DeviceManager();
        public ActionResult Index()
        {
            BusinessLayer.Test t = new BusinessLayer.Test();
            if (Session["login"]!=null)
            {
                JobAdvManager jm = new JobAdvManager();
                return View(jm.List().OrderByDescending(x => x.publishDate).ToList());
            }
            else
            {
                return RedirectToAction("TesterLogin");
            }
            
        }

        public ActionResult MostAwardScore()
        {
            JobAdvManager jm = new JobAdvManager();
            return View("Index", jm.ListQueryable().OrderByDescending(x => x.awardScoreValue).ToList());
        }
        public ActionResult About()
        {
            return View();
        }

       

        
        #region Logins
        #region CustomerLogin
        [HttpGet]
        public ActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerLogin(CustomerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerManager cm = new CustomerManager();
                BusinessLayerResult<Customer> res = cm.LoginCustomer(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://localhost:53605/Home/CustomerActivate/" + res.Result.ActivateGuid.ToString();
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["login"] = res.Result;   
                return RedirectToAction("Index");  
            }
            return View();
        }

        #endregion
        #region TesterLogin
        [HttpGet]
        public ActionResult TesterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TesterLogin(TesterLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                TesterManager tm = new TesterManager();
                BusinessLayerResult<Tester> res = tm.LoginTester(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://localhost:53605/Home/TesterActivate/" + res.Result.ActivateGuid.ToString();
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["login"] = res.Result;   //Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index");   //Yönlendirme

            }

            return View();
        }
        #endregion
        #region TestMasterLogin
        [HttpGet]
        public ActionResult TestMasterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestMasterLogin(TestMasterLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                TestMasterManager tm = new TestMasterManager();
                BusinessLayerResult<Test_Master> res = tm.LoginTestMaster(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://localhost:53605/Home/TestMasterActivate/" + res.Result.ActivateGuid.ToString();
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["login"] = res.Result;   //Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index", "Job_Adv");   //Yönlendirme

            }

            return View();
        }
        #endregion

        #region AdminLogin
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminManager tm = new AdminManager();
                BusinessLayerResult<Admin> res = tm.LoginAdmin(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://localhost:53605/Home/AdminActivate/" + res.Result.ActivateGuid.ToString();
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                Session["login"] = res.Result;   //Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index");   //Yönlendirme

            }

            return View();
        }

        #endregion
        #endregion

        #region Registers
        
        #region TesterRegister
        [HttpGet]
        public ActionResult TesterRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TesterRegister(TesterRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                TesterManager tm = new TesterManager();
                BusinessLayerResult<Tester> res = tm.RegisterTester(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel ntfobj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/TesterLogin",

                };
                ntfobj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz..");
                return View("Ok", ntfobj);
            }
            return View(model);
        }
        #endregion
        #region CustomerRegister
        [HttpGet]
        public ActionResult CustomerRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerRegister(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                CustomerManager cm = new CustomerManager();
                BusinessLayerResult<Customer> res = cm.RegisterCustomer(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkViewModel ntfobj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/CustomerLogin",

                };
                ntfobj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz..");
                return View("Ok", ntfobj);
            }
            return View(model);
        }
        #endregion
        #region TestMasterRegister
        [HttpGet]
        public ActionResult TestMasterRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestMasterRegister(TestMasterRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                TestMasterManager tmm = new TestMasterManager();
                BusinessLayerResult<Test_Master> res = tmm.RegisterTestMaster(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel ntfobj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Index",

                };
                ntfobj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz..");
                return View("Ok", ntfobj);
            }
            return View(model);
        }
        #endregion
        #region AdminRegister
        [HttpGet]
        public ActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminRegister(AdminRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminManager adm = new AdminManager();
                BusinessLayerResult<Admin> res = adm.RegisterAdmin(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel ntfobj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/AdminLogin",

                };
                ntfobj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz..");
                return View("Ok", ntfobj);
            }
            return View(model);
        }
        #endregion
        #endregion

        #region ShowAndEditProfiles
        #region Tester
        public ActionResult ShowTesterProfile()
        {
            Tester CurrentTester = (Session["login"] as Tester);
            //TesterManager tm = new TesterManager();
            
            return View(CurrentTester);
        }

        public ActionResult DeleteTesterDevice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = DM.Find(x => x.DeviceId == id.Value);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }


        [HttpPost, ActionName("DeleteTesterDevice")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = DM.Find(x => x.DeviceId == id);
            if (DM.Delete(device) > 0)
            {
                
            }

            return RedirectToAction("ShowTesterProfile");
        }

        CacheHelper CH = new CacheHelper();
        public ActionResult CreateTesterDevice()
        {
            if (Session["login"] is Tester)
            {
                ViewBag.BrandId = new SelectList(CH.GetBrandsFromCache(), "BrandId", "BrandName");
                ViewBag.ModelId = new SelectList(CH.GetModelsFromCache(), "ModelId", "ModelName");
                ViewBag.BrowserId = new SelectList(CH.GetBrowsersFromCache(), "BrowserId", "BrowserName");
                ViewBag.OSId = new SelectList(CH.GetOSFromCache(), "OSId", "OSName");
                ViewBag.TypeId = new SelectList(CH.GetTypesFromCache(), "TypeId", "TypeName");
            }
            

            return View();
        }
     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTesterDevice(Device device)
        {
            TesterManager tm = new TesterManager();
            if (ModelState.IsValid)
            {
                Tester t = (Session["login"] as Tester);
                t.Device = device;
                if (DM.Insert(device) > 0 && tm.Update(t)>0)
                {
                    return View(device);
                }
                CurrentSession.Set<Tester>("login", t);
                
                ViewBag.BrandId = new SelectList(CH.GetBrandsFromCache(), "BrandId", "BrandName", device.BrandId);
                ViewBag.ModelId = new SelectList(CH.GetModelsFromCache(), "ModelId", "ModelName", device.ModelId);
                ViewBag.BrowserId = new SelectList(CH.GetBrowsersFromCache(), "BrowserId", "BrowserName", device.Model.BrowserId);
                ViewBag.OSId = new SelectList(CH.GetOSFromCache(), "OSId", "OSName", device.Model.OSId);
                ViewBag.TypeId = new SelectList(CH.GetTypesFromCache(), "TypeId", "TypeName", device.Model.Type.TypeId);

                return RedirectToAction("CreateTesterDevice");
            }
            return View(device);
        }

        public ActionResult EditTesterProfile()
        {
            Tester currentTester = Session["login"] as Tester;
            TesterManager tm = new TesterManager();
            BusinessLayerResult<Tester> res = tm.GetTesterById(currentTester.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditTesterProfile(Tester tester, HttpPostedFileBase ProfileImage)
        {
            TesterManager tm = new TesterManager();
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
               (ProfileImage.ContentType == "image/jpeg" ||
               ProfileImage.ContentType == "image/jpg" ||
               ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{tester.UserId}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/User_Images/{filename}"));
                    tester.user_picturepath = filename;
                }
                BusinessLayerResult<Tester> res = tm.UpdateProfile(tester);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel Errmsgs = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditTesterProfile"
                    };
                    return View("Error", Errmsgs);
                }
                CurrentSession.Set<Tester>("login", res.Result);

                return RedirectToAction("ShowTesterProfile");
            }
            return View(tm.GetTesterById(tester.UserId).Result);

        }
        #endregion
        #region Customer
        public ActionResult ShowCustomerProfile()
        {
            Customer currentCustomer = Session["login"] as Customer;
            CustomerManager tm = new CustomerManager();
            BusinessLayerResult<Customer> res = tm.GetCustomerById(currentCustomer.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditCustomerProfile()
        {
            Customer currentCustomer = Session["login"] as Customer;
            CustomerManager cm = new CustomerManager();
            BusinessLayerResult<Customer> res = cm.GetCustomerById(currentCustomer.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditCustomerProfile(Customer customer, HttpPostedFileBase ProfileImage)
        {
            CustomerManager cm = new CustomerManager();
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{customer.UserId}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/User_Images/{filename}"));
                    customer.user_picturepath = filename;
                }

                BusinessLayerResult<Customer> res = cm.UpdateProfile(customer);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel Errmsgs = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditCustomerProfile"
                    };
                    return View("Error", Errmsgs);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowCustomerProfile");
            }
            return View(cm.GetCustomerById(customer.UserId).Result);
        }
        #endregion
        #region TestMaster
        public ActionResult ShowTestMasterProfile()
        {
            Test_Master currentTestMaster = Session["login"] as Test_Master;
            TestMasterManager tm = new TestMasterManager();
            BusinessLayerResult<Test_Master> res = tm.GetTestMasterById(currentTestMaster.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditTestMasterProfile()
        {
            Test_Master currentTestMaster = Session["login"] as Test_Master;
            TestMasterManager cm = new TestMasterManager();
            BusinessLayerResult<Test_Master> res = cm.GetTestMasterById(currentTestMaster.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditTestMasterProfile(Test_Master test_Master, HttpPostedFileBase ProfileImage)
        {
            TestMasterManager tm = new TestMasterManager();
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{test_Master.UserId}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/User_Images/{filename}"));
                    test_Master.user_picturepath = filename;
                }
                BusinessLayerResult<Test_Master> res = tm.UpdateProfile(test_Master);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel Errmsgs = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditTestMasterProfile"
                    };
                    return View("Error", Errmsgs);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowTestMasterProfile");
            }
            return View(tm.GetTestMasterById(test_Master.UserId).Result);
        }
        #endregion
        #region Admin
        public ActionResult ShowAdminProfile()
        {
            Admin currentAdmin = Session["login"] as Admin;
            AdminManager tm = new AdminManager();
            BusinessLayerResult<Admin> res = tm.GetAdminById(currentAdmin.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditAdminProfile()
        {
            Admin currentAdmin = Session["login"] as Admin;
            AdminManager adm = new AdminManager();
            BusinessLayerResult<Admin> res = adm.GetAdminById(currentAdmin.UserId);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditAdminProfile(Admin admin, HttpPostedFileBase ProfileImage)
        {
            AdminManager adm = new AdminManager();
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
              (ProfileImage.ContentType == "image/jpeg" ||
              ProfileImage.ContentType == "image/jpg" ||
              ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{admin.UserId}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/User_Images/{filename}"));
                    admin.user_picturepath = filename;
                }
                BusinessLayerResult<Admin> res = adm.UpdateProfile(admin);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel Errmsgs = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditAdminProfile"
                    };
                    return View("Error", Errmsgs);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowAdminProfile");
            }
            return View(adm.GetAdminById(admin.UserId).Result);
        }
        #endregion
        #endregion

        #region DeleteProfiles
        #region DeleteTester
        public ActionResult DeleteTesterProfile()
        {
            Tester currentTester = Session["login"] as Tester;
            TesterManager tm = new TesterManager();
            BusinessLayerResult<Tester> res = tm.RemoveTesterById(currentTester.UserId);
            if (res.Errors.Count > 0)
            {


                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Hesap Silinemedi",
                    RedirectingUrl = "/Home/ShowTesterProfile/"

                };
                
                return View("Error", ErrnotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion
        #region DeleteTestMaster
        #endregion
        #region 
        #endregion
        #region DeleteAdmin
        #endregion
        #endregion

        #region Activates
        #region Tester
        public ActionResult TesterActivate(Guid id)
        {
            TesterManager tm = new TesterManager();
            BusinessLayerResult<Tester> res = tm.ActivateTester(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            OkViewModel OkntfyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştiridi.",
                RedirectingUrl = "/Home/TesterLogin"

            };
            OkntfyObj.Items.Add("Hesabınız Aktifleştiridi.. Aramıza Hoşgeldin !");
            return View("Ok", OkntfyObj);
        }
        #endregion
        #region Customer
        public ActionResult CustomerActivate(Guid id)
        {
            CustomerManager cm = new CustomerManager();
            BusinessLayerResult<Customer> res = cm.ActivateCustomer(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            OkViewModel OkntfyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştiridi.",
                RedirectingUrl = "/Home/CustomerLogin/"

            };
            OkntfyObj.Items.Add("Hesabınız Aktifleştiridi.. Aramıza Hoşgeldin !");
            return View("Ok", OkntfyObj);
        }
        #endregion
        #region TestMaster
        public ActionResult TestMasterActivate(Guid id)
        {
            TestMasterManager tmm = new TestMasterManager();
            BusinessLayerResult<Test_Master> res = tmm.ActivateTestMaster(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            OkViewModel OkntfyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştiridi.",
                RedirectingUrl = "/Home/Index"
            };
            OkntfyObj.Items.Add("Hesabınız Aktifleştiridi.. Aramıza Hoşgeldin !");
            return View("Ok", OkntfyObj);
        }
        #endregion
        #region Admin
        public ActionResult AdminActivate(Guid id)
        {
            AdminManager adm = new AdminManager();
            BusinessLayerResult<Admin> res = adm.ActivateAdmin(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
                return View("Error", ErrnotifyObj);
            }
            OkViewModel OkntfyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleştiridi.",
                RedirectingUrl = "/Home/AdminLogin"
            };
            OkntfyObj.Items.Add("Hesabınız Aktifleştiridi.. Aramıza Hoşgeldin !");
            return View("Ok", OkntfyObj);
        }
        #endregion
        #endregion
        public ActionResult Logout()
        {
            if (Session["login"] is Admin)
            {
                Session.Clear();
                return RedirectToAction("AdminLogin");
            }
            else if (Session["login"] is Tester)
            {
                Session.Clear();
                return RedirectToAction("TesterLogin");
            }
            else if (Session["login"] is Test_Master)
            {
                Session.Clear();
                return RedirectToAction("TestMasterLogin");
            }
            else if (Session["login"] is Customer)
            {
                Session.Clear();
                return RedirectToAction("CustomerLogin");
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Index");
            }

        }


    }
}