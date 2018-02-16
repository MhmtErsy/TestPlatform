using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    [Table("JobAdverts")]
    public class Job_Adv
    {
        public Job_Adv()
        {
            Applicants = new List<Applicant>();
            isPublished = false;
           
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobAdvId { get; set; }

        public virtual Customer adv_customer { get; set; }

        [DisplayName("Başlık"),Required,StringLength(50)]
        public string job_adv_title { get; set; }

        [DisplayName("İster"),Required, StringLength(4000)]
        public string job_adv { get; set; }

        [DisplayName("Onay")]
        public bool confirmation { get; set; }

        public string adv_picturepath { get; set; }

        [DisplayName("Ücret"), Required]
        public decimal price { get; set; }

        [DisplayName("Yayın Durumu")]
        public bool isPublished { get; set; }

        public virtual Test_Master SelectedTestMaster { get; set; }
        
        [DisplayName("Yayın Tarihi")]
        public Nullable<DateTime> publishDate { get; set; }

        [DisplayName("Bitiş Tarihi")]
        public Nullable<DateTime> finishDate { get; set; }
        

        public virtual List<Applicant> Applicants { get; set; }

        [DisplayName("Ödül Skor"), Required]
        public int awardScoreValue { get; set; }
    }
}
