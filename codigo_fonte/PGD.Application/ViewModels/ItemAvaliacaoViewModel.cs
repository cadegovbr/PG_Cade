using PGD.Application.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class ItemAvaliacaoViewModel
    {
        public ItemAvaliacaoViewModel()
        {   
        }

        public ItemAvaliacaoViewModel(UsuarioViewModel usuario)
        {         
            Usuario = usuario;
        }

        public int IdItemAvaliacao { get; set; }

        [Required(ErrorMessage = "O campo Descrição do Item é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Descrição do Item")]
        public string DescItemAvaliacao { get; set; }

        [Required(ErrorMessage = "O campo Impacto na Nota é de preenchimento obrigatório!")]        
        [Range(-10, 0, ErrorMessage = "Impacto na Nota deve ser um número de -10 a 0!")]                
        public decimal ImpactoNota { get; set; }
        public int  IdNotaMaxima { get; set; }
        public virtual NotaAvaliacaoViewModel NotaMaxima { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public bool Inativo { get; set; }
        public bool Excluir { get; set; }

        public string IdCriterioAvaliacaoIdItemAvaliacao { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
