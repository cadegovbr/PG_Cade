using PGD.Domain.Entities;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IUnidade_TipoPactoRepository : IRepository<Unidade_TipoPacto>
    {
        Unidade_TipoPacto AdicionarSave(Unidade_TipoPacto obj);
        Paginacao<Unidade_TipoPacto> Buscar(UnidadeTipoPactoFiltro filtro);
    }
}
