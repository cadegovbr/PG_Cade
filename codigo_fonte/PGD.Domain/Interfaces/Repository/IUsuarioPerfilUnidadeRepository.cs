using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IUsuarioPerfilUnidadeRepository : IRepository<UsuarioPerfilUnidade>
    {
        Paginacao<UsuarioPerfilUnidade> Buscar(UsuarioPerfilUnidadeFiltro filtro);
        UsuarioPerfilUnidade AdicionarSave(UsuarioPerfilUnidade obj);
    }
}
