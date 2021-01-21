using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(PGDDbContext context)
            : base(context)
        {

        }

        public Produto BuscarPorId(int idpacto, int idproduto)
        {
            return DbSet.AsNoTracking().Where(a => a.IdProduto == idproduto && a.IdPacto == idpacto).FirstOrDefault();
        }
        //public void DeletarGrupos(OrdemServico ordemservico)
        //{
        //    Db.OS_GrupoAtividade.RemoveRange(ordemservico.Grupos);
        //}

        //public override OrdemServico ObterPorId(int id)
        //{
        //    return DbSet.AsNoTracking().Where(a => a.IdOrdemServico == id).FirstOrDefault();
        //}
    }
}
