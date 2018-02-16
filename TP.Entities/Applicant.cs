using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    [Table("Applicants")]
    public class Applicant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicantId { get; set; }

        public virtual Job_Adv Job_Adv { get; set; }
        public virtual Test_Master Test_Master { get; set; }
    }
}
