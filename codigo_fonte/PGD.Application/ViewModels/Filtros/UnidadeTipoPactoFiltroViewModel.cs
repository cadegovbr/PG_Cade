using PGD.Application.ViewModels.Filtros.Base;

namespace PGD.Application.ViewModels.Filtros
{
    public class UnidadeTipoPactoFiltroViewModel : BaseFiltroViewModel
    {
        public UnidadeTipoPactoFiltroViewModel()
        {
            OrdenarPor = "IdUnidade_TipoPacto";
        }

        public string NomeUnidade { get; set; }
        public string DescTipoPacto { get; set; }
        public int? IdTipoPacto { get; set; }
        public int? IdUnidade { get; set; }
        public bool IncludeUnidade { get; set; }
        public bool IncludeTipoPacto { get; set; }

    }
}