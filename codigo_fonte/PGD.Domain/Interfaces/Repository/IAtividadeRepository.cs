using PGD.Domain.Entities;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IAtividadeRepository : IRepository<Atividade>
    {
        Atividade AdicionarSave(Atividade obj);
    }
}
