using PGD.Application.ViewModels.Filtros.Base;

namespace PGD.Application.ViewModels.Filtros
{
    public class UsuarioPerfilUnidadeFiltroViewModel : BaseFiltroViewModel
    {
        public UsuarioPerfilUnidadeFiltroViewModel()
        {
            OrdenarPor = "Id";
        }

        public int? IdUsuario { get; set; }
        public bool IncludeUnidade { get; set; } = true;
        public bool IncludePerfil { get; set; } = true;
        public bool IncludeUsuario { get; set; } = true;
        public bool SomenteNaoExcluidos { get; set; } = true;
    }
}
