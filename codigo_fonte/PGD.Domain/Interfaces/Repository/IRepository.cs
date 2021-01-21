using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PGD.Domain.Filtros.Base;

namespace PGD.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Adicionar(TEntity obj);
        TEntity ObterPorId(int id);
        IEnumerable<TEntity> ObterTodos();
        IEnumerable<TEntity> ObterTodos(string strInclude);
        TEntity Atualizar(TEntity obj);
        void Remover(int id);
        IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);
        int SaveChanges();
    }
}
