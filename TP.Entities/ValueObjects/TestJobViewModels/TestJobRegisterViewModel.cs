using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.ValueObjects.TestJobViewModels
{
    public class TestJobRegisterViewModel
    {
        public int adv_id { get; set; }

        public string Adv_Title { get; set; }

        [DisplayName("Başlık:"), Required, StringLength(50)]
        public string testjob_title { get; set; }

        [DisplayName("İster:"), Required, StringLength(4000)]
        public string testjob_desc { get; set; }

        [DisplayName("Tester Limiti"), Required]
        public int tester_limit { get; set; }

        [DisplayName("Ücret"), Required]
        public decimal price { get; set; }

        [DisplayName("Bitiş Tarihi")]
        public Nullable<DateTime> finish_Date { get; set; }
    }
}
