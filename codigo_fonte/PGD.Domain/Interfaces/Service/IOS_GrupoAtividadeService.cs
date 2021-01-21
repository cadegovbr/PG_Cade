using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Service
{
    public interface IOS_GrupoAtividadeService : IService<OS_GrupoAtividade>
    {
        IEnumerable<OS_GrupoAtividade> ObterTodosAtivos();
    }
}
