using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class SituacaoPactoViewModel
    {
        public SituacaoPactoViewModel()
        {
            //ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        public int IdSituacaoPacto { get; set; }

        [Display(Name = "Situação")]
        [StringLength(100,ErrorMessage = "O campo situação do plano de trabalho deve conter no máximo 100 caracteres.")]
        public string DescSituacaoPacto { get; set; }

        
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
