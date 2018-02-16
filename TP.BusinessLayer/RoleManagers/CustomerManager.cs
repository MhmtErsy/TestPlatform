using MyEvernote.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TP.BusinessLayer.Abstract;
using TP.BusinessLayer.Results;
using TP.Common.Helpers;
using TP.DataAccessLayer.EntityFramwork;
using TP.Entities;
using TP.Entities.Messages;
using TP.Entities.ValueObjects.LoginViewModels;
using TP.Entities.ValueObjects.UserViewModels;

namespace TP.BusinessLayer.RoleManagers
{
    public class CustomerManager : ManagerBase<Customer>
    {
        private Repository<User> repo_user = new Repository<User>();
        private Repository<Test_Job> repo_test_job = new Repository<Test_Job>();

        public static void ReturnNotTrue(User user)
        {
            Repository<Notification> repo_not = new Repository<Notification>();

            foreach (Notification n in repo_not.List().Where(x => x.User.UserId == user.UserId))
            {
                repo_not.List().FirstOrDefault(x => x.NotificationId == n.NotificationId).IsRead = true;
                repo_not.Save();
            }
        }
        public BusinessLayerResult<Customer> RegisterCustomer(CustomerRegisterViewModel data)
        {

            // Kullanıcı user kontrolü
            // Kullanıcı e-posta kontrolü
            // Kayıt işlemi
            // Aktivasyon e-postası gönderimi

            User user = repo_user.Find(x => x.mail == data.mail);
            BusinessLayerResult<Customer> res = new BusinessLayerResult<Customer>();

            if (user != null)
            {
                if (user.mail == data.mail)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.EMailAlreadyExists, "E-Posta adresi kayıtlı.");
                }
            }
            else
            {
                int dbResult = Insert(new Customer()
                {
                    user_name = data.user_name,
                    user_surname = data.user_surname,
                    mail = data.mail,
                    user_picturepath = "user_def.png",
                    password = data.password,
                    user_regdate = DateTime.Now,
                    ActivateGuid = Guid.NewGuid(),
                    notifications=new List<Notification>()
                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.mail == data.mail);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/CustomerActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.user_name}; !<br><br>Hesabını aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";
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
        public BusinessLayerResult<Customer> LoginCustomer(CustomerLoginViewModel data)
        {
            Customer c = Find(x => x.mail == data.email && x.password != data.password);
            if (c != null)
            {
                c.notifications.Add(new Notification { notification = "IP: " + GetIp() + " - HATALI GİRİŞ YAPILMIŞTIR", link = "#" });
            }
            Customer customer = Find(x => x.mail == data.email && x.password == data.password);
            BusinessLayerResult<Customer> res = new BusinessLayerResult<Customer>();

            res.Result = customer;
            if (customer != null)
            {
                if (!customer.IsActive)
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

        public BusinessLayerResult<Customer> ActivateCustomer(Guid activateId)
        {
            BusinessLayerResult<Customer> res = new BusinessLayerResult<Customer>();
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

        public BusinessLayerResult<Customer> GetCustomerById(int id)
        {
            BusinessLayerResult<Customer> res = new BusinessLayerResult<Customer>();
            res.Result = Find(x => x.UserId == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı..");
            }
            return res;
        }

        public BusinessLayerResult<Customer> UpdateProfile(Customer data)
        {
            User db_user = repo_user.Find(x => x.UserId != data.UserId && (x.mail == data.mail));
            BusinessLayerResult<Customer> res = new BusinessLayerResult<Customer>();

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
            res.Result.user_picturepath = (data.user_picturepath != null) ? data.user_picturepath : res.Result.user_picturepath;
            Notification not = new Notification() { notification = "Profil Bilgileri Güncellendi", not_time = DateTime.Now, User = res.Result, IsRead = false ,link="/Home/ShowCustomerProfile/"+res.Result.UserId};

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

        public BusinessLayerResult<Customer> RemoveCustomerById(int id)
        {
            BusinessLayerResult<Customer> res = new BusinessLayerResult<Customer>();
            Customer customer = Find(x => x.UserId == id);

            if (customer != null)
            {
                Test_Job res_tj = repo_test_job.List().Where(x => x.job_customer == customer).Where(x => x.confirmation == false).FirstOrDefault();
                if (res_tj != null)
                {
                    res.AddError(ErrorMessageCode.TesterHasUncompletedJob, "Müşteri'nin henüz tamamlanmamış" +
                        res_tj.test_job_title + " görevi vardır..!");

                    return res;
                }
                if (Delete(customer) == 0)
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
