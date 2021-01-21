using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class CriterioAvaliacaoViewModel
    {

        public int IdCriterioAvaliacao { get; set; }
        public int IdCriterioAvaliacaoOriginal { get; set; }

        [Required(ErrorMessage = "O campo Descrição é de preenchimento obrigatório!")]
        [StringLength(100)]
        [Display(Name = "Descrição do Critério")]
        public string DescCriterioAvaliacao { get; set; }

        [Required(ErrorMessage = "O campo Texto Explicativo é de preenchimento obrigatório!")]
        [StringLength(1000)]
        [Display(Name = "Texto Explicativo")]
        public string StrTextoExplicativo { get; set; }

        [Required(ErrorMessage = "O campo Atividades é de preenchimento obrigatório!")]
        public virtual List<ItemAvaliacaoViewModel> ItensAvaliacao { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public UsuarioViewModel Usuario { get; set; }
    }
}
