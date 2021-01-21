using PGD.Domain.Entities.RH;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IUnidadeRepository : IRepository<Unidade>
    {
        Paginacao<Unidade> Buscar(UnidadeFiltro filtro);
    }
}
