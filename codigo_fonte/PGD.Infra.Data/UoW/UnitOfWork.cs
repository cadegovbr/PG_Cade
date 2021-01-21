using CGU.Util;
using DomainValidation.Validation;
using PGD.Infra.Data.Context;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace PGD.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PGDDbContext _context;
        private bool _disposed;

        public UnitOfWork(PGDDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _disposed = false;
        }

        public DbContextTransaction BeginDbTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Rollback(DbContextTransaction transaction)
        {
            transaction.Rollback();
        }

        public int Commit(DbContextTransaction transaction)
        {
            var retorno = _context.SaveChanges();
            transaction.Commit();
            return retorno;
        }

        public List<ValidationError> Commit()
        {
            List<ValidationError> validacoes = new List<ValidationError>();

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                      var mensagem = "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage;
                      // LogManagerComum.LogarDebug(null, null, mensagem);
                      validacoes.Add(new ValidationError(validationError.ErrorMessage));
                    }
                }
            }

            return validacoes;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AtivarLogs(bool logAtivo) => _context.AtivarLog(logAtivo);
    }
}
