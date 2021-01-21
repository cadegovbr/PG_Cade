using PGD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IPactoRepository:IRepository<Pacto>
    {
        Pacto BuscarPorId(int id);
        Pacto Atualizar(Pacto obj, int idPacto);
        void AtualizaEstadoEntidadesRelacionadas(Pacto pacto);
        IEnumerable<Pacto> ConsultarPactos(Pacto objFiltro, bool incluirUnidadesSubordinadas = false);
    }
}
