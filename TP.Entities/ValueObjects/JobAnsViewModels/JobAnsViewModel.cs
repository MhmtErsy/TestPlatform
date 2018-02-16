using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Entities.ValueObjects.JobAnsViewModels
{
    public class JobAnsViewModel
    {

        [DisplayName("Cevap Başlık"), Required, StringLength(50)]
        public string ans_title { get; set; }

        public int testjob_id { get; set; }

        [DisplayName("Cevap Açıklama"), Required, StringLength(4000)]
        public string ans_desc { get; set; }


        [DisplayName("Test Master'a göndermek için işaretleyin. Şimdilik kaydetmek istiyorsanız boş bırakın.")]
        public bool isSubmitted { get; set; }

        public string State { get; set; }
    }
}
