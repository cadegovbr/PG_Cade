using PGD.Domain.Entities;

namespace PGD.Domain.Interfaces.Repository
{
    public interface ICriterioAvaliacaoRepository : IRepository<CriterioAvaliacao>
    {
        CriterioAvaliacao AdicionarSave(CriterioAvaliacao obj);
    }
}
