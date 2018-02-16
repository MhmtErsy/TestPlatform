using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.DataAccessLayer.EntityFramwork;
using TP.Entities;
using TP.Entities.ValueObjects.LoginViewModels;
using TP.Entities.ValueObjects.UserViewModels;
using TP.Entities.Messages;
using MyEvernote.Common.Helpers;
using TP.Common.Helpers;
using System.Net;
using TP.BusinessLayer.Results;
using TP.BusinessLayer.Abstract;

namespace TP.BusinessLayer.RoleManagers
{
    public class TesterManager : ManagerBase<Tester>
    {
        private Repository<User> repo_user = new Repository<User>();
        private Repository<Rank> repo_rank = new Repository<Rank>();
        private Repository<Notification> repo_not = new Repository<Notification>();
        private Repository<Test_Job> repo_test_job = new Repository<Test_Job>();
        private Repository<Job_Ans> repo_job_ans = new Repository<Job_Ans>();
        private Repository<Device> repo_device = new Repository<Device>();


        public static void ReturnNotTrue(User user)
        {
            Repository<Notification> repo_not = new Repository<Notification>();

            foreach (Notification n in repo_not.List().Where(x => x.User.UserId == user.UserId))
            {
                repo_not.List().FirstOrDefault(x => x.NotificationId == n.NotificationId).IsRead = true;
                repo_not.Save();
            }
        }
        public BusinessLayerResult<Tester> RegisterTester(TesterRegisterViewModel data)
        {

            User user = repo_user.Find(x => x.mail == data.mail);
            BusinessLayerResult<Tester> res = new BusinessLayerResult<Tester>();

            if (user != null)
            {
                if (user.mail == data.mail)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EMailAlreadyExists, "E-Posta adresi kayıtlı.");
                }
            }
            else
            {
                int dbResult = Insert(new Tester()
                {
                    user_name = data.user_name,
                    user_surname = data.user_surname,
                    score = 0,
                    mail = data.mail,
                    rank = repo_rank.Find(x => x.requiredMinScore >= 0 && x.requiredMinScore < 250),
                    user_picturepath = "user_def.png",
                    isReady=true,
                    password = data.password,
                    user_regdate = DateTime.Now,
                    ActivateGuid = Guid.NewGuid(),
                    notifications = new List<Notification>(),
                    
                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.mail == data.mail);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/TesterActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba yeni Tester {res.Result.user_name}; !<br><br>Hesabını aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";
                    MailHelper.SendMail(body, res.Result.mail, "Online Test Platform Hesap Aktifleştirme");
                }
            }
            return res;
        }
        private string GetIp()
        {
            var strHostName = "";
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            var addr = ipEntry.AddressList;
            return addr[2].ToString();
        }
        public BusinessLayerResult<Tester> LoginTester(TesterLoginViewModel data)
        {
            Tester t = Find(x => x.mail == data.email && x.password != data.password);
            if (t != null)
            {
                t.notifications.Add(new Notification { notification = "IP: " + GetIp() + " - HATALI GİRİŞ YAPILMIŞTIR",link="#" });
            }
            Tester tester = Find(x => x.mail == data.email && x.password == data.password);
            BusinessLayerResult<Tester> res = new BusinessLayerResult<Tester>();

            res.Result = tester;
            if (tester != null)
            {
                if (!tester.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Hesap aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen E-Posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.EMailorPassWrong, "Kullanıcı adı ya da parola uyuşmuyor.");
            }
            return res;

        }

        public BusinessLayerResult<Tester> ActivateTester(Guid activateId)
        {
            BusinessLayerResult<Tester> res = new BusinessLayerResult<Tester>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir..");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı..");
            }
            return res;
        }
        
        public BusinessLayerResult<Tester> RemoveTesterById(int id)
        {
            BusinessLayerResult<Tester> res = new BusinessLayerResult<Tester>();
            Tester tester = Find(x => x.UserId == id);

            if (tester != null)
            {
                Job_Ans res_ja = repo_job_ans.List().Where(x => x.tester == tester).Where(x => x.isSubmitted == true).FirstOrDefault();
                if (res_ja != null)
                {
                    res.AddError(ErrorMessageCode.TesterHasUncompletedJob, "Tester'ın henüz tamamlanmamış" +
                        res_ja.test_job.test_job_title + " görevi vardır..!");
                    return res;
                }
                if (Delete(tester) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        

        public BusinessLayerResult<Tester> UpdateProfile(Tester data)
        {
            User db_user = repo_user.Find(x => x.UserId != data.UserId && (x.mail == data.mail));
            BusinessLayerResult<Tester> res = new BusinessLayerResult<Tester>();

            if (db_user != null && db_user.UserId != data.UserId)
            {
                if (db_user.mail == data.mail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.UserId == data.UserId);
            res.Result.mail = data.mail;
            res.Result.user_name = data.user_name;
            res.Result.user_surname = data.user_surname;
            res.Result.password = data.password;
            res.Result.user_name = data.user_name;
            res.Result.bank_account = data.bank_account;
            res.Result.user_picturepath = (data.user_picturepath != null) ? data.user_picturepath : res.Result.user_picturepath;
            if (res.Result.notifications == null)
            {
                res.Result.notifications = new List<Notification>();
            }
            Notification not = new Notification() { notification = "Profil Bilgileri Güncellendi", not_time = DateTime.Now, User = res.Result, IsRead = false ,link="/Home/ShowTesterProfile/"+res.Result.UserId};
            res.Result.notifications.Add(not);
            if (string.IsNullOrEmpty(data.user_picturepath) == false)
            {
                res.Result.user_picturepath = data.user_picturepath;
            }
            //if (data.device!=null)
            //{
            //    if (res.Result.device!=null)
            //    {
            //        repo_device.Delete(res.Result.device);
            //    }
            //    res.Result.device = data.device;
            //}
            if (Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<Tester> GetTesterById(int id)
        {
            BusinessLayerResult<Tester> res = new BusinessLayerResult<Tester>();
            res.Result = Find(x => x.UserId == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı..");
            }
            return res;
        }

    }
}
