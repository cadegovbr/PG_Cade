using PGD.Domain.Filtros.Base;

namespace PGD.Domain.Filtros
{
    public class UnidadeTipoPactoFiltro : BaseFiltro
    {
        public string NomeUnidade { get; set; }
        public string DescTipoPacto { get; set; }
        public int? IdTipoPacto { get; set; }
        public int? IdUnidade { get; set; }
        public bool IncludeUnidade { get; set; }
        public bool IncludeTipoPacto { get; set; }

    }
}