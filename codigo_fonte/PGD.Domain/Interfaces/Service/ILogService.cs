using PGD.Domain.Entities;

namespace PGD.Domain.Interfaces.Service
{
    public interface ILogService : IService<Log>
    {
        Log Logar(object obj, string CpfUsuario, string oper);

    }
}
