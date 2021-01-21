using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;

namespace PGD.Infra.Data.Repository
{
    public class PerfilRepository : Repository<Perfil>, IPerfilRepository
    {
        public PerfilRepository(PGDDbContext context)
           : base(context)
        {

        }

    }
}
