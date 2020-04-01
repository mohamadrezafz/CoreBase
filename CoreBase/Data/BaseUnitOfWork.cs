using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;

namespace CoreBase.Data
{

    public interface IBaseUnitOfWork<TContext>
     where TContext : DbContext
    {
        TContext Context { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
        void Dispose();
    }
    public abstract class BaseUnitOfWork<TContext> : IBaseUnitOfWork<TContext>, IDisposable
    where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private DbContextTransaction _objTran;

        public BaseUnitOfWork()
        {
            _context = new TContext();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public TContext Context
        {
            get { return _context; }
        }
        public void CreateTransaction()
        {
            _objTran = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _objTran.Commit();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
    }
}
