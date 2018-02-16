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
    [Table("Reports")]
    public class Report
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportID { get; set; }

        
       public int TestJobID { get; set; }
        [ForeignKey("TestJobID")]
        public virtual Test_Job Test_Job { get; set; }

        [DisplayName("Rapor Başlığı"),Required,StringLength(150)]
        public string report_title { get; set; }

        [DisplayName("Rapor İçeriği"),Required, StringLength(4000)]
        public string report { get; set; }

      //  public virtual Test_Master ownerTestMaster { get; set; }

        
    }
}
