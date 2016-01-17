using System;

using Supermortal.Common.PCL.Abstract;
using Supermortal.Common.PCL.Helpers;

using RiffSharer.Models;
using RiffSharer.Repositories.Abstract;

namespace RiffSharer.Repositories.Concrete
{
    public class SQLiteRatingRepository : ARatingRepository
    {
        private readonly ISQLite _sqlLite;

        public SQLiteRatingRepository()
            : this(IoCHelper.Instance.GetService<ISQLite>())
        {

        }

        public SQLiteRatingRepository(ISQLite sqlLite)
        {
            _sqlLite = sqlLite;
            //TESTING
            DropTable();
            //TESTING
            CreateTable();
            DataSet = _sqlLite.GetConnection().Table<Rating>();
        }

        public void CreateTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.CreateTable<Rating>();
            }
        }

        public void DropTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Rating>();
            }
        }

        public override void DeleteAll()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Rating>();
                conn.CreateTable<Rating>();
            }
        }

        public override Rating Insert(Rating obj)
        {
            obj.DateCreated = DateTime.UtcNow;
            obj.RatingID = Guid.NewGuid().ToString();

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
                conn.Delete<Rating>(id);
            }
        }

        public override Rating Update(Rating obj)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var a = conn.Find<Rating>(obj.RatingID);
                a.UpdateModel(obj);
                conn.Update(a);

                return a;
            }
        }
    }
}

