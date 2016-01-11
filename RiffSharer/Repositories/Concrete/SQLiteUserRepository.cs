using System;

using SQLite.Net;
using SQLite.Net.Async;

using Supermortal.Common.PCL.Abstract;
using Supermortal.Common.PCL.Helpers;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer
{
    public class SQLiteUserRepository : AUserRepository
    {
        private readonly ISQLite _sqlLite;

        public SQLiteUserRepository()
            : this(IoCHelper.Instance.GetService<ISQLite>())
        {

        }

        public SQLiteUserRepository(ISQLite sqlLite)
        {
            _sqlLite = sqlLite;
            //TESTING
//            _sqlLite.DeleteDatabase();
            //TESTING
            CreateTable();
            DataSet = _sqlLite.GetConnection().Table<User>();
        }

        public void CreateTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.CreateTable<User>();
            }
        }

        public override User Insert(User obj)
        {
            obj.DateCreated = DateTime.UtcNow;
            obj.UserID = Guid.NewGuid().ToString();

            using (var conn = _sqlLite.GetConnection())
            {
                conn.Insert(obj);
            }

            return obj;
        }

        public override void Delete(string id)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.Delete<User>(id);
            }
        }

        public override User Update(User obj)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var u = conn.Find<User>(obj.UserID);
                u.UpdateModel(obj);
                conn.Update(u);

                return u;
            }
        }
    }
}

