using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class SituacaoProdutoViewModel
    {

        public SituacaoProdutoViewModel()
        {
            //ValidationResult = new DomainValidation.Validation.ValidationResult();
        }
        public int IdSituacaoProduto { get; set; }

        [Display(Name = "Situação")]
        [StringLength(100, ErrorMessage = "O campo situação do produto deve conter no máximo 100 caracteres.")]
        public string DescSituacaoProduto { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

    }
}
