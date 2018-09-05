using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;


using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
// using MMercan.Common.Interfaces;

namespace Mercan.Common.Repos
{
    public class EFRepository<T> : IRepository<T>,IDisposable where T : class, new()
    {
        public readonly DbContext db;
        public readonly DbSet<T> dbSet;
        private PropertyInfo idField;
        ILogger logger { get; }

        public EFRepository(DbContext db, Expression<Func<T, object>> idField, ILogger logger = null) : this(db)
        {
            this.logger = logger;
            //idField.Compile()(default(T));
            if (idField != null)
            {
                // var memberExpression = idField.Body as MemberExpression ?? ((UnaryExpression)idField.Body).Operand as MemberExpression;
                if (idField.Body is MemberExpression)
                {
                    this.idField = (idField.Body as MemberExpression).Member as PropertyInfo;
                    Logit(LogLevel.Trace, "id field added from parameter : " + this.idField.Name);
                }
                else if (idField.Body is UnaryExpression)
                {
                    var res = ((UnaryExpression)idField.Body).Operand as MemberExpression;
                    this.idField = res.Member as PropertyInfo;
                    Logit(LogLevel.Trace, "id field added from parameter : " + this.idField.Name);
                }
            }
            else
            {
                Type typeParameterType = typeof(T);
                var item = typeParameterType.GetProperties().FirstOrDefault(p => p.CustomAttributes.Any(p1 => p1.AttributeType == typeof(KeyAttribute)));
                if (item == null)
                {
                    item = typeParameterType.GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
                    Logit(LogLevel.Warning, "id field added from Name match : " + idField.Name);
                }
                if (item != null)
                {
                    this.idField = item;
                    Logit(LogLevel.Trace, "id field added from Key attribute : " + idField.Name);
                }
            }
        }

        public EFRepository(DbContext db, string idField, ILogger logger = null)
        {
            this.logger = logger;
            Type typeParameterType = typeof(T);
            var item = typeParameterType.GetProperties().FirstOrDefault(p => p.Name.ToLower() == idField);
            if (item == null) throw new ArgumentException(idField + " is not a Property in " + typeof(T).GetType().ToString());
            else this.idField = item;
        }

        public EFRepository(DbContext db, ILogger logger = null)
        {
            this.logger = logger;
            if (db == null)
            {
                throw new ArgumentNullException("db");
            }
            this.db = (DbContext)db;
            //   this.db.Configuration.LazyLoadingEnabled = false;
            //   this.db.Configuration.ProxyCreationEnabled = false;
            dbSet = this.db.Set<T>();

            Type typeParameterType = typeof(T);
            var item = typeParameterType.GetProperties().FirstOrDefault(p => p.CustomAttributes.Any(p1 => p1.AttributeType == typeof(KeyAttribute)));
            if (item == null)
            {
                item = typeParameterType.GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
            }
            if (item != null)
            {
                this.idField = item;
            }
        }
        public virtual IQueryable<T> GetAll()
        {
            return dbSet;
        }
        public virtual Task<IQueryable<T>> GetAllAsync()
        {
            Task<IQueryable<T>> ts = Task<IQueryable<T>>.Factory.StartNew(() => dbSet);
            return ts;
        }

        public virtual Task<T> FindAsync(object id)
        {
            Task<T> findTask = Task<T>.Factory.StartNew(() => Find(id));
            return findTask;
        }

        public virtual T Find(T item)
        {
            object id = idField.GetValue(item);
            return Find(id);
        }

        public static object Cast(string id, Type t)
        {
            if (t == typeof(string))
            {
                return id;
            }
            else if (t == typeof(Guid))
            {
                return Guid.Parse(id);
            }
            else if (t == typeof(int))
            {
                return int.Parse(id);
            }
            else if (t == typeof(byte))
            {
                return Byte.Parse(id);
            }
            else if(t==typeof(long))
            {
                return long.Parse(id);
            }
            else if (t == typeof(decimal))
            {
                return decimal.Parse(id);
            }
            else if (t == typeof(float))
            {
                return float.Parse(id);
            }
            else if (t == typeof(char))
            {
                return Convert.ToChar(id);
            }
            else if (id is IConvertible)
            {
                return Convert.ChangeType(id, t) as dynamic;
            }
            else
            {
                try
                {
                    var param = Expression.Parameter(typeof(object));
                    return Expression.Lambda(Expression.Convert(param, t), param)
                        .Compile().DynamicInvoke(id) as dynamic;
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;

                   
                }
            }
        }

        public virtual T FindStringID(string id)
        {
            var res = Cast(id, idField.PropertyType);
            return Find(res);
        }

        public virtual T Find(object id)
        {
            var queryable = dbSet.AsQueryable();
            ParameterExpression pe = Expression.Parameter(typeof(T), "p");

            Expression l1 = Expression.Property(pe, idField.Name);
            Expression r1 = Expression.Constant(id);
            Expression e11 = Expression.Equal(l1, r1);

            MethodCallExpression whereExpression = Expression.Call(typeof(Queryable),"Where",
                new Type[] { queryable.ElementType },
                queryable.Expression,
                Expression.Lambda<Func<T, bool>>(e11, new ParameterExpression[] { pe }));

            IQueryable<T> results = queryable.Provider.CreateQuery<T>(whereExpression);
            return results.FirstOrDefault();
        }

       

        //public virtual T Find(object id)
        //{
        //    return Find(id.ToString()).FirstOrDefault();
        //}
        ////public PropertyInfo findKey()
        ////{
        ////    PropertyInfo pro = typeof(T).GetProperties().SingleOrDefault(p => p.IsDefined(typeof(KeyAttribute)));
        ////    if (pro == null)
        ////    {
        ////        pro = typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("ID"));
        ////    }
        ////    return pro;
        ////}
        ////public IEnumerable<T> Find(string searchTerm)
        ////{
        ////    PropertyInfo getter = null;
        ////    if (idField!=null)
        ////    {
        ////        getter = idField;
        ////    } else
        ////    {
        ////        getter = findKey();
        ////    }

        ////    if (getter == null)
        ////    {
        ////        throw new ArgumentOutOfRangeException("searchField");
        ////    }
        ////    return dbSet.Where(x => getter.GetValue(x, null).ToString() == searchTerm);
        ////}

        ////public IEnumerable<T> getPeople(string searchField, string searchTerm)
        ////{
        ////    PropertyInfo getter = typeof(T).GetProperty(searchField);
        ////    if (getter == null)
        ////    {
        ////        throw new ArgumentOutOfRangeException("searchField");
        ////    }
        ////    return dbSet.Where(x => getter.GetValue(x, null).ToString() == searchTerm);

        ////}

        ////public T Find(Expression<Func<T, bool>> predicate)
        ////{
        ////    return dbSet.SingleOrDefault(predicate);
        ////}





        public virtual T Add(T item)
        {
            EntityEntry dbEntityEntry = db.Entry(item);
            bool isValid = true;//dbEntityEntry.GetValidationResult().IsValid;
            if (isValid)
            {
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                //  else
                // {
                dbSet.Add(item);
                // }
                return item;
            }
            throw new Exception("Model is Not Valid");
        }

        public virtual void Update(T item)
        {
            EntityEntry entityEntry = db.Entry(item);
            bool isValid = true;//dbEntityEntry.GetValidationResult().IsValid;
            if (isValid)
            {
                if (entityEntry.State == EntityState.Detached)
                {
                    dbSet.Attach(item);
                }
                entityEntry.State = EntityState.Modified;
            }
            else
            {
                throw new Exception("Model is Not Valid");
            }
        }

        public virtual void Delete(T item)
        {
            EntityEntry dbEntityEntry = db.Entry(item);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                dbSet.Attach(item);
                dbSet.Remove(item);
            }
        }

        public virtual T Delete(object id)
        {
            T item = Find(id);
            if (item != null)
            {
                EntityEntry dbEntityEntry = db.Entry(item);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    dbSet.Attach(item);
                    dbSet.Remove(item);
                }
            }
            return item;
        }


        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        private void Logit(LogLevel logLevel, string message)
        {
            if (logger != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                        logger.LogTrace(message, this);
                        break;
                    case LogLevel.Debug:
                        logger.LogDebug(message, this);
                        break;
                    case LogLevel.Information:
                        logger.LogInformation(message, this);
                        break;
                    case LogLevel.Warning:
                        logger.LogWarning(message, this);
                        break;
                    case LogLevel.Error:
                        logger.LogError(message, this);
                        break;
                    case LogLevel.Critical:
                        logger.LogCritical(message, this);
                        break;
                    case LogLevel.None:
                        logger.LogTrace(message, this);
                        break;
                    default:
                        break;
                }

            }
        }
    }
}