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
    [Table("Models")]
    public class Model
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ModelId { get; set; }
        [DisplayName("Model")]
        public string ModelName { get; set; }
        public virtual List<Browser> Browsers { get; set; }
        public virtual List<OS> OS { get; set; }
        public Brand Brand { get; set; }
        public virtual Type  Type { get; set; }

        public Nullable<int> TypeId { get; set; }
        public Nullable<int> BrowserId { get; set; }
        public Nullable<int> OSId { get; set; }

        public Model()
        {
            Browsers = new List<Browser>();
            OS = new List<OS>();
        }
    }
}
