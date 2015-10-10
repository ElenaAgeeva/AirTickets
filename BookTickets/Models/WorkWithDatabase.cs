using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTickets.Models
{
    public class WorkWithDatabase : IDisposable
    {
        UserContext db = null;

        public WorkWithDatabase()
        {
            db = new UserContext();
        }

        public TEntity GetEntityById<TEntity, TId>(TId Id) where TEntity : class, IBase<TId>
        {
            return db.Set<TEntity>().AsEnumerable().Where(x => x.Id.Equals(Id)).FirstOrDefault();
        }

        public List<TEntity> GetEntityList<TEntity>() where TEntity : class
        {
            return db.Set<TEntity>().AsEnumerable().ToList();
        }

        public void Update()
        {
            db.SaveChanges();
        }

        public void Commit()
        {
            db.SaveChanges();
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            db.Set<TEntity>().Add(entity);
        }

        public void Dispose()
        {
            db.Dispose();
            db = null;
        }
    }
}