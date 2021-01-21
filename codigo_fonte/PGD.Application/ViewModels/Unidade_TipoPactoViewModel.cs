using System.ComponentModel.DataAnnotations;

namespace PGD.Application.ViewModels
{
    public class Unidade_TipoPactoViewModel
    {

        public int IdUnidade_TipoPacto { get; set; }
        [Required(ErrorMessage ="A unidade deve ser selecionada")]
        public int IdUnidade { get; set; }
        [Required(ErrorMessage = "O tipo de plano deve ser selecionado")]
        public int IdTipoPacto { get; set; }
        public bool IndPermitePactoExterior { get; set; }
        public string DescTipoPacto { get; set; }
        public string NomeUnidade { get; set; }

    }
}
