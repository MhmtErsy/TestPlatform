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
    [Table("TestJobs")]
    public class Test_Job
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestJobID { get; set; }


        public int JobAdvId { get; set; }
        [ForeignKey("JobAdvId")]
        public virtual Job_Adv Job_Adv { get; set; }


        [DisplayName("Test Başlığı"),Required,StringLength(50)]
        public string test_job_title { get; set; }

        [DisplayName("Test Açıklaması"),Required,StringLength(4000)]
        public string description { get; set; }

        [DisplayName("Başlangıç")]
        public Nullable<DateTime> start_date { get; set; }

        [DisplayName("Bitiş")]
        public Nullable<DateTime> finish_date { get; set; }

        [DisplayName("Gözden Geçirme")]
        public Nullable<DateTime> review_date { get; set; }

        [DisplayName("Ücret"),Required]
        public decimal price { get; set; }

        [DisplayName("Onay"),ScaffoldColumn(false)]
        public bool confirmation { get; set; }

        public virtual Test_Master job_test_master { get; set; }
        public virtual Customer job_customer { get; set; }

        [DisplayName("Durum"), ScaffoldColumn(false)]
        public string state { get; set; }

        [DisplayName("Tester Limiti"),Required]
        public int tester_limit { get; set; }

      //  public virtual Report Job_Report { get; set; }

        public virtual List<Tester> job_testers { get; set; }
        public virtual List<Job_Ans> job_answers { get; set; }

        public Test_Job()
        {
            job_testers = new List<Tester>();
            job_answers = new List<Job_Ans>();
        }
        
    }
}
