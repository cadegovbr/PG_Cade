using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class SelecionarUnidadeViewModel
    {
        [Required(ErrorMessage = "O campo Unidade é de preenchimento obrigatório!")]
        [Display(Name = "Unidade")]
        public int? IdUnidade { get; set; }
    }
}
