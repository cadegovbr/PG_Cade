using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IOS_CriterioAvaliacaoService : IService<OS_CriterioAvaliacao>
    {
        IEnumerable<OS_CriterioAvaliacao> ObterTodosAtivos();
        IEnumerable<OS_CriterioAvaliacao> BuscarPorIdOS(int idOS);
    }
}
