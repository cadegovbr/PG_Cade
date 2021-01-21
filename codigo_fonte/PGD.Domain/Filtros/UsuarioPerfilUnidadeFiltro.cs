using PGD.Domain.Filtros.Base;

namespace PGD.Domain.Filtros
{
    public class UsuarioPerfilUnidadeFiltro : BaseFiltro
    {
        public long? IdUsuarioPerfilUnidade { get; set; }
        public int? IdUsuario { get; set; }
        public bool IncludeUnidade { get; set; }
        public bool IncludePerfil { get; set; }
        public bool IncludeUsuario { get; set; }
        public bool SomenteNaoExcluidos { get; set; } = true;
    }
}
