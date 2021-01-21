using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class LoginViewModel
    {
       
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo Senha é de preenchimento obrigatório!")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo Usuário é de preenchimento obrigatório!")]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }
    }
}
