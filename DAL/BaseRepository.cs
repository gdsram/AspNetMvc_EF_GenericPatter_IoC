using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Util.EntityFramework;
using DTO;
using Util.Extensions;

namespace DAL
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IObjectWithState
    {
        public IDbContext Context { get; }

        public BaseRepository(IDbContext Context)
        {
            this.Context = Context;
        }

        public DataTable GenerateDatatableForStructured<V>(IEnumerable<V> input)
        {


            var result = new DataTable();
            result.Columns.Add(new DataColumn("Value", typeof(T)));

            if (input == null)
            {
                return result;
            }

            foreach (var obj in input)
            {
                var row = result.NewRow();
                row.SetField("Value", obj);
                result.Rows.Add(row);
            }
            return result;
        }

        public IEnumerable<string> GetIdFields<V>(V objRef, params Type[] contextTypes) where V : class, IObjectWithState
        {
            EntitySet set = null;
            var context = this.GetEntityContext(objRef, out set, contextTypes);
            if (context == null)
            {
                return new List<string>();
            }


            var keyNames = set.ElementType.KeyMembers.Select(k => k.Name);

            return keyNames;
        }

        public Dictionary<string, object> GetIdDictionary<V>(V objRef, params Type[] contextTypes) where V : class, IObjectWithState
        {
            var idFieldNames = this.GetIdFields<V>(objRef, contextTypes);

            var result = new Dictionary<string, object>();
            object idFieldValue;

            foreach (var idFieldName in idFieldNames)
            {
                idFieldValue = objRef.GetPropertyValue<object>(idFieldName);
                result.Add(idFieldName, idFieldValue);
            }

            return result;
        }

        public DbContext GetEntityContext<V>(V objRef, out EntitySet entitySet, params Type[] contextTypes)
        {
            var internalContexts = new List<Type>(contextTypes);

            if (!contextTypes.Any(type => type.FullName == this.Context.GetType().FullName))
            {
                internalContexts.Add(this.Context.GetType());
            }

            DbContext objectContext = null;
            ObjectContext internalContext = null;

            entitySet = null;

            foreach (var contextType in internalContexts)
            {
                try
                {
                    objectContext = Activator.CreateInstance(contextType) as DbContext;
                    internalContext = ((IObjectContextAdapter)objectContext).ObjectContext;
                    entitySet = internalContext.GetEntitySet(objRef);
                    break;
                }
                catch (Exception)
                {
                    objectContext = null;
                }
            }

            return objectContext;
        }

        public DbEntityEntry<T> Entry(T entry)
        {
            return Context.Entry(entry);
        }

        public DbEntityEntry<T> Attach(DbEntityEntry<T> entry)
        {
            entry.State = EntityState.Unchanged;

            return entry;
        }

        public DbEntityEntry<V> Attach<V>(DbEntityEntry<V> entry) where V : class
        {
            entry.State = EntityState.Unchanged;

            return entry;
        }


        public DbEntityEntry<V> Entry<V>(V entry) where V : class
        {
            return Context.Entry(entry);
        }


        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = Context.Set<T>().AsQueryable();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public void Delete<TId>(TId id)
        {
            var entity = Context.Set<T>().Find(id);
            this.Delete(entity);
        }

        public void Delete(T entity)
        {
            (entity as IObjectWithState).EntityState = CustomEntityState.Deleted;
        }


        public bool Any(params Expression<Func<T, bool>>[] expressions)
        {
            var query = Context.Set<T>().AsQueryable();
            foreach (var expression in expressions)
            {
                query = query.Where(expression);
            }

            return query.Any();
        }

        protected void DeleteRelationship<T1, T2>(T1 parent, Expression<Func<T1, object>> expression, params T2[] children)
            where T1 : class
            where T2 : class
        {

            var obj = (this.Context as IObjectContextAdapter).ObjectContext;
            foreach (T2 child in children)
            {
                Context.Set<T2>().Attach(child);
                obj.ObjectStateManager.ChangeRelationshipState(parent, child, expression, EntityState.Deleted);
            }
            Context.SaveChanges();
        }

        public virtual void InsertOrUpdate(params T[] entities)
        {
            foreach (var entity in entities)
            {
                this.Context.Entry(entity).State = EntityState.Added;
            }
        }

        public void SaveChanges()
        {
            this.Context.SetCurrentStateToEntities();
            this.Context.SaveChanges();
            this.Context.SetToUndefined();
        }

        public virtual T Find<TId>(TId id, bool tracking = true)
        {
            var query = Context.Set<T>();

            var entity = query.Find(id);
            /*var objectContext = (((IObjectContextAdapter)context).ObjectContext);
            objectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, entity);
            */
            return entity;
        }

        public IQueryable<T> Retrieve(bool traking = true, params Expression<Func<T, bool>>[] predicates)
        {
            var query = Context.Set<T>().AsQueryable();
            if (!traking)
            {
                query = query.AsNoTracking();
            }

            if (predicates == null)
            {
                predicates = new List<Expression<Func<T, bool>>>().ToArray();
            }

            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public void RejectChanges()
        {
            foreach (var entry in this.Context.ChangeTracker
                             .Entries<T>()
                             .Where(e => e.State != EntityState.Unchanged))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.Entity.EntityState = CustomEntityState.Unchanged;
            }
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
