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
    [Table("JobAnswers")]
    public class Job_Ans
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobAnsId { get; set; }

        public virtual Test_Job test_job { get; set; }
        public virtual Tester tester { get; set; }

        [DisplayName("Cevap Başlık"),Required,StringLength(50)]
        public string ans_title { get; set; }

        [DisplayName("Cevap Açıklama"), Required,StringLength(4000)]
        public string ans_desc { get; set; }

        [DisplayName("Durum"), ScaffoldColumn(false)]
        public string State { get; set; }

        [DisplayName("Düzenleme Tarihi"), ScaffoldColumn(false)]
        public Nullable<DateTime> ModifiedTime { get; set; }

        [DisplayName("Gönderim Tarihi"), ScaffoldColumn(false)]
        public Nullable<DateTime> SubmitTime { get; set; }

        [DisplayName("Gönderim Durumu"), ScaffoldColumn(false)]
        public bool isSubmitted { get; set; }

        [DisplayName("Onay Durumu"), ScaffoldColumn(false)]
        public bool conf { get; set; }

        public string ans_screenshot { get; set; }
        

       

    }
}
