using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.ValueObjects.JobViewModels
{
    public class JobAdvRegisterViewModel
    {
        [DisplayName("Başlık:"), Required, StringLength(50)]
        public string adv_title { get; set; }

        [DisplayName("Açıklama:"), Required, StringLength(4000)]
        public string adv_desc { get; set; }

        [DisplayName("Ödül Skor:"), Required]
        public int AwardScoreValue { get; set; }

        [DisplayName("Ücret ( ₺ )")]
        [Required(ErrorMessage = "Price is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal price { get; set; }



    }
}
