using PGD.Domain.Entities.Usuario;
using PGD.Domain.Filtros;
using PGD.Domain.Interfaces.Repository;
using PGD.Domain.Paginacao;
using PGD.Infra.Data.Context;
using PGD.Infra.Data.Util;
using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PGD.Infra.Data.Repository
{
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
    Justification = "False positive.")]
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {

        public UsuarioRepository(PGDDbContext context)
            : base(context)
        {

        }

        public Usuario ObterPorCPF(string cpf)
        {
            cpf = cpf.PadLeft(11, '0');
            var usuario = DbSet.AsNoTracking()
                .Where(a => a.Cpf.Replace("\r", string.Empty).Replace("\n", string.Empty) == cpf)
                .Include("UsuariosPerfisUnidades")
                .Include("UsuariosPerfisUnidades.Perfil")
                .Include("UsuariosPerfisUnidades.Unidade")
                .FirstOrDefault();
            if (usuario == null)
            {
                long novocpf;
                if (long.TryParse(cpf, out novocpf))
                {
                    cpf = novocpf.ToString();
                    usuario = DbSet.AsNoTracking().Where(a => a.Cpf.Replace("\r", string.Empty).Replace("\n", string.Empty) == cpf)
                        .Include("UsuariosPerfisUnidades")
                        .Include("UsuariosPerfisUnidades.Perfil")
                        .Include("UsuariosPerfisUnidades.Unidade")
                        .FirstOrDefault();
                }

            }
            return usuario;
        }

        public Usuario ObterPorEmail(string email)
        {
            return DbSet.AsNoTracking().Where(a => a.Email.Replace("\r", string.Empty).Replace("\n", string.Empty) == email).FirstOrDefault();
        }

        public Usuario ObterPorNome(string nome)
        {
           
            return DbSet.AsNoTracking().Where(a => a.Nome.Trim().ToLower().Contains(nome.Trim().ToLower())).FirstOrDefault();
        }

        public override Usuario ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario Teste(string email)
        {
            var usuario = DbSet.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(email))
                usuario = usuario.Where(x => x.Email.ToUpper() == email.ToUpper());

            return new Usuario();
        }

        public Paginacao<Usuario> Buscar(UsuarioFiltro filtro)
        {
            var retorno = new Paginacao<Usuario>();
            var query = DbSet.AsQueryable();

            if (filtro.IncludeUnidadesPerfis)
                query.Include("UsuariosPerfisUnidades")
                    .Include("UsuariosPerfisUnidades.Perfil")
                    .Include("UsuariosPerfisUnidades.Unidade");

            query = !string.IsNullOrWhiteSpace(filtro.Nome) ? query.Where(x => x.Nome.ToLower().Contains(filtro.Nome.ToLower())) : query;
            query = !string.IsNullOrWhiteSpace(filtro.Cpf) ? query.Where(x => x.Cpf == filtro.Cpf) : query;
            query = !string.IsNullOrWhiteSpace(filtro.Sigla) ? query.Where(x => x.Sigla == filtro.Sigla) : query;
            query = !string.IsNullOrWhiteSpace(filtro.Matricula) ? query.Where(x => x.Matricula.ToLower().Contains(filtro.Matricula.ToLower())) : query;
            query = filtro.IdUnidade.HasValue ? query.Where(x => x.UsuariosPerfisUnidades.Any(y => y.IdUnidade == filtro.IdUnidade)) : query;
            query = filtro.Id.HasValue ? query.Where(x => x.IdUsuario == filtro.Id) : query;

            query = filtro.Perfil.HasValue ? query.Where(x => x.UsuariosPerfisUnidades.Any(y => y.IdPerfil == (int)filtro.Perfil)) : query;

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
