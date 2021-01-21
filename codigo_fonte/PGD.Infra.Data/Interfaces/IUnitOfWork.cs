using DomainValidation.Validation;
using System.Collections.Generic;
using System.Data.Entity;

namespace PGD.Infra.Data.Interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        List<ValidationError> Commit();
        void AtivarLogs(bool logAtivo);
        DbContextTransaction BeginDbTransaction();
        void Rollback(DbContextTransaction transaction);
        int Commit(DbContextTransaction transaction);
    }
}
