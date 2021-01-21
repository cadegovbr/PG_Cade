using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class VincularPerfilUsuarioViewModel
    {
        public VincularPerfilUsuarioViewModel()
        {
            UsuarioPerfilUnidade = new List<UsuarioPerfilUnidadeViewModel>();
            ValidationResult = new DomainValidation.Validation.ValidationResult();
        }

        public int IdUsuario { get; set; }

        [Display(Name = "Nome do Servidor")]
        public string Nome { get; set; }

        [Display(Name = "Matrícula")]
        public string Matricula { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo Perfil é de preenchimento obrigatório!")]
        [Display(Name = "Perfil")]
        public int? IdPerfil { get; set; }

        [Required(ErrorMessage = "O campo Unidade é de preenchimento obrigatório!")]
        [Display(Name = "Unidade")]
        public int? IdUnidade { get; set; }

        public IEnumerable<UsuarioPerfilUnidadeViewModel> UsuarioPerfilUnidade { get; set; }

        [ScaffoldColumn(false)]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }
    }
}
