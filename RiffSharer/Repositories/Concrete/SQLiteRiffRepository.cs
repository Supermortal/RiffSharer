using System;

using Supermortal.Common.PCL.Abstract;
using Supermortal.Common.PCL.Helpers;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Repositories.Concrete
{
    public class SQLiteRiffRepository : ARiffRepository
    {
        private readonly ISQLite _sqlLite;

        public SQLiteRiffRepository()
            : this(IoCHelper.Instance.GetService<ISQLite>())
        {

        }

        public SQLiteRiffRepository(ISQLite sqlLite)
        {
            _sqlLite = sqlLite;
            //TESTING
            DropTable();
            //TESTING
            CreateTable();
            DataSet = _sqlLite.GetConnection().Table<Riff>();
        }

        public void CreateTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.CreateTable<Riff>();
            }
        }

        public void DropTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Riff>();
            }
        }

        public override void DeleteAll()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Riff>();
                conn.CreateTable<Riff>();
            }
        }

        public override Riff Insert(Riff obj)
        {
            obj.DateCreated = DateTime.UtcNow;
            obj.RiffID = Guid.NewGuid().ToString();

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
                conn.Delete<Riff>(id);
            }
        }

        public override Riff Update(Riff obj)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var a = conn.Find<Riff>(obj.RiffID);
                a.UpdateModel(obj);
                conn.Update(a);

                return a;
            }
        }
    }
}

