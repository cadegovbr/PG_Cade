using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class IniciativaPlanoOperacional 
    {
        [Key]
        [StringLength(5)]
        public string IdIniciativaPlanoOperacional { get; set; }
        [Column(TypeName = "varchar(max)")]
        [StringLength(1000)]
        public string DescIniciativaPlanoOperacional { get; set; }

    }
}
