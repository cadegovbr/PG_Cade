using PGD.Domain.Entities.Usuario;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ObterPorNome(string nome);
        Usuario ObterPorCPF(string cpf);
        Usuario ObterPorEmail(string email);
        Paginacao<Usuario> Buscar(UsuarioFiltro filtro);
    }
}
