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
    public class OS
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OSId { get; set; }

        [DisplayName("İşletim Sistemi")]
        public string OSName { get; set; }

        public Model Model { get; set; }
    }
}
