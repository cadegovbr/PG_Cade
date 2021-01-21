using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IHistoricoRepository : IRepository<Historico>
    {
        Historico BuscarPorId(int idpacto, int idhistorico);
        Historico Atualizar(Historico historico, int idPacto);
    }
}
