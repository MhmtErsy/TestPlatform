using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    [Table("Customers")]
    public class Customer : User
    {
        public virtual List<Job_Adv> JobAnnouncement { get; set; }
    }
}
