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
    [Table("Devices")]
    public class Device
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceId { get; set; }

        
        public virtual List<Tester> testers { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Model Model { get; set; }
        public virtual OS OS { get; set; }
        public virtual Browser Browser { get; set; }

        [DisplayName("Cihaz")]
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> ModelId { get; set; }
        public Nullable<int> OSId { get; set; }
        public Nullable<int> BrowserId { get; set; }

        public Device()
        {
            testers = new List<Tester>();
        }
        //[DisplayName("Cihaz Markası"),Required, StringLength(50)]
        //public string device_brand { get; set; }

        //[DisplayName("Cihaz Modeli"), Required, StringLength(50)]
        //public string device_model { get; set; }

        //[DisplayName("Cihaz Tipi"), Required, StringLength(25)]
        //public string device_type { get; set; }

        //[DisplayName("Tarayıcı"), StringLength(50)]
        //public string browser { get; set; }

        //[DisplayName("İşletim Sistemi"), StringLength(50)]
        //public string OS { get; set; }
    }
}
