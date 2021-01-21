using PGD.Domain.Filtros.Base;

namespace PGD.Domain.Filtros
{
    public class PermissaoFiltro : BaseFiltro
    {
        public PermissaoFiltro()
        {
            OrdenarPor = "Controller";
        }

        public int? IdPermissao { get; set; }
        public int? IdPerfil { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Descricao { get; set; }

    }
}