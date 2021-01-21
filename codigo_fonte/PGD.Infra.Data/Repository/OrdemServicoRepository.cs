using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class OrdemServicoRepository : Repository<OrdemServico>, IOrdemServicoRepository
    {

        public OrdemServicoRepository(PGDDbContext context)
            : base(context)
        {
           
        }

        public void DeletarGrupos(OrdemServico ordemservico)
        {
            Db.OS_GrupoAtividade.RemoveRange(ordemservico.Grupos);
        }

        public  OrdemServico ObterPorIdInclude(int id)
        {
            
            return DbSet.AsNoTracking().Include("Grupos")
                .Include("Grupos.Atividades")
                .Include("Grupos.Atividades.Tipos").Where(a => a.IdOrdemServico == id).FirstOrDefault();
        }

        public OrdemServico ObterTodosInclude()
        {

            return DbSet.AsNoTracking().Include("Grupos")
                .Include("Grupos.Atividades")
                .Include("Grupos.Atividades.Tipos")
                .Where(x => x.DatInicioSistema <= System.DateTime.Now).OrderByDescending(a => a.DatInicioSistema).FirstOrDefault();
        }
    }
}
