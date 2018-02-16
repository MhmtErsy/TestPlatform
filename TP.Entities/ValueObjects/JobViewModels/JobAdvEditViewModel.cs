using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.ValueObjects.JobViewModels
{
    public class JobAdvEditViewModel
    {

        [DisplayName("Başlık"), Required, StringLength(50)]
        public string adv_title { get; set; }

        [DisplayName("İster"), Required, StringLength(4000)]
        public string adv_desc { get; set; }

        [DisplayName("Ödül Skor"), Required]
        public int AwardScoreValue { get; set; }

        [DisplayName("Resim")]
        public string adv_picturepath { get; set; }

        [DisplayName("Yayın Durumu")]
        public bool adv_ispublished { get; set; }

        [DisplayName("Yayın Tarihi")]
        public DateTime adv_publishdate { get; set; }

        [DisplayName("Ücret")]
        [Required(ErrorMessage = "Price is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal price { get; set; }

        public int Id { get; set; }
    }
}
