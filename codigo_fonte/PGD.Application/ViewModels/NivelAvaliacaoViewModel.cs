using PGD.Application.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application.ViewModels
{
    public class NivelAvaliacaoViewModel
    {
        public NivelAvaliacaoViewModel()
        {
            
        }
        public int IdNivelAvaliacao { get; set; }

        [Display(Name = "Nível da avaliação")]
        [StringLength(100,ErrorMessage ="O campo nível da avaliação deve conter no máximo 100 caracteres.")]
        public string DescNivelAvaliacao { get; set; }

        
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
