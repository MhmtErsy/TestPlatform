using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Entities;

namespace TP.BusinessLayer.JobManagers
{
    public class SelectAdvTestMasterViewModel
    {
        public int Adv_Id { get; set; }
        public List<Test_Master> testMasters { get; set; }
        public int selectedTestMasterId { get; set; }
    }
}
