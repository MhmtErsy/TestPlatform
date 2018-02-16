using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    public class User
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [DisplayName("Ad"),Required,StringLength(25, ErrorMessage="{0} alanı max. {1} karakter olmalıdır..")]
        public string user_name { get; set; }

        [DisplayName("Soyad"), Required, StringLength(25 ,ErrorMessage = "{0} alanı max. {1} karakter olmalıdır..")]
        public string user_surname { get; set; }

        [DisplayName("E-Posta"), Required, StringLength(70 ,ErrorMessage = "{0} alanı max. {1} karakter olmalıdır..")]
        public string mail { get; set; }

        [DisplayName("Parola"), Required,StringLength(15,MinimumLength =5, ErrorMessage = "{0} alanı max. {1} - min. {2} karakter olmalıdır..")]
        public string password { get; set; }

        public string user_picturepath { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; }

        public DateTime user_regdate { get; set; }

        public virtual List<Notification> notifications { get; set; }
    }
}
