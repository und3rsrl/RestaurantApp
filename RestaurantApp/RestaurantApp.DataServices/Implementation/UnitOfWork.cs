using Microsoft.EntityFrameworkCore.Storage;
using RestaurantApp.DataServices.Interfaces;
using System;
using System.Threading.Tasks;

namespace RestaurantApp.DataServices.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Entities _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(Entities dbContext)
        {
            _dbContext = dbContext;
        }

        public int CommitChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void CreateTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _dbContext.Database.BeginTransaction();
            }
            else
            {
                throw new Exception("Transaction already exists! Use Rollback or Commit to close it.");
            }
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
