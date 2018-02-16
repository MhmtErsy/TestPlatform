using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Entities;

namespace TP.BusinessLayer.JobManagers
{
    public class SelectTestJobTesterViewModel
    {
        public Test_Job Test_job { get; set; }
        public int Adv_Id { get; set; }
        public List<Tester> Testers { get; set; }
        public int selectedTesterId { get; set; }
        public int Remaining { get; set; }
        public int limit { get; set; }
        public List<Tester> jobtesters { get; set; }
    }
}
