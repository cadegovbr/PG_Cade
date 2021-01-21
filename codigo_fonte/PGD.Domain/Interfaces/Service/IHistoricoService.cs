using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Service
{
    public interface IHistoricoService:IService<Historico>
    {
        IEnumerable<Historico> ObterTodos(Historico objFiltro);
        IEnumerable<Historico> ObterTodos(int idpacto);
        Historico BuscarPorId(int idpacto, int idhistorico);
        Historico AtualizarIdPacto(Historico obj, int idPacto);

    }
}
