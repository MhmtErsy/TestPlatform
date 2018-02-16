using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.ValueObjects.UserViewModels
{
    public class AdminRegisterViewModel
    {
        [DisplayName("Ad"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(25)]
        public string user_name { get; set; }

        [DisplayName("Soyadı"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(25)]
        public string user_surname { get; set; }

        [DisplayName("E-Mail"), StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalı."), EmailAddress(ErrorMessage = "{0} alanı için lütfen geçerli bir e-posta adresi giriniz.")]
        public string mail { get; set; }

        [DisplayName("Parola"), DataType(DataType.Password), StringLength(15, MinimumLength = 5, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı.")]
        public string password { get; set; }

        [DisplayName("Parola Tekrar"), DataType(DataType.Password), StringLength(15, MinimumLength = 5, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı."),
            Compare("password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string Repassword { get; set; }
    }
}
