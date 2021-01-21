using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IPermissaoRepository : IRepository<Permissao>
    {
        Paginacao<Permissao> Buscar(PermissaoFiltro filtro);
    }
}
