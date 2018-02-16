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
    [Table("Ranks")]
    public class Rank
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RankId { get; set; }

        [Required]
        public int requiredMinScore { get; set; }

        [DisplayName("Rütbe"),Required,StringLength(80)]
        public string rank { get; set; }
    }
}
