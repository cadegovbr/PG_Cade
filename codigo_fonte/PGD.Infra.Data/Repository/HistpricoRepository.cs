using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class HistoricoRepository:Repository<Historico>,IHistoricoRepository
    {
        public HistoricoRepository(PGDDbContext context)
            : base(context)
        {

        }

        public Historico BuscarPorId(int idpacto, int idhistorico)
        {
            return DbSet.AsNoTracking().Where(a => a.IdPacto == idpacto && a.IdHistorico == idhistorico).FirstOrDefault();
        }
    }
}
