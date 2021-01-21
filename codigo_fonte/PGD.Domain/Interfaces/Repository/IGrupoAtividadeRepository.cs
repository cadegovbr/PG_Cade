using PGD.Domain.Entities;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IGrupoAtividadeRepository : IRepository<GrupoAtividade>
    {
        GrupoAtividade AdicionarSave(GrupoAtividade obj);
    }
}
