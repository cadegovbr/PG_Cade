using DomainValidation.Validation;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application
{
    public class ApplicationService
    {
        private readonly IUnitOfWork _uow;

        public ApplicationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void BeginTransaction()
        {
            _uow.BeginTransaction();
        }


        public List<ValidationError> Commit()
        {
            var results = _uow.Commit();
            return results;
        }

        public DbContextTransaction BeginDbTransaction()
        {
            return _uow.BeginDbTransaction();
        }
        
        public void Rollback(DbContextTransaction transaction)
        {
            _uow.Rollback(transaction);
        }

        public int Commit(DbContextTransaction transaction)
        {
            return _uow.Commit(transaction);
        }
    }
}
