using PGD.Application.ViewModels.Filtros.Base;
using PGD.Domain.Enums;

namespace PGD.Application.ViewModels.Filtros
{
    public class UsuarioFiltroViewModel : BaseFiltroViewModel
    {
        public UsuarioFiltroViewModel()
        {
            OrdenarPor = "Nome";
        }

        public string Nome { get; set; }
        public string Matricula { get; set; }
        public string Cpf { get; set; }

        public string Sigla { get; set; }
        public int? IdUnidade { get; set; }
        public Perfil? Perfil { get; set; }
        public bool IncludeUnidadesPerfis { get; set; }

    }
}