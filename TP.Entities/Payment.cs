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
    [Table("Payments")]
    public class Payment
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        public User pay_user { get; set; }

        [DisplayName("Miktar")]
        public decimal payment { get; set; }

        [DisplayName("Ödeme Tarihi")]
        public DateTime payDate { get; set; }

        public int JobAdvId { get; set; }
        [ForeignKey("JobAdvId")]
        public virtual Job_Adv Job_Adv { get; set; }

    }
}
