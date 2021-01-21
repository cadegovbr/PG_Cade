using PGD.Domain.Entities.RH;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;

namespace PGD.Infra.Data.Repository
{
    public class FeriadoRepository : Repository<Feriado>, IFeriadoRepository
    {
        public FeriadoRepository(PGDDbContext context)
            : base(context)
        {

        }

    }
}
