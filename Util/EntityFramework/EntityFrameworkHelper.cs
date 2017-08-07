using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using DTO;

namespace Util.EntityFramework
{
    public static class EntityFrameworkHelper
    {

        public static void SetCurrentStateToEntities(this IDbContext context)
        {
            var entries = context.ChangeTracker.Entries<IObjectWithState>();
            foreach (var entry in entries)
            {
                var currentStatus = (entry.Entity as IObjectWithState).EntityState;
                entry.State = RetrieveEFState(currentStatus);
            }
        }

        public static void SetToUndefined(this DbContext context)
        {
            var entries = context.ChangeTracker.Entries<IObjectWithState>();
            foreach (var entry in entries)
            {
                (entry.Entity as IObjectWithState).EntityState = CustomEntityState.Unchanged;
            }
        }

        public static System.Data.Entity.EntityState RetrieveEFState(CustomEntityState status)
        {
            switch (status)
            {
                case CustomEntityState.Added: return System.Data.Entity.EntityState.Added;
                case CustomEntityState.Updated: return System.Data.Entity.EntityState.Modified;
                case CustomEntityState.Deleted: return System.Data.Entity.EntityState.Deleted;
                case CustomEntityState.Unchanged: return System.Data.Entity.EntityState.Unchanged;
                default: return System.Data.Entity.EntityState.Modified;
            }
        }

        public static CustomEntityState RetrieveCustomEntityStateFromBreeze(Breeze.ContextProvider.EntityState status)
        {
            switch (status)
            {
                case Breeze.ContextProvider.EntityState.Added: return CustomEntityState.Added;
                case Breeze.ContextProvider.EntityState.Modified: return CustomEntityState.Updated;
                case Breeze.ContextProvider.EntityState.Deleted: return CustomEntityState.Deleted;
                case Breeze.ContextProvider.EntityState.Unchanged: return CustomEntityState.Unchanged;
                default: return CustomEntityState.Updated;
            }
        }

        public static EntitySet GetEntitySet(this ObjectContext objectContext, object x)
        {
            

            //Supply the generic type of the method

            object set = GetEntitySetObj(objectContext, x);

            //Retrieve EntitySet of the set
            var prop = set.GetType().GetProperty("EntitySet");
            var entitySet = (EntitySet)prop.GetValue(set);
            return entitySet;
        }

        private static object GetEntitySetObj(ObjectContext objectContext, object x)
        {
            object result = null;

            //Get the approperiate overload of CreateObjectSet method
            var methodInfo = typeof(ObjectContext).GetMethods()
                             .Where(m => m.Name == "CreateObjectSet").First();

            //Invoke the method and get the ObjectSet<?> as an object          
            var type = x.GetType();

            Exception lastException = null;

            while (result == null && type != null)
            {
                try
                {
                    var genericMethodInfo = methodInfo.MakeGenericMethod(type);
                    result = genericMethodInfo.Invoke(objectContext, new object[] { });
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    type = type.BaseType;
                }
            }

            if (result == null)
            {
                throw lastException;
            }

            return result;
        }
    }
}
