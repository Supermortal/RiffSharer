using System;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Abstract;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Repositories.Concrete
{
    public class SQLiteSavedUserRepository : ASavedUserRepository
    {

        private readonly ISQLite _sqlLite;
        private const int SAVED_USER_ID = 0;

        public SQLiteSavedUserRepository()
            : this(IoCHelper.Instance.GetService<ISQLite>())
        {

        }

        public SQLiteSavedUserRepository(ISQLite sqlLite)
        {
            _sqlLite = sqlLite;
            //TESTING
//            _sqlLite.DeleteDatabase();
            //TESTING
            CreateTable();
            DataSet = _sqlLite.GetConnection().Table<SavedUser>();
        }

        public void CreateTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.CreateTable<SavedUser>();
            }
        }

        public override SavedUser InsertOrUpdate(SavedUser user)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var su = conn.Find<SavedUser>(SAVED_USER_ID);

                if (su == null)
                {
                    user.SavedUserID = SAVED_USER_ID;
                    conn.Insert(user);
                    return user;
                }

                su.SavedUserID = user.SavedUserID;

                conn.Update(su);

                return su;
            }
        }

        public override SavedUser Get()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                return conn.Find<SavedUser>(SAVED_USER_ID);
            }
        }

        public override SavedUser Delete()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var su = conn.Find<SavedUser>(SAVED_USER_ID);
                conn.Delete<SavedUser>(SAVED_USER_ID);
                return su;
            }
        }

        #region Not Implemented

        public override SavedUser Get(int id)
        {
            throw new NotImplementedException();
        }

        public override SavedUser Insert(SavedUser obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override SavedUser Update(SavedUser obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

