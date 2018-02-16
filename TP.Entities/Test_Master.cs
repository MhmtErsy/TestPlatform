using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    [Table("TestMasters")]
    public class Test_Master : User
    {
        public Test_Master()
        {
            Applicants = new List<Applicant>();
        }
        [DisplayName("Banka Hesap Bilgisi")]
        public string bank_account { get; set; }

        public virtual List<Applicant> Applicants { get; set; }

        [DisplayName("Skor")]
        public int score { get; set; }
    }
}
