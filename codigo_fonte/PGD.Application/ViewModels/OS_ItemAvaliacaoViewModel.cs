using PGD.Application.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class OS_ItemAvaliacaoViewModel
    {
        public OS_ItemAvaliacaoViewModel()
        {   
        }


        public int IdItemAvaliacao { get; set; }
        public string DescItemAvaliacao { get; set; }

        public decimal ImpactoNota { get; set; }
        public int  IdNotaMaxima { get; set; }
        public virtual NotaAvaliacaoViewModel NotaMaxima { get; set; }
        
        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
