using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using Tarts.Base;

namespace Tarts.Persistance
{
    public class GenericRepo
    {
        #region dependancy

        private readonly ISession _currentSession;
      

        #endregion

        #region constructor

        public GenericRepo(ISession currentSession)
        {
            _currentSession = currentSession;
         
        }

        #endregion



        public IList<T> GetByIDs<T>(IList<int> ids) where T : EntityBase
        {
            ICriteria query = _currentSession.CreateCriteria<T>();
            query.Add(Expression.In("ID", ids.ToArray<int>()));
            var lst = query.List<T>() as List<T>;
            return lst;
        }

        public T GetById<T>(int id) where T : EntityBase
        {
            var itm = _currentSession.Get<T>(id);
        
            return itm;
        }

        public T GetByUniqueID<T>(Guid uniqueID) where T : EntityBase
        {
            return GetByFieldName<T>("UniqueID", uniqueID);
        }

        public T GetByFieldName<T>(string fieldName, object value) where T : EntityBase
        {
            var query = _currentSession.CreateCriteria<T>().Add(Restrictions.Eq(fieldName, value));
            var ret = query.List<T>();

            if (ret == null) return null;
            var itm = ret.Take(1).SingleOrDefault();
           
            return itm;
        }

        public IList<T> GetAll<T>(string orderby = "")
        {
            var criteria = _currentSession.CreateCriteria(typeof(T));
            if (!string.IsNullOrEmpty(orderby))
                criteria.AddOrder(new Order(orderby, true));
            IList results = _currentSession.CreateMultiCriteria()
                .Add(criteria)
                .List();
            return ((IList)results[0]).Cast<T>().ToList();
        }
       

        public void Save<T>(T entity)
        {
            if (_currentSession.Transaction.IsActive)
            {
                _currentSession.SaveOrUpdate(entity);
                return;
            }

            using (var tran = _currentSession.BeginTransaction())
            {
                _currentSession.SaveOrUpdate(entity);
                tran.Commit();

            }
        }

        public virtual void Delete<T>(T entity)
        {

            if (_currentSession.Transaction.IsActive)
            {
                _currentSession.Delete(entity);
                return;
            }

            using (var tran = _currentSession.BeginTransaction())
            {
                _currentSession.Delete(entity);
                tran.Commit();

            }
        }
    }
}
