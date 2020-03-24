using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBase.Data
{
    public interface IBaseRepository<DatabaseContext , Table> where Table : class where DatabaseContext : DbContext
    {
        IEnumerable<Table> GetAll();
        Table GetById(object id);
        void Insert(Table obj);
        void Update(Table obj);
        void Delete(Table obj);

        Task<IEnumerable<Table>> GetAllAsync();
        //Task<Table> GetByIdAsync(object id);
        //Task InsertAsync(Table obj);
        //Task UpdateAsync(Table obj);
        //Task DeleteAsync(Table obj);

    }
    public class BaseRepository<DatabaseContext, Table> : IBaseRepository<DatabaseContext , Table>, IDisposable where Table : class where DatabaseContext : DbContext
    {
        private IDbSet<Table> _entities;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;

        public BaseRepository(DatabaseContext context)
        {
            _isDisposed = false;
            Context = context;
        }
        public DatabaseContext Context { get; set; }
        public virtual IQueryable<Table> Tables
        {
            get { return Entities; }
        }
        protected virtual IDbSet<Table> Entities
        {
            get { return _entities ?? (_entities = Context.Set<Table>()); }
        }
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }
        public virtual IEnumerable<Table> GetAll()
        {
            return Entities.ToList();
        }
        public virtual Table GetById(object id)
        {
            return Entities.Find(id);
        }
        public virtual void Insert(Table entity)
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
        public void BulkInsert(IEnumerable<Table> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Set<Table>().AddRange(entities);
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
        public virtual void Update(Table entity)
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
        public virtual void Delete(Table entity)
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
        public virtual void SetEntryModified(Table entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public async Task<IEnumerable<Table>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        //public async Task<Table> GetByIdAsync(object id)
        //{
        //    return await Entities.Find(id);

        //}

        //public async Task InsertAsync(Table entity)
        //{
        //    try
        //    {

        //      await  Entities.AddAsync(entity);

        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //                _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
        //        throw new Exception(_errorMessage, dbEx);
        //    }
        //}

        //public async Task UpdateAsync(Table entity)
        //{
        //    try
        //    {

        //       await SetEntryModifiedAsync(entity);
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //                _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //        throw new Exception(_errorMessage, dbEx);
        //    }
        //}

        //public async Task DeleteAsync(Table entity)
        //{
        //    try
        //    {

        //      await  Entities.RemoveAsync(entity);
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //                _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
        //        throw new Exception(_errorMessage, dbEx);
        //    }
        //}
    }


}
