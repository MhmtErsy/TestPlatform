using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TP.BusinessLayer.JobManagers;
using TP.BusinessLayer.Results;
using TP.BusinessLayer.RoleManagers;
using TP.Entities;
using TP.Entities.ValueObjects.JobViewModels;
using TP.Entities.ValueObjects.TestJobViewModels;
using TP.Web.Models;
using TP.Web.ViewModels;

namespace TP.Web.Controllers
{
    public class Test_JobController : Controller
    {
        JobAdvManager jobAdvManager = new JobAdvManager();
        TesterManager testerManager = new TesterManager();
        TestJobManager testJobManager = new TestJobManager();
        TestMasterManager testMasterManager = new TestMasterManager();

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Test_Job test_job = testJobManager.Find(x => x.JobAdvId == id.Value);
            if (test_job == null)
            {
                return HttpNotFound();
            }

            return View(test_job);
        }

        public ActionResult LookTesterProfile(int? id)
        {
            TesterManager tm = new TesterManager();
            BusinessLayerResult<Tester> res = tm.GetTesterById(id.Value);

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

        public ActionResult EmployTesterForJob(int? id)
        {
            SelectTestJobTesterViewModel jobTester = new SelectTestJobTesterViewModel();
            if (id == null)
            {
                WarningViewModel erv = new WarningViewModel()
                {
                    Title = "HATA 404",
                    RedirectingTimeout = 2000,
                    RedirectingUrl = "/Job_Adv/Details/" + id
                };
                erv.Items.Add("Hatalı Parametre..!");
                return View("Warning", erv);
            }

            Test_Job tj = testJobManager.Find(x => x.JobAdvId == id.Value);
            if (tj != null && tj.job_testers != null)
            {



                Test_Job test_Job = testJobManager.Find(x => x.JobAdvId == id.Value);
                if (test_Job == null)
                {
                    return HttpNotFound();
                }
                List<Tester> ts = new List<Tester>();
                foreach (var item in testerManager.List(x => x.isReady))
                {
                    ts.Add(item);
                }
                jobTester.Adv_Id = id.Value;
                jobTester.Testers = ts;
                jobTester.Test_job = test_Job;
                jobTester.limit = test_Job.tester_limit;
                jobTester.jobtesters = test_Job.job_testers;
                jobTester.Remaining = test_Job.tester_limit - test_Job.job_testers.Count;
                return View(jobTester);


            }
            else
            {
                WarningViewModel erv = new WarningViewModel()
                {
                    Title = "HATA",
                    RedirectingTimeout = 2000,
                    RedirectingUrl = "/Job_Adv/Details/" + id
                };
                erv.Items.Add("Henüz bir kullanım senaryosu oluşturmadınız..!");
                return View("Warning", erv);
            }

        }

        [HttpPost]
        public ActionResult EmployTesterForJob(SelectTestJobTesterViewModel tjt)
        {
            
            if (ModelState.IsValid)
            {
                TesterManager tm = new TesterManager();
                if (tm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Test_Job tj = testJobManager.Find(x => x.JobAdvId == tjt.Adv_Id);
                if (tj == null)
                {
                    return HttpNotFound();
                }

                if (tj.job_testers.Count == tj.tester_limit)
                {
                    WarningViewModel erv = new WarningViewModel()
                    {
                        Title = "HATA",
                        RedirectingTimeout = 2000,
                        RedirectingUrl = "/Test_Job/EmployTesterForJob/" + tj.JobAdvId
                    };
                    erv.Items.Add("Görevin Tester Limiti Dolmuştur..!");
                    return View("Warning", erv);
                }
                else
                {
                    Tester selectedTM = testerManager.GetTesterById(tjt.selectedTesterId).Result;
                    selectedTM.isReady = false;
                    tj.job_testers.Add(selectedTM);
                    if (tj.job_testers.Count == tj.tester_limit)
                    {
                        tj.start_date = DateTime.Now;
                    }
                    if (testJobManager.Update(tj) > 0)
                    {
                        selectedTM.notifications.Add(new Notification() { notification = tj.test_job_title + " Görevine Atandınız.", link = "/Test_Job/Details/" + tj.JobAdvId, IsRead = false });
                        tm.Update(selectedTM);

                        OkViewModel OkntfyObj = new OkViewModel()
                        {
                            Title = "Atama Başarılı..",
                            RedirectingUrl = "/Job_Adv/Details/" + jobAdvManager.Find(y => y.JobAdvId == tjt.Adv_Id).JobAdvId,
                            RedirectingTimeout = 3000
                        };
                        OkntfyObj.Items.Add("Tester " + selectedTM.user_name + " " + selectedTM.user_surname + " Başarılı bir şekilde " + tj.test_job_title + " görevine atandı..!");
                        return View("Ok", OkntfyObj);
                    }
                }
            }
            return View(tjt);
        }

        public ActionResult Create(int? id)
        {
            TestJobRegisterViewModel test_job = new TestJobRegisterViewModel()
            {
                Adv_Title = jobAdvManager.Find(x => x.JobAdvId == id).job_adv_title,
                adv_id = id.Value,

            };
            return View(test_job);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestJobRegisterViewModel test_job)
        {
            if (ModelState.IsValid)
            {
                Job_Adv ja = jobAdvManager.Find(x => x.JobAdvId == test_job.adv_id);
                Test_Job TJ = new Test_Job()
                {
                    test_job_title = test_job.testjob_title,
                    description = test_job.testjob_desc,
                    tester_limit = test_job.tester_limit,
                    start_date = DateTime.Now,
                    JobAdvId = ja.JobAdvId,
                    Job_Adv = jobAdvManager.Find(x => x.JobAdvId == ja.JobAdvId),
                    finish_date = test_job.finish_Date,
                    job_test_master = (CurrentSession.User as Test_Master),
                    price = test_job.price,
                    job_customer = jobAdvManager.Find(x => x.JobAdvId == ja.JobAdvId).adv_customer

                };
                testJobManager.Insert(TJ);
                return RedirectToAction("Details", "Job_Adv", new { id = test_job.adv_id });
            }

            return View(test_job);
        }

        // GET: Test_Job/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test_Job tj = testJobManager.Find(x => x.JobAdvId == id);
            if (tj == null)
            {
                return HttpNotFound("Kullanım Senaryosu Bulunamadı..!");
            }

            return View(tj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Test_Job test_Job)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }
            testJobManager.Update(test_Job);
            return View(test_Job);
        }

        public ActionResult MyTestJobs()
        {
            return View(testJobManager.List(x => x.job_testers.Where(y => y == (CurrentSession.User as Tester)).FirstOrDefault() != null));
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test_Job tj = testJobManager.Find(x => x.JobAdvId == id.Value);
            if (tj == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Test_Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            return RedirectToAction("Index");
        }


    }
}
