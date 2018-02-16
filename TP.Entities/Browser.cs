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
    public class Browser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrowserId { get; set; }

        [DisplayName("Tarayıcı")]
        public string BrowserName { get; set; }

        public Model Model { get; set; }


    }
}
