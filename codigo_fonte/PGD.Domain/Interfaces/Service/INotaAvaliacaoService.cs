using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface INotaAvaliacaoService : IService<NotaAvaliacao>
    {
        IEnumerable<NotaAvaliacao> ObterTodosPorNivelAvaliacao(int idNivelAvaliacao);
    }
}
