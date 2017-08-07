using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ManyToManyRepository<T1, T2> : IManyToManyRepository<T1, T2> where T1 : BaseEntity, new()
                                                                       where T2 : BaseEntity, new()
    {
        private readonly IDbContext dbContext = null;
        private IDbSet<T1> entities1;
        private IDbSet<T2> entities2;

        public ManyToManyRepository(ManyToManyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Create a relationship between two objects from scratch
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <param name="navigationProperty"></param>
        public void InsertWithoutData(T1 entity1, T2 entity2, string navigationProperty)
        {
            try
            {
                if (entity1 == null || entity2 == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities1.Add(entity1);
                Entities2.Add(entity2);

                var navigationPropertyList = entity1
                    .GetType()
                    .GetProperty(navigationProperty)
                    .GetValue(entity1, null);
                 
                ((ICollection<T2>)navigationPropertyList).Add(entity2);

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Create a relationship between one object and a list of other objects. None of the object exists in the database yet
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entities"></param>
        /// <param name="navigationProperty"></param>
        public void InsertWithoutData(T1 entity1, List<T2> entities, string navigationProperty)
        {
            try
            {
                if (entity1 == null || entities == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities1.Add(entity1);
                var navigationPropertyList = entity1
                .GetType()
                .GetProperty(navigationProperty)
                .GetValue(entity1, null);

                foreach (var entity2 in entities)
                {
                    Entities2.Add(entity2);
                    ((ICollection<T2>)navigationPropertyList).Add(entity2);
                }

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Insert a relationship when data already exists
        /// </summary>
        /// <param name="entity1ID"></param>
        /// <param name="entity2ID"></param>
        /// <param name="navigationProperty"></param>
        public void InsertWithData(int entity1ID, int entity2ID, string navigationProperty)
        {
            try
            {
                T1 t1 = Entities1.Find(entity1ID);
                dbContext.Entry<T1>(t1).State = EntityState.Modified;

                T2 t2 = Entities2.Find(entity2ID);
                dbContext.Entry<T2>(t2).State = EntityState.Unchanged;

                var navigationPropertyAux = t1.GetType().GetProperty(navigationProperty).GetValue(t1);
                ((ICollection<T2>)navigationPropertyAux).Add(t2);

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Insert a set of relationships when data already exists
        /// </summary>
        /// <param name="entity1ID"></param>
        /// <param name="entity2ID"></param>
        /// <param name="navigationProperty"></param>
        public void InsertWithData(int entity1ID, List<int> entities2ID, string navigationProperty)
        {
            try
            {
                T1 t1 = Entities1.Find(entity1ID);

                //foreach (var id in entities2ID)
                //{
                //    T2 t2 = Entities2.Find(id); 

                //    var navigationPropertyAux = t1.GetType().GetProperty(navigationProperty).GetValue(t1);
                //    ((ICollection<T2>)navigationPropertyAux).Add(t2);
                //}
                List<T2> entities2ToAdd = new List<T2>();
                foreach (var id in entities2ID)
                {
                    entities2ToAdd.Add(Entities2.Find(id));
                }

                var navigationPropertyAux = t1.GetType().GetProperty(navigationProperty).GetValue(t1);
                ((List<T2>)navigationPropertyAux).AddRange(entities2ToAdd);

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <summary>
        /// Delete a specific relationship
        /// </summary>
        /// <param name="entity1ID"></param>
        /// <param name="entity2ID"></param>
        /// <param name="navigationProperty"></param>
        public void DeleteRelationship(int entity1ID, int entity2ID, string navigationProperty)
        {
            T1 t1 = Entities1.FirstOrDefault(x => x.Id == entity1ID);
            T2 t2 = Entities2.FirstOrDefault(x => x.Id == entity2ID);

            var navigationPropertyAux = t1.GetType().GetProperty(navigationProperty).GetValue(t1);
            ((ICollection<T2>)navigationPropertyAux).Remove(t2);

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Delete all relationships of Entity1
        /// </summary>
        /// <param name="entity1ID"></param>
        /// <param name="navigationProperty"></param>
        public void DeleteAllRelationship(int entity1ID, string navigationProperty)
        {
            T1 t1 = Entities1.FirstOrDefault(x => x.Id == entity1ID);

            var navigationPropertyAux = t1.GetType().GetProperty(navigationProperty).GetValue(t1);
            ((ICollection<T2>)navigationPropertyAux).Clear();

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Delete a set of relationships
        /// </summary>
        /// <param name="entity1ID"></param>
        /// <param name="listID"></param>
        /// <param name="navigationProperty"></param>
        public void DeleteRelationship(int entity1ID, List<int> listID, string navigationProperty)
        {
            T1 t1 = Entities1.FirstOrDefault(x => x.Id == entity1ID);
            
            var navigationPropertyAux = t1.GetType().GetProperty(navigationProperty).GetValue(t1);

            foreach (var id in listID)
            {
                T2 t2 = Entities2.FirstOrDefault(x => x.Id == id);
                ((ICollection<T2>)navigationPropertyAux).Remove(t2);
            }

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Update a relationship by replacing a old item with a new one
        /// </summary>
        /// <param name="idT1">Entity1 ID</param>
        /// <param name="oldIDT2">Old ID of Entity2</param>
        /// <param name="newIDT2">New ID of Entity2</param>
        /// <param name="navigationProperty">Navigation Property Name</param>
        public void UpdateRelationship(int idT1, int oldIDT2, int newIDT2, string navigationProperty)
        {
            // Checking whether the Navigation Property have been updated
            if (oldIDT2 != newIDT2)
            {
                DeleteRelationship(idT1, oldIDT2, navigationProperty);
            }

            InsertWithData(idT1, newIDT2, navigationProperty);
        }

        /// <summary>
        /// Update a relationship by removing a list of old items and adding a list of new ones
        /// </summary>
        /// <param name="idT1">Entity1 ID</param>
        /// <param name="oldIDT2List">Old ID List of Entity2</param>
        /// <param name="newIDT2List">New ID List of Entity2</param>
        /// <param name="navigationProperty">Navigation Property Name</param>
        public void UpdateRelationship(int idT1, List<int> oldIDT2List, List<int> newIDT2List, string navigationProperty)
        {
            // Checking whether the Navigation Property have been updated
            if (oldIDT2List.Except(newIDT2List).Any())
            {
                DeleteRelationship(idT1, oldIDT2List, navigationProperty);
            }

            InsertWithData(idT1, newIDT2List, navigationProperty);
        }

        /// <summary>
        /// Update a relationship by adding a new list of items
        /// </summary>
        /// <param name="idT1">Entity1 ID</param>
        /// <param name="newIDT2List">New ID List of Entity2</param>
        /// <param name="navigationProperty">Navigation Property Name</param>
        public void UpdateRelationship(int idT1, List<int> newIDT2List, string navigationProperty)
        {
            InsertWithData(idT1, newIDT2List, navigationProperty);
        }

        /// <summary>
        /// Get T1 entity by T2's Id 
        /// </summary>
        /// <param name="entity2ID"></param>
        /// <param name="navigationProperty"></param>
        /// <returns></returns>
        public List<DTOGenericObject> GetT1ByT2(int entity2ID, string navigationProperty)
        {
            var result = (
            from t2 in Entities2
            from t1 in ((ICollection<T2>)t2.GetType().GetProperty(navigationProperty).GetValue(t2))
            join t in Entities1 on t1.Id equals t.Id
            where t2.Id == entity2ID
            select new DTOGenericObject
            {
                ID = t.Id,
                Name = GetNavigationPropertyValue(t).ToString()
            }).ToList();

            return result;
        }

        /// <summary>
        /// Return a PropertyInfo object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private object GetNavigationPropertyValue<T> (T t)
        {
            var type = t.GetType();
            var properties = type.GetProperties();
            
            var property = properties
                .FirstOrDefault(p => p.GetCustomAttributes(false)
                    .Any(a => a.GetType() == typeof(DisplayableAttribute)));

            return property.GetValue(t);
        }

        /// <summary>
        /// List of entities of type T1
        /// </summary>
        private IDbSet<T1> Entities1
        {
            get
            {
                if (entities1 == null)
                {
                    entities1 = dbContext.Set<T1>();
                }
                return entities1;
            }
        }

        /// <summary>
        /// List of entities of type T2
        /// </summary>
        private IDbSet<T2> Entities2
        {
            get
            {
                if (entities2 == null)
                {
                    entities2 = dbContext.Set<T2>();
                }
                return entities2;
            }
        }
    }
}
