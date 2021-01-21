using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class OS_CriterioAvaliacaoViewModel
    {

        public int IdCriterioAvaliacao { get; set; }
        public int IdCriterioAvaliacaoOriginal { get; set; }
        public string DescCriterioAvaliacao { get; set; }
        public string StrTextoExplicativo { get; set; }
        public virtual List<ItemAvaliacaoViewModel> ItensAvaliacao { get; set; }
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        
    }
}
