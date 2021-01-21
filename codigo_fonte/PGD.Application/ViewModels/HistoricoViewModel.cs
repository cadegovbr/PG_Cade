using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class HistoricoViewModel
    {
        public int IdHistorico { get; set; }
        public int IdPacto { get; set; }
        public string Descricao { get; set; }
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
