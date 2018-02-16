using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TP.BusinessLayer.JobManagers;
using TP.Entities;
using TP.Entities.ValueObjects.JobAnsViewModels;
using TP.Web.Models;
using TP.Web.ViewModels;

namespace TP.Web.Controllers
{
    public class Job_AnsController : Controller
    {
        TestJobManager testJobManager = new TestJobManager();
        JobAdvManager jobAdvManager = new JobAdvManager();
        JobAnsManager jobAnsManager = new JobAnsManager();
        // GET: Job_Ans
        public ActionResult Index()
        {
            Tester t = CurrentSession.User as Tester;
            List<Test_Job> MyTJs = new List<Test_Job>();
            foreach (var item in testJobManager.List())
            {
                if (item.job_testers.Where(x => x.UserId == t.UserId).FirstOrDefault() != null)
                {
                    MyTJs.Add(item);
                }

            }
            return View(MyTJs);
        }

        public ActionResult ListAnswers(int? id)
        {

            List<Job_Ans> MyTJs = new List<Job_Ans>();
            foreach (var item in jobAnsManager.List())
            {
                if (item.isSubmitted == true && item.test_job.JobAdvId == id.Value)
                {

                    MyTJs.Add(item);
                }

            }
            return View(MyTJs);
        }

        // GET: Job_Ans/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["login"] is Tester)
            {
                Test_Job tj = testJobManager.Find(x => x.TestJobID == id.Value);

                Job_Ans JA = tj.job_answers.Find(x => x.tester == Session["login"] as Tester);
                return View(JA);
            }
            if(Session["login"] is Test_Master)
            {
                Job_Ans JA = jobAnsManager.Find(x => x.JobAnsId == id.Value);
                return View(JA);
            }
            if (Session["login"] is Admin)
            {
                Job_Ans JA = jobAnsManager.Find(x => x.JobAnsId == id.Value);
                return View(JA);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult Details(Job_Ans job_ans)
        {

            Test_Master currentTM = Session["login"] as Test_Master;
            Job_Ans jobans= jobAnsManager.Find(x => x.JobAnsId==job_ans.JobAnsId);
            

            if (Session["login"] is Test_Master && jobans.test_job.job_test_master.UserId==currentTM.UserId)
            {
                jobans.conf = job_ans.conf;
                job_ans.State = "Onaylandı";
                jobAnsManager.Update(jobans);
                if (job_ans.conf)
                {
                    OkViewModel ntfobj = new OkViewModel()
                    {
                        Title = "Onay Başarılı",
                        RedirectingUrl = "/Job_Ans/ListAnswers/" + jobans.test_job.JobAdvId,
                        RedirectingTimeout = 1800
                    };
                    ntfobj.Items.Add(jobans.test_job.test_job_title + " " + "Görevine Tester" + jobans.tester.user_name + " " + jobans.tester.user_surname + " tarafından gönderilen " + jobans.ans_title + " başlıklı cevabın onay işlemi başarılı..");
                    return View("Ok", ntfobj);
                }
                else
                {
                    OkViewModel ntfobj = new OkViewModel()
                    {
                        Title = "Onay Kaldırma Başarılı",
                        RedirectingUrl = "/Job_Ans/ListAnswers/" + jobans.test_job.JobAdvId,
                        RedirectingTimeout = 1800
                    };
                    ntfobj.Items.Add(jobans.test_job.test_job_title + " " + "Görevine Tester" + jobans.tester.user_name + " " + jobans.tester.user_surname + " tarafından gönderilen " + jobans.ans_title + " başlıklı cevabın onay kaldırma işlemi başarılı..");
                    return View("Ok", ntfobj);
                }
                
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: Job_Ans/Create
        public ActionResult Create(int? id)
        {
            JobAnsViewModel JAVM = new JobAnsViewModel();

            JAVM.State = "Gönderilmedi";
            JAVM.testjob_id = testJobManager.Find(x => x.TestJobID == id.Value).TestJobID;
            return View(JAVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobAnsViewModel job_Ans, HttpPostedFileBase AdvImage)
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
                    string filename = $"ans_{rnd.Next()}.{AdvImage.ContentType.Split('/')[1]}";

                    AdvImage.SaveAs(Server.MapPath($"~/Images/Ans_Images/{filename}"));
                    pic = filename;
                }
                Test_Job tj = testJobManager.Find(x => x.TestJobID == job_Ans.testjob_id);

                Job_Ans ja = new Job_Ans()
                {
                    ans_desc = job_Ans.ans_desc,
                    ans_title = job_Ans.ans_title,
                    isSubmitted = job_Ans.isSubmitted,
                    ModifiedTime = DateTime.Now,
                    State = (job_Ans.isSubmitted) ? "Gönderildi" : "Kaydedildi",
                    SubmitTime = DateTime.Now,
                    test_job = tj,
                    tester = Session["login"] as Tester,
                    ans_screenshot = pic
                };
                tj.job_answers.Add(ja);
                testJobManager.Update(tj);
                return RedirectToAction("Index");
            }

            return View(job_Ans);
        }

        // GET: Job_Ans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tester t = Session["login"] as Tester;
            JobAnsEditViewModel JAEVM = new JobAnsEditViewModel();
            JAEVM.old_ansid = jobAnsManager.Find(x => x.test_job.TestJobID == id.Value && x.tester.UserId==t.UserId).JobAnsId;
            JAEVM.ans_title = jobAnsManager.Find(x=>x.JobAnsId==JAEVM.old_ansid).ans_title;
            JAEVM.testjob_id = jobAnsManager.Find(x => x.JobAnsId == JAEVM.old_ansid).test_job.TestJobID;
            JAEVM.ans_desc = jobAnsManager.Find(x => x.JobAnsId == JAEVM.old_ansid).ans_desc;
            return View(JAEVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobAnsEditViewModel job_Ans, HttpPostedFileBase AdvImage)
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
                    string filename = $"ans_{rnd.Next()}.{AdvImage.ContentType.Split('/')[1]}";

                    AdvImage.SaveAs(Server.MapPath($"~/Images/Ans_Images/{filename}"));
                    pic = filename;
                }
                Nullable<DateTime> d = new DateTime();
                Test_Job tj = testJobManager.Find(x => x.TestJobID == job_Ans.testjob_id);
                Job_Ans ja = jobAnsManager.Find(x => x.JobAnsId == job_Ans.old_ansid);
                ja.ans_title=job_Ans.ans_title;
                ja.ans_desc = job_Ans.ans_desc;
                ja.isSubmitted = job_Ans.isSubmitted;
                ja.State = (job_Ans.isSubmitted == true) ? "Gönderildi" : "Düzenlendi";
                ja.ModifiedTime = DateTime.Now;
                ja.SubmitTime = (job_Ans.isSubmitted == true) ? DateTime.Now : d;
                ja.ans_screenshot = (pic != null) ? pic : ja.ans_screenshot;
                jobAnsManager.Update(ja);
                return RedirectToAction("Index");
            }
            return View(job_Ans);
        }

        // GET: Job_Ans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job_Ans ja = jobAnsManager.Find(x => x.JobAnsId == id.Value);
            if (ja == null)
            {
                return HttpNotFound();
            }
            else
                return View(ja);

        }

        // POST: Job_Ans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            return RedirectToAction("Index");
        }


    }
}
