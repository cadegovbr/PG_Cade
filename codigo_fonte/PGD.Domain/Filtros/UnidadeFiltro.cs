using System.Collections.Generic;
using PGD.Domain.Filtros.Base;

namespace PGD.Domain.Filtros
{
    public class UnidadeFiltro : BaseFiltro
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public string NomeOuSigla { get; set; }
        public List<int> IdsPactos { get; set; }
        public int? IdUnidadeSuperior { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdTipoPacto { get; set; }
        public bool BuscarExcluidos { get; set; }
        public bool IncludeUnidadeSuperior { get; set; }
        public bool IncludeUnidadesFilhas { get; set; }
        public bool IncludeUnidadePerfisUnidades { get; set; }
        public bool IncludeUnidadeTiposPactos { get; set; }

    }
}