using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Repository
{
    public interface INotaAvaliacaoRepository : IRepository<NotaAvaliacao>
    {
        IEnumerable<NotaAvaliacao> ObterTodosPorNivelAvaliacao(int idNivelAvaliacao);
        
    }
}
