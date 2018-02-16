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
    public class Type
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [DisplayName("Tip")]
        public string TypeName { get; set; }
        
        public virtual List<Model> Models { get; set; }

        public Type()
        {
            Models = new List<Model>();
        }
    }
}
