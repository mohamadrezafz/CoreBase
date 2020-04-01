using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBase.Data
{
    public interface IBaseRepository<TContext , TEntity> where TEntity : class where TContext : DbContext
    {
        TEntity GetById(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void BulkDelete(List<TEntity> entities);
        void Delete(TEntity entityToDelete);
        void BulkInsert(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        IQueryable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        void Dispose();
        IEnumerable<TEntity> GetAll();

    }
    public abstract class BaseRepository<TContext, TEntity> : IBaseRepository<TContext, TEntity>, IDisposable where TEntity : class where TContext : DbContext
    {
        private IDbSet<TEntity> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;

        public BaseRepository(TContext context)
        {
            _isDisposed = false;
            Context = context;
        }
        public TContext Context { get; set; }
        public virtual IQueryable<TEntity> Tables
        {
            get { return Entities; }
        }
        protected virtual IDbSet<TEntity> Entities
        {
            get { return _entities ?? (_entities = Context.Set<TEntity>()); }
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }
        public virtual IEnumerable<TEntity> GetAll()
        {
            return Entities.ToList();
        }
        public virtual TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        public IQueryable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate);
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {

                Entities.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Set<TEntity>().AddRange(entities);
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                        validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public virtual void Update(TEntity entity)
        {
            try
            {

                SetEntryModified(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public virtual void Delete(TEntity entity)
        {
            try
            {

                Entities.Remove(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public virtual void BulkDelete(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Delete(entity);
            }
        }
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = Entities.Find(id);
            Delete(entityToDelete);
        }

        public virtual void SetEntryModified(TEntity entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        
    }


}
