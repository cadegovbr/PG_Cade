using PGD.Domain.Enums;
using PGD.Domain.Filtros.Base;

namespace PGD.Domain.Filtros
{
    public class UsuarioFiltro : BaseFiltro
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Sigla { get; set; }
        public string Matricula { get; set; }
        public int? IdUnidade { get; set; }
        public Perfil? Perfil { get; set; }
        public bool IncludeUnidadesPerfis { get; set; }
    }
}