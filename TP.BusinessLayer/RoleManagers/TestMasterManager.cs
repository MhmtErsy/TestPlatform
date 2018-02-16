using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.DataAccessLayer.EntityFramwork;
using TP.Entities;
using TP.Entities.ValueObjects.UserViewModels;
using TP.Entities.ValueObjects.LoginViewModels;
using TP.Entities.Messages;
using TP.Common.Helpers;
using MyEvernote.Common.Helpers;
using System.Net;
using TP.BusinessLayer.Results;
using TP.BusinessLayer.Abstract;

namespace TP.BusinessLayer.RoleManagers
{
    public class TestMasterManager:ManagerBase<Test_Master>
    {
        private static Repository<User> repo_user = new Repository<User>();
        private Repository<Test_Job> repo_test_job = new Repository<Test_Job>();
        
        public static void ReturnNotTrue(User user)
        {
            Repository<Notification> repo_not = new Repository<Notification>();

            foreach (Notification n in repo_not.List().Where(x=>x.User.UserId == user.UserId))
            {
                repo_not.List().FirstOrDefault(x => x.NotificationId == n.NotificationId).IsRead = true;

                repo_not.Save();
            }
        }
        public BusinessLayerResult<Test_Master> RegisterTestMaster(TestMasterRegisterViewModel data)
        {

            // Kullanıcı user kontrolü
            // Kullanıcı e-posta kontrolü
            // Kayıt işlemi
            // Aktivasyon e-postası gönderimi

            User user = repo_user.Find(x => x.mail == data.mail);
            BusinessLayerResult<Test_Master> res = new BusinessLayerResult<Test_Master>();

            if (user != null)
            {
                if (user.mail == data.mail)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EMailAlreadyExists, "E-Posta adresi kayıtlı.");
                }
            }
            else
            {
                int dbResult = Insert(new Test_Master()
                {
                    user_name = data.user_name,
                    user_surname = data.user_surname,
                    mail = data.mail,
                    password = data.password,
                    user_picturepath="user_def.png",
                    user_regdate = DateTime.Now,
                    score=0,
                    ActivateGuid = Guid.NewGuid(),
                    notifications=new List<Notification>()
                });

                if (dbResult >= 1)
                {
                    res.Result = Find(x => x.mail == data.mail);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/TestMasterActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba yeni Test Master {res.Result.user_name}; !<br><br>Hesabını aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";
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
        public BusinessLayerResult<Test_Master> LoginTestMaster(TestMasterLoginViewModel data)
        {
            Test_Master tm = Find(x => x.mail == data.email && x.password != data.password);
            if (tm != null)
            {
                tm.notifications.Add(new Notification { notification = "IP: " + GetIp() + " - HATALI GİRİŞ YAPILMIŞTIR" ,link="#"});
            }
            Test_Master test_master = Find(x => x.mail == data.email && x.password == data.password);
            BusinessLayerResult<Test_Master> res = new BusinessLayerResult<Test_Master>();

            res.Result = test_master;
            if (test_master != null)
            {
                if (!test_master.IsActive)
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

        public BusinessLayerResult<Test_Master> ActivateTestMaster(Guid activateId)
        {
            BusinessLayerResult<Test_Master> res = new BusinessLayerResult<Test_Master>();
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

        public BusinessLayerResult<Test_Master> GetTestMasterById(int id)
        {
            BusinessLayerResult<Test_Master> res = new BusinessLayerResult<Test_Master>();
            res.Result = Find(x => x.UserId == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı..");
            }
            return res;
        }

        public BusinessLayerResult<Test_Master> UpdateProfile(Test_Master data)
        {
            User db_user = repo_user.Find(x => x.UserId != data.UserId && (x.mail == data.mail));
            BusinessLayerResult<Test_Master> res = new BusinessLayerResult<Test_Master>();

            if (db_user != null && db_user.UserId != data.UserId)
            {
                if (db_user.mail == data.mail)
                {
                    res.AddError(ErrorMessageCode.user_nameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.UserId == data.UserId)
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
            Notification not = new Notification() { notification = "Profil Bilgileri Güncellendi", not_time = DateTime.Now, User = res.Result, IsRead = false ,link="/Home/TestMasterManager/"+res.Result.UserId};
            res.Result.notifications.Add(not);
            if (string.IsNullOrEmpty(data.user_picturepath) == false)
            {
                res.Result.user_picturepath = data.user_picturepath;
            }

            if (Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }
        public BusinessLayerResult<Test_Master> RemoveTestMasterById(int id)
        {
            BusinessLayerResult<Test_Master> res = new BusinessLayerResult<Test_Master>();
            Test_Master test_master = Find(x => x.UserId == id);

            if (test_master != null)
            {
                Test_Job res_tj = repo_test_job.List().Where(x => x.job_test_master == test_master).Where(x => x.confirmation == false).FirstOrDefault();
                if (res_tj != null)
                {
                    res.AddError(ErrorMessageCode.TesterHasUncompletedJob, "Test Master'ın henüz tamamlanmamış" +
                        res_tj.test_job_title + " görevi vardır..!");
                    return res;
                }
                if (Delete(test_master) == 0)
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
    }
}
