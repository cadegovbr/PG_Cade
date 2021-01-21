using System.Data.Entity;
using PGD.Domain.Entities;
using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PGD.Domain.Entities.Usuario;
using PGD.Domain.Filtros;
using PGD.Domain.Paginacao;
using PGD.Infra.Data.Util;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class Unidade_TipoPactoRepository : Repository<Unidade_TipoPacto>, IUnidade_TipoPactoRepository
    {

        public Unidade_TipoPactoRepository(PGDDbContext context)
            : base(context)
        { }

        public Paginacao<Unidade_TipoPacto> Buscar(UnidadeTipoPactoFiltro filtro)
        {
            var retorno = new Paginacao<Unidade_TipoPacto>();
            var query = DbSet.AsQueryable();

            if (filtro.IncludeTipoPacto)
                query.Include("TipoPacto");

            if (filtro.IncludeUnidade)
                query.Include("Unidade");

            query = !string.IsNullOrWhiteSpace(filtro.DescTipoPacto) ? query.Where(x => x.TipoPacto.DescTipoPacto.ToLower().Contains(filtro.DescTipoPacto.ToLower())) : query;
            query = !string.IsNullOrWhiteSpace(filtro.NomeUnidade) ? query.Where(x => x.Unidade.Nome.ToLower().Contains(filtro.NomeUnidade.ToLower())) : query;
            query = filtro.IdUnidade.HasValue && filtro.IdUnidade != 0 ? query.Where(x => x.IdUnidade == filtro.IdUnidade) : query;
            query = filtro.IdTipoPacto.HasValue && filtro.IdTipoPacto != 0 ? query.Where(x => x.IdTipoPacto == filtro.IdTipoPacto) : query;
            query = filtro.Id.HasValue && filtro.Id != 0 ? query.Where(x => x.IdUnidade_TipoPacto == filtro.Id) : query;

            retorno.QtRegistros = query.Count();

            if (filtro.Skip.HasValue && filtro.Take.HasValue)
            {
                retorno.Lista = filtro.OrdenarDescendente
                    ? query.OrderByDescending(filtro.OrdenarPor).Skip(filtro.Skip.Value).Take(filtro.Take.Value).ToList()
                    : query.OrderBy(filtro.OrdenarPor).Skip(filtro.Skip.Value).Take(filtro.Take.Value).ToList();
            }
            else
            {
                retorno.Lista = query.ToList();
            }


            return retorno;
        }

    }
}
