using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TP.BusinessLayer;
using TP.BusinessLayer.JobManagers;
using TP.BusinessLayer.Results;
using TP.BusinessLayer.RoleManagers;
using TP.Entities;
using TP.Entities.ValueObjects;
using TP.Entities.ValueObjects.JobViewModels;
using TP.Web.ViewModels;

namespace TP.Web.Controllers
{
    public class Job_AdvController : Controller
    {
        private JobAdvManager jobAdvManager = new JobAdvManager();
        private JobApplicantManager jobAppManager = new JobApplicantManager();
        private TestMasterManager tm = new TestMasterManager();
        private TestJobManager tjm = new TestJobManager();
        private CustomerManager cm = new CustomerManager();
        private ReportManager RM = new ReportManager();
        private NotificationManager notificationManager = new NotificationManager();
        private TesterManager testerManager = new TesterManager();
        private PaymentManager paymentManager = new PaymentManager();

        public ActionResult Index()
        {
            return View(jobAdvManager.List());
        }

        public ActionResult JobsInTask()
        {
            return View(jobAdvManager.List(x => x.SelectedTestMaster != null));
        }

        public ActionResult MyJobs()
        {
            if (Session["login"] != null)
            {
                Customer c = Session["login"] as Customer;
                return View(jobAdvManager.List(x => x.adv_customer.UserId == c.UserId));
            }
            return View();
        }
        public ActionResult ListCustomers()
        {
            return View(cm.List());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id.Value);
            if (job_Adv == null)
            {
                return HttpNotFound();
            }
            return View(job_Adv);
        }

        public ActionResult UnPublishedJobs()
        {
            if (Session["login"] is Admin)
            {
                return View(jobAdvManager.List(x => x.isPublished == false));
            }
            else
            {
                return View("Error", new ErrorViewModel() { Title = "Yetki Dışı" });
            }
        }



        public ActionResult Applicants(int? id)
        {
            SelectAdvTestMasterViewModel advtestmaster = new SelectAdvTestMasterViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (jobAdvManager.Find(x => x.JobAdvId == id.Value).SelectedTestMaster != null)
            {
                WarningViewModel erv = new WarningViewModel()
                {
                    Title = "HATA",
                    RedirectingTimeout = 2000,
                    RedirectingUrl = "/Job_Adv/Details/" + id
                };
                erv.Items.Add("Görevde atanmış bir Test Master bulunmaktadır..!");
                return View("Warning", erv);
            }
            else
            {
                Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id.Value);
                if (job_Adv == null)
                {
                    return HttpNotFound();
                }
                List<Test_Master> tms = new List<Test_Master>();
                foreach (var item in jobAppManager.List(x => x.Job_Adv.JobAdvId == id.Value))
                {
                    tms.Add(item.Test_Master);
                }
                advtestmaster.Adv_Id = job_Adv.JobAdvId;
                advtestmaster.testMasters = tms;
                return View(advtestmaster);
            }

        }
        public ActionResult RemoveSelectedTestMaster(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Adv JA = jobAdvManager.Find(x => x.JobAdvId == id.Value);
            if (JA == null)
            {
                return HttpNotFound();
            }
            return View(JA);
        }


        [HttpPost, ActionName("RemoveSelectedTestMaster")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveSelectedTestMasterConfirmed(int id)
        {
            Job_Adv JA = jobAdvManager.Find(x => x.JobAdvId == id);
            string tm = JA.SelectedTestMaster.user_name + " " + JA.SelectedTestMaster.user_surname;
            JA.SelectedTestMaster = null;
            if (jobAdvManager.Update(JA) > 0)
            {
                OkViewModel OkntfyObj = new OkViewModel()
                {
                    Title = "Çıkarma İşlemi Başarılı..",
                    RedirectingUrl = "/Job_Adv/Details/" + id,
                    RedirectingTimeout = 3000
                };
                OkntfyObj.Items.Add("Test Master " + tm + " " + JA.job_adv_title + " görevinden çıkartıldı..!");
                return View("Ok", OkntfyObj);
            }
            else
            {
                ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    RedirectingUrl = "/Job_Adv/Details/" + id,
                };
                return View("Error", ErrnotifyObj);
            }

        }

        [HttpPost]
        public ActionResult Applicants(SelectAdvTestMasterViewModel tm)
        {
            if (ModelState.IsValid)
            {
                TestMasterManager tmm = new TestMasterManager();
                if (tm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == tm.Adv_Id);
                if (job_Adv == null)
                {
                    return HttpNotFound();
                }
                job_Adv.SelectedTestMaster = tmm.GetTestMasterById(tm.selectedTestMasterId).Result;

                if (jobAdvManager.Update(job_Adv) > 0)
                {
                    foreach (var item in jobAppManager.List(x => x.Job_Adv.JobAdvId == job_Adv.JobAdvId))
                    {
                        jobAppManager.Delete(item);
                    }
                    Test_Master selectedTM = tmm.GetTestMasterById(tm.selectedTestMasterId).Result;
                    OkViewModel OkntfyObj = new OkViewModel()
                    {
                        Title = "Atama Başarılı..",
                        RedirectingUrl = "/Job_Adv/Details/" + tm.Adv_Id,
                        RedirectingTimeout = 3000
                    };
                    OkntfyObj.Items.Add("Test Master " + selectedTM.user_name + " " + selectedTM.user_surname + " Başarılı bir şekilde " + job_Adv.job_adv_title + " görevine atandı..!");
                    return View("Ok", OkntfyObj);
                }
            }
            return View(tm);
        }

        public ActionResult LookTestMasterProfile(int? id)
        {
            TestMasterManager tm = new TestMasterManager();
            BusinessLayerResult<Test_Master> res = tm.GetTestMasterById(id.Value);
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

        public ActionResult LookCustomerProfile(int? id)
        {
            CustomerManager cm = new CustomerManager();
            BusinessLayerResult<Customer> res = cm.GetCustomerById(id.Value);
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

        public ActionResult Apply(int? id)
        {
            if (Session["login"] is Test_Master)
            {
                Test_Master applierTM = Session["login"] as Test_Master;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Eksik Parametre");
                }
                Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id);
                job_Adv.Applicants.Add(new Applicant() { Job_Adv = job_Adv, Test_Master = applierTM });
                jobAdvManager.Update(job_Adv);
                return RedirectToAction("Index");
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Başvuru Yetkiniz Bulunmamaktadır");
            }
        }
        public ActionResult MyMissions()
        {
            if (Session["login"] is Test_Master)
            {
                Test_Master authorizedTM = Session["login"] as Test_Master;
                List<Job_Adv> mymissionlist = new List<Job_Adv>();
                foreach (var item in jobAdvManager.List(x => x.SelectedTestMaster.UserId == authorizedTM.UserId))
                {
                    mymissionlist.Add(item);
                }
                if (mymissionlist == null)
                {
                    ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                    {
                        Title = "Görevli olduğunuz iş bulunmamaktadır",
                        RedirectingUrl = "/Job_Adv/Index/",
                        RedirectingTimeout = 1500
                    };
                    return View("Error", ErrnotifyObj);
                }
                return View(mymissionlist);
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Başvuru Yetkiniz Bulunmamaktadır");
            }
        }

        public ActionResult MyApplicants()
        {
            if (Session["login"] is Test_Master)
            {
                Test_Master applierTM = Session["login"] as Test_Master;
                List<Job_Adv> myapplicantlist = new List<Job_Adv>();
                foreach (var item in jobAppManager.List(x => x.Test_Master.UserId == applierTM.UserId))
                {
                    myapplicantlist.Add(item.Job_Adv);
                }
                if (myapplicantlist == null)
                {
                    ErrorViewModel ErrnotifyObj = new ErrorViewModel()
                    {
                        Title = "Başvurduğunuz İlan Bulunmamaktadır",
                        RedirectingUrl = "/Job_Adv/Index/",
                        RedirectingTimeout = 1500
                    };
                    return View("Error", ErrnotifyObj);
                }
                return View(myapplicantlist);
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Başvuru Yetkiniz Bulunmamaktadır");
            }
        }

        public ActionResult ApplyCancel(int? id)
        {
            if (Session["login"] is Test_Master)
            {
                Test_Master applierTM = Session["login"] as Test_Master;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Eksik Parametre");
                }
                Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id);
                job_Adv.Applicants.Remove(applierTM.Applicants.Where(x => x.Job_Adv == job_Adv).FirstOrDefault());
                jobAppManager.Delete(applierTM.Applicants.Where(x => x.Job_Adv == job_Adv).FirstOrDefault());
                return RedirectToAction("Index");
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Başvuru Yetkiniz Bulunmamaktadır");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult WriteReport(int? id)
        {
            Report R = new Report();
            R.Test_Job = tjm.Find(x => x.TestJobID == id.Value);
            R.TestJobID = id.Value;
            return View(R);
        }

        [HttpPost]
        public ActionResult WriteReport(Report model)
        {
            model.Test_Job = tjm.Find(x => x.TestJobID == model.TestJobID);
            if (ModelState.IsValid)
            {
                if (RM.Insert(model) > 0)
                {
                    return RedirectToAction("ShowReport",new { id = model.TestJobID });
                }
                else
                {
                    return View("Error", new ErrorViewModel() { Title = "HATA" });
                }

            }
            else
            {
                return View("Error", new ErrorViewModel() { Title = "Geçersiz girişler mevcut" });
            }
        }

        public ActionResult EditReport(int? id)
        {
            Report R = RM.Find(x => x.TestJobID == id.Value);

            return View(R);
        }

        [HttpPost]
        public ActionResult EditReport(Report model)
        {
            model.Test_Job = tjm.Find(x => x.TestJobID == model.TestJobID);
            Report R = RM.Find(x => x.ReportID == model.ReportID);
            R.report = model.report;
            R.report_title = model.report_title;
            R.TestJobID = model.TestJobID;
            R.Test_Job = model.Test_Job;

            if (ModelState.IsValid)
            {
                if (RM.Update(R) > 0)
                {
                    return RedirectToAction("ShowReport", new { id = model.TestJobID });
                }
                else
                {
                    return View("Error", new ErrorViewModel() { Title = "HATA" });
                }

            }
            else
            {
                return View("Error", new ErrorViewModel() { Title = "Geçersiz girişler mevcut" });
            }
        }

        public ActionResult ShowReport(int? id)
        {
            Report r = RM.Find(x => x.TestJobID == id.Value);
            ReportViewModel ReportModel = new ReportViewModel()
            {
                ReportID = r.ReportID,
                Job_AdvID = r.Test_Job.JobAdvId,
                Job_Adv = r.Test_Job.Job_Adv,
                Report = r,
                confirmation=r.Test_Job.Job_Adv.confirmation
            };
            if (r != null)
            {
                return View(ReportModel);
            }
            else
            {
                return View("Error", new ErrorViewModel() { Title = "Rapor mevcut değil.." });
            }

        }
        public ActionResult CustomerPayments()
        {

            //Testmasterlara verilecek ödemelerin bilgileri
            Customer c = Session["login"] as Customer;
            List<Payment> customerPayments = new List<Payment>();
            foreach (var item in paymentManager.List())
            {
                if (item.Job_Adv.adv_customer == c && item.Job_Adv.confirmation == true && item.pay_user is Test_Master)
                {
                    customerPayments.Add(item);
                }
            }
            return View(customerPayments);
        }
        public ActionResult TestMasterPayments()
        {
            Test_Master t = Session["login"] as Test_Master;
            List<Payment> testmasterPayments = new List<Payment>();
            foreach (var item in paymentManager.List())
            {
                if (item.Job_Adv.SelectedTestMaster == t && item.Job_Adv.confirmation == true && item.pay_user is Test_Master)
                {

                    testmasterPayments.Add(item);
                }
            }
            return View(testmasterPayments);
        }
        public ActionResult TestMasterTestJobPayments()
        {
            Test_Master t = Session["login"] as Test_Master;

            List<Payment> testmasterPaymentsDB = paymentManager.ListQueryable().ToList();
            List<Payment> testmasterPayments = new List<Payment>();
            foreach (var item in testmasterPaymentsDB)
            {
                if (item.Job_Adv.SelectedTestMaster == t && item.Job_Adv.confirmation == true && item.pay_user is Tester)
                {

                    testmasterPayments.Add(item);
                }
            }
            return View(testmasterPayments);
        }
        public ActionResult TesterPayments(int? id)
        {
            Tester t = Session["login"] as Tester;
            List<Payment> testmasterPayments = new List<Payment>();
            foreach (var item in paymentManager.List())
            {
                if (item.pay_user == t && item.Job_Adv.confirmation == true && item.pay_user is Tester)
                {

                    testmasterPayments.Add(item);
                }
            }
            return View(testmasterPayments);
        }
        [HttpPost]
        public ActionResult ShowReport(ReportViewModel model)
        {
            model.Report = RM.Find(x => x.ReportID == model.ReportID);
            model.Job_Adv = jobAdvManager.Find(x => x.JobAdvId == model.Job_AdvID);

            Report r = RM.Find(x=>x.ReportID==model.ReportID);

            r.Test_Job.Job_Adv.confirmation = model.confirmation;
            r.Test_Job.Job_Adv.finishDate = DateTime.Now;
            foreach (var item in r.Test_Job.job_testers)
            {
                Tester t = testerManager.Find(x => x.UserId == item.UserId);
                t.score += r.Test_Job.Job_Adv.awardScoreValue;
                t.isReady = true;
                if (t.score>=0 && t.score<250)
                    t.rank.rank = "Çaylak";
                if (t.score >= 250 && t.score < 500)
                    t.rank.rank = "Tecrübeli";
                if (t.score >= 500)
                    t.rank.rank = "Uzman";
                Payment p = new Payment() {JobAdvId=r.Test_Job.JobAdvId,Job_Adv=r.Test_Job.Job_Adv,payment=r.Test_Job.price,payDate=DateTime.Now,pay_user=item };
                paymentManager.Insert(p);
                t.notifications.Add(new Notification() { link = "#", notification = r.Test_Job.test_job_title + " Görevinizden +" + r.Test_Job.Job_Adv.awardScoreValue + " kazandınız. Rütbe durumu: "+t.rank.rank, User = t });
                testerManager.Update(t);
            }
            Test_Master testmaster = tm.Find(x => x.UserId == r.Test_Job.job_test_master.UserId);
            testmaster.score += r.Test_Job.Job_Adv.awardScoreValue;
            tm.Update(testmaster);
            Payment p2 = new Payment() { JobAdvId = r.Test_Job.JobAdvId, Job_Adv = r.Test_Job.Job_Adv, payment = r.Test_Job.Job_Adv.price, payDate = DateTime.Now, pay_user = testmaster };
            paymentManager.Insert(p2);
            int resultUpdate = RM.Update(r);
            if (resultUpdate>=0)
            {
                string msj1 = "";
                string msj2 = "";
                if (r.Test_Job.Job_Adv.confirmation)
                {
                    msj1 = "Onay işlemi başarılı";
                    msj2 = r.Test_Job.Job_Adv.job_adv_title + " - " + r.report_title + " isimli rapor başarılı bir şekilde onaylandı..";
                    Notification n = new Notification() {notification=msj2,link= "/Job_Adv/ShowReport/" + r.TestJobID,User=r.Test_Job.job_test_master };
                    notificationManager.Insert(n);
                }
                else
                {
                    msj1 = "Onay kaldırma işlemi başarılı";
                    msj2 = r.Test_Job.Job_Adv.job_adv_title + " - " + r.report_title + " isimli rapor onayı kaldırıldı..";
                    Notification n = new Notification() { notification = msj2, link = "/Job_Adv/ShowReport/" + r.TestJobID, User = r.Test_Job.job_test_master };
                    notificationManager.Insert(n);
                }
                OkViewModel ntfobj = new OkViewModel()
                {
                    Title = msj1,
                    RedirectingUrl = "/Job_Adv/ShowReport/" + r.TestJobID,
                    RedirectingTimeout = 1200
                };
                ntfobj.Items.Add(msj2);
                return View("Ok", ntfobj);
            }
            else
            {
                return View("Error", new ErrorViewModel() { Title = "Rapor onay işlemi hatalı.." });
            }
            

        }
        //public ActionResult ConfirmReport(int? id)
        //{

        //}
        //public ActionResult ConfirmReport(Report model)
        //{

        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobAdvRegisterViewModel job_Adv, HttpPostedFileBase AdvImage)
        {
            Random rnd = new Random();
            string pic = null;
            if (ModelState.IsValid)
            {
                if (AdvImage != null &&
               (AdvImage.ContentType == "image/jpeg" ||
               AdvImage.ContentType == "image/jpg" ||
               AdvImage.ContentType == "image/png"))
                {
                    string filename = $"adv_{rnd.Next()}.{AdvImage.ContentType.Split('/')[1]}";

                    AdvImage.SaveAs(Server.MapPath($"~/Images/Adv_Images/{filename}"));
                    pic = filename;
                }

                jobAdvManager.Insert(new Job_Adv()
                {
                    adv_customer = Session["login"] as Customer,
                    job_adv_title = job_Adv.adv_title,
                    job_adv = job_Adv.adv_desc,
                    awardScoreValue = job_Adv.AwardScoreValue,
                    adv_picturepath = pic,
                    price = job_Adv.price


                });
                return RedirectToAction("MyJobs");
            }

            return View(job_Adv);
        }

        public ActionResult PublishAdv(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id.Value);
            if (job_Adv == null)
            {
                return HttpNotFound();
            }
            JobAdvEditViewModel jw = new JobAdvEditViewModel();
            jw.adv_title = job_Adv.job_adv_title;
            jw.adv_desc = job_Adv.job_adv;
            jw.AwardScoreValue = job_Adv.awardScoreValue;
            jw.adv_picturepath = job_Adv.adv_picturepath;
            jw.Id = job_Adv.JobAdvId;
            jw.adv_ispublished = job_Adv.isPublished;
            jw.price = job_Adv.price;
            return View(jw);
        }

        [HttpPost]
        public ActionResult PublishAdv(JobAdvEditViewModel job_Adv)
        {
            ModelState.Remove("adv_title");
            ModelState.Remove("adv_desc");
            ModelState.Remove("AwardScoreValue");

            if (ModelState.IsValid)
            {

                Job_Adv cat = jobAdvManager.Find(x => x.JobAdvId == job_Adv.Id);
                cat.isPublished = job_Adv.adv_ispublished;
                cat.publishDate = DateTime.Now;
                jobAdvManager.Update(cat);
                Customer c = cm.GetCustomerById(cat.adv_customer.UserId).Result;
                c.notifications.Add(new Notification { User = c, notification = cat.job_adv_title + " Adlı İlan Yayınınız Onaylandı..", link = "/Job_Adv/Details/" + cat.JobAdvId, IsRead = false });
                cm.Update(c);
                return RedirectToAction("Index");
            }
            return View(job_Adv);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id.Value);
            if (job_Adv == null)
            {
                return HttpNotFound();
            }
            JobAdvEditViewModel jw = new JobAdvEditViewModel();
            jw.adv_title = job_Adv.job_adv_title;

            jw.adv_desc = job_Adv.job_adv;
            jw.AwardScoreValue = job_Adv.awardScoreValue;
            jw.adv_picturepath = job_Adv.adv_picturepath;
            jw.Id = job_Adv.JobAdvId;
            jw.price = job_Adv.price;
            return View(jw);
        }


        [HttpPost]
        public ActionResult Edit(JobAdvEditViewModel job_Adv, HttpPostedFileBase AdvImage)
        {
            Random rnd = new Random();
            string pic = null;
            if (ModelState.IsValid)
            {
                if (AdvImage != null &&
               (AdvImage.ContentType == "image/jpeg" ||
               AdvImage.ContentType == "image/jpg" ||
               AdvImage.ContentType == "image/png"))
                {
                    string filename = $"adv_{rnd.Next()}.{AdvImage.ContentType.Split('/')[1]}";

                    AdvImage.SaveAs(Server.MapPath($"~/Images/Adv_Images/{filename}"));
                    pic = filename;
                }

                Job_Adv cat = jobAdvManager.Find(x => x.JobAdvId == job_Adv.Id);
                job_Adv.adv_picturepath = pic;
                cat.job_adv_title = job_Adv.adv_title;
                cat.job_adv = job_Adv.adv_desc;
                cat.price = job_Adv.price;
                cat.awardScoreValue = job_Adv.AwardScoreValue;
                jobAdvManager.Update(cat);

                return RedirectToAction("Index");
            }
            return View(job_Adv);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id.Value);
            if (job_Adv == null)
            {
                return HttpNotFound();
            }
            return View(job_Adv);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job_Adv job_Adv = jobAdvManager.Find(x => x.JobAdvId == id);
            List<Applicant> willdelete = jobAppManager.List(x => x.Job_Adv.JobAdvId == id).ToList();
            if (jobAdvManager.Delete(job_Adv) > 0)
            {
                foreach (var item in willdelete)
                {
                    jobAppManager.Delete(item);
                }
            }

            return RedirectToAction("Index");
        }


    }
}
