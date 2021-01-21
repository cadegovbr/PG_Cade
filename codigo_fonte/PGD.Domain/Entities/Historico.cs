using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Entities
{
    public class Historico
    {
        public int IdHistorico { get; set; }
        public int IdPacto { get; set; }
        [MaxLength]
        [Column(TypeName = "varchar(MAX)")]
        public string Descricao { get; set; }
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return true;
        }

    }
}
