using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities
{
    [Table("Testers")]
    public class Tester:User
    {
        [DisplayName("Skor")]
        public int score { get; set; }
        

        public virtual Rank rank { get; set; }
        
        public Nullable<int> DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        [DisplayName("Hazırlık Durumu")]
        public bool isReady { get; set; }

        [DisplayName("Banka Hesabı")]
        public string bank_account { get; set; }
        
    }
}
