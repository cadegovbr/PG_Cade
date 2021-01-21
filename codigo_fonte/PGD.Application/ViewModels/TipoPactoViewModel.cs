using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class TipoPactoViewModel
    {
        public TipoPactoViewModel()
        {
            //ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        public int IdTipoPacto { get; set; }

        [Display(Name = "Tipo do Plano de Trabalho")]
        [StringLength(100,ErrorMessage = "O campo tipo do plano de trabalho deve conter no máximo 100 caracteres.")]
        public string DescTipoPacto { get; set; }

        
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
