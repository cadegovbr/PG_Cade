using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface ICriterioAvaliacaoService : IService<CriterioAvaliacao>
    {
        IEnumerable<CriterioAvaliacao> ObterTodosAtivos();
    }
}
