using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class JustificativaViewModel
    {
        public JustificativaViewModel()
        {
            //ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        public int IdJustificativa { get; set; }

        [Display(Name = "Descrição da Justificativa")]
        [StringLength(100,ErrorMessage ="O Campo Tipo de Faixa deve conter no máximo 100 caracteres.")]
        public string DescJustificativa { get; set; }

        
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
