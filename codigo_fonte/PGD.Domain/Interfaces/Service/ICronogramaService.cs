using PGD.Domain.Entities;
using PGD.Domain.Entities.Usuario;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface ICronogramaService : IService<Cronograma>
    {
        IEnumerable<Cronograma> ObterTodos(int idPacto);

        void ValidarCronograma(CronogramaPacto cronogramaPacto, List<Pacto> pactosConcorrentes, bool validarHorasADistribuir = true);
    }
}
