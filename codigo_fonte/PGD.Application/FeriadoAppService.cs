using PGD.Application.Interfaces;
using PGD.Domain.Entities.RH;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PGD.Application
{
    public class FeriadoAppService : ApplicationService, IFeriadoAppService
    {
        private readonly IFeriadoService _feriadoService;

        public FeriadoAppService(IFeriadoService feriadoService, IUnitOfWork uow)
            : base(uow)
        {
            this._feriadoService = feriadoService;
        }

        public Feriado Adicionar(Feriado obj)
        {
            throw new NotImplementedException();
        }

        public Feriado Atualizar(Feriado obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Feriado> Buscar(Expression<Func<Feriado, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Feriado ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Feriado> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Feriado> ObterTodos(string strInclude)
        {
            throw new NotImplementedException();
        }

        public void Remover(int id)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
