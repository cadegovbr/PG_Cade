using PGD.Domain.Entities;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class NotaAvaliacaoRepository : Repository<NotaAvaliacao>, INotaAvaliacaoRepository
    {

        public NotaAvaliacaoRepository(PGDDbContext context)
            : base(context)
        {

        }

        public IEnumerable<NotaAvaliacao> ObterTodosPorNivelAvaliacao(int idNivelAvaliacao)
        {
            if (idNivelAvaliacao == (int)eNivelAvaliacao.Detalhada)
            {
                return DbSet.AsNoTracking().Where(a => a.IndAtivoAvaliacaoDetalhada);
            }
            else if (idNivelAvaliacao == (int)eNivelAvaliacao.Simplificada)
            {
                return DbSet.AsNoTracking().Where(a => a.IndAtivoAvaliacaoSimplificada);
            }
            else
            {
                return DbSet.AsNoTracking(); 
            }
        }
    }
}
