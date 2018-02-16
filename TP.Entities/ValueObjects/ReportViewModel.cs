using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.ValueObjects
{
    public class ReportViewModel
    {
        public int ReportID { get; set; }
        public Report Report { get; set; }

        public int Job_AdvID { get; set; }
        public Job_Adv Job_Adv { get; set; }

        [DisplayName("Onay Durumu ")]
        public bool confirmation { get; set; }
    }
}
