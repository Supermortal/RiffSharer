using System;

using Supermortal.Common.PCL.Abstract;
using Supermortal.Common.PCL.Helpers;

using RiffSharer.Models;
using RiffSharer.Repositories.Abstract;

namespace RiffSharer.Repositories.Concrete
{
    public class SQLiteAudioRepository : AAudioRepository
    {
        private readonly ISQLite _sqlLite;

        public SQLiteAudioRepository()
            : this(IoCHelper.Instance.GetService<ISQLite>())
        {

        }

        public SQLiteAudioRepository(ISQLite sqlLite)
        {
            _sqlLite = sqlLite;
            //TESTING
            DropTable();
            //TESTING
            CreateTable();
            DataSet = _sqlLite.GetConnection().Table<Audio>();
        }

        public void CreateTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.CreateTable<Audio>();
            }
        }

        public void DropTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Audio>();
            }
        }

        public override void DeleteAll()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Audio>();
                conn.CreateTable<Audio>();
            }
        }

        public override Audio Insert(Audio obj)
        {
            obj.DateCreated = DateTime.UtcNow;
            obj.AudioID = Guid.NewGuid().ToString();

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
                conn.Delete<Audio>(id);
            }
        }

        public override Audio Update(Audio obj)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var a = conn.Find<Audio>(obj.AudioID);
                a.UpdateModel(obj);
                conn.Update(a);

                return a;
            }
        }
    }
}

