using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        public Notification()
        {
            this.not_time = DateTime.Now;
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        [Required,StringLength(150)]
        public string notification { get; set; }

        public string link { get; set; }

        public Nullable<DateTime> not_time { get; set; }
        public virtual User User { get; set; }
        public bool IsRead { get; set; }

       
    }
}
