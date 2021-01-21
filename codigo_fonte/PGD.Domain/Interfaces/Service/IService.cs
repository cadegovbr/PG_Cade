using System;
using System.Collections.Generic;

namespace PGD.Domain.Interfaces.Service
{
    public interface IService<TEntity> where TEntity : class
    {
        TEntity Adicionar(TEntity obj);
        TEntity ObterPorId(int id);
        IEnumerable<TEntity> ObterTodos();        
        TEntity Atualizar(TEntity obj);
        TEntity Remover(TEntity obj);
    }
}
