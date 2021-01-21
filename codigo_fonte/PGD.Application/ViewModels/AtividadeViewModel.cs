using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class AtividadeViewModel
    {
        public AtividadeViewModel()
        {
            Tipos = new List<TipoAtividadeViewModel>();
        }

        public AtividadeViewModel(UsuarioViewModel usuario)
        {
            Tipos = new List<TipoAtividadeViewModel>();
            Usuario = usuario;
        }

        public int IdAtividade { get; set; }

        [Required(ErrorMessage = "O campo Atividade é de preenchimento obrigatório!")]
        [StringLength(1000)]
        [Display(Name = "Nome da Atividade")]
        public string NomAtividade { get; set; }

        [Required(ErrorMessage = "O campo Percentual Mínimo de Redução é de preenchimento obrigatório!")]
        [Display(Name = "Percentual de Produtividade Adicional")]
        [Range(0, 100, ErrorMessage = "Percentual Mínimo de Redução deve ser um número de 0 a 100!")]
        //[DataAnnotationsExtensions.Integer(ErrorMessage = "Favor informar um número válido.")]
        //[DataType(DataType.Currency, ErrorMessage = "Percentual Mínimo de Redução deve ser um número de 0 à 100!")]
        public int PctMinimoReducao { get; set; }

        public List<TipoAtividadeViewModel> Tipos { get; set; }

        //public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

        public UsuarioViewModel Usuario { get; set; }

        [Display(Name = "Link para explicações sobre a atividade")]
        public string Link { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
