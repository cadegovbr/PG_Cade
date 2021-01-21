using PGD.Domain.Interfaces.Repository;
using PGD.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CGU.Util.ListsWebServices;
using PGD.Domain.Filtros.Base;

namespace PGD.Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected PGDDbContext Db;
        protected DbSet<TEntity> DbSet;
        private bool _disposed;

        protected Repository(PGDDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual TEntity Adicionar(TEntity obj)
        {
            var objreturn = DbSet.Add(obj);
            return objreturn;
        }
        public virtual TEntity AdicionarSave(TEntity obj)
        {
            var objreturn = DbSet.Add(obj);
            Db.SaveChanges();
            return objreturn;
        }
        public virtual TEntity ObterPorId(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> ObterTodos()//(int s, int t)
        {
            return DbSet.ToList(); //Take(t).Skip(s).ToList();
        }

        public virtual TEntity Atualizar(TEntity obj, int id)
        {
            try
            {
                var entity = DbSet.Find(id);
                Db.Entry(entity).CurrentValues.SetValues(obj);
                Db.SaveChanges();

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            return obj;
        }

        public virtual void Remover(int id)
        {
            DbSet.Remove(DbSet.Find(id));
        }

        public virtual IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Db.Dispose();
                }
            }
            _disposed = true;
        }
        public IEnumerable<TEntity> ObterTodos(string strInclude)
        {
            return DbSet.Include(strInclude).Include("Historicos").ToList();
        }

        public TEntity Atualizar(TEntity obj)
        {
            var entry = Db.Entry(obj);
            DbSet.Attach(obj);
            entry.State = EntityState.Modified;
            return obj;
        }
    }
}
