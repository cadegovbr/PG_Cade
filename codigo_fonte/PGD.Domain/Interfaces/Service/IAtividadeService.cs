using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IAtividadeService : IService<Atividade>
    {
        IEnumerable<Atividade> ObterTodosAtivos();
    }
}
