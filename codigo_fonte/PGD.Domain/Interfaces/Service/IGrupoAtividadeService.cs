using PGD.Domain.Entities;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IGrupoAtividadeService : IService<GrupoAtividade>
    {
        IEnumerable<GrupoAtividade> ObterTodosAtivos();
    }
}
