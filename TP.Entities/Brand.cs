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
    [Table("Brands")]
    public class Brand
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [DisplayName("Marka")]
        public string BrandName { get; set; }
        
        public virtual List<Model> Models { get; set; }


        public Brand()
        {
            Models = new List<Model>();
        }
    }
}
