﻿using CoreBase.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace CoreBase.Logic
{
    public interface IBaseBusiness<TContext>  where TContext : DbContext , IDisposable
    {

    }
   public abstract class BaseBusiness<TContext> where TContext : DbContext, IDisposable
    {
        protected bool IsInExternalUnitOfWork { get; }
        private bool _disposed;
        private IBaseUnitOfWork<TContext> _mUnitOfWork;

        protected IBaseUnitOfWork<TContext> UnitOfWork
        {
            get { return _mUnitOfWork; }
            set { _mUnitOfWork = value; }
        }

        protected BaseBusiness(IBaseUnitOfWork<TContext> unitOfWork)
        {
            IsInExternalUnitOfWork = true;
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (false == IsInExternalUnitOfWork)
                {
                    if (_mUnitOfWork != null)
                    {
                        _mUnitOfWork.Dispose();
                        _mUnitOfWork = null;
                    }
                }
            }

            _disposed = true;
        }
    }
}
