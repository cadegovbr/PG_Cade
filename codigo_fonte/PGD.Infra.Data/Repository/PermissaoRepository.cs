using System.Data.Entity;
using PGD.Domain.Entities.RH;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Paginacao;
using PGD.Infra.Data.Context;
using PGD.Infra.Data.Util;
using System.Linq;
using PGD.Domain.Entities;
using QueryableExtensions = PGD.Infra.Data.Util.QueryableExtensions;

namespace PGD.Infra.Data.Repository
{
    public class PermissaoRepository : Repository<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(PGDDbContext context)
            : base(context)
        {

        }

        public Paginacao<Permissao> Buscar(PermissaoFiltro filtro)
        {
            var retorno = new Paginacao<Permissao>();
            var query = DbSet.AsQueryable();

            query.Include("PermissoesPerfil");

            if (filtro.IdPerfil.HasValue && filtro.IdPerfil > 0)
                query = query.Where(x => x.PermissoesPerfil.Any(y => y.IdPerfil == filtro.IdPerfil));
            
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
