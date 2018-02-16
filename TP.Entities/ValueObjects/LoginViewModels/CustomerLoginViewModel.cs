using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP.Entities.ValueObjects.LoginViewModels
{
    public class CustomerLoginViewModel
    {
        [DisplayName("E-Mail"), StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalı."), EmailAddress(ErrorMessage = "{0} alanı için lütfen geçerli bir e-posta adresi giriniz.")]
        public string email { get; set; }

        [DisplayName("Parola"), DataType(DataType.Password), StringLength(15, MinimumLength = 5, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string password { get; set; }
    }
}