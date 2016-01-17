using System;

using Supermortal.Common.PCL.Abstract;
using Supermortal.Common.PCL.Helpers;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Repositories.Concrete
{
    public class SQLiteCommentRepository : ACommentRepository
    {
        private readonly ISQLite _sqlLite;

        public SQLiteCommentRepository()
            : this(IoCHelper.Instance.GetService<ISQLite>())
        {

        }

        public SQLiteCommentRepository(ISQLite sqlLite)
        {
            _sqlLite = sqlLite;
            //TESTING
            DropTable();
            //TESTING
            CreateTable();
            DataSet = _sqlLite.GetConnection().Table<Comment>();
        }

        public void CreateTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.CreateTable<Comment>();
            }
        }

        public void DropTable()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Comment>();
            }
        }

        public override void DeleteAll()
        {
            using (var conn = _sqlLite.GetConnection())
            {
                conn.DropTable<Comment>();
                conn.CreateTable<Comment>();
            }
        }

        public override Comment Insert(Comment obj)
        {
            obj.DateCreated = DateTime.UtcNow;
            obj.CommentID = Guid.NewGuid().ToString();

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
                conn.Delete<Comment>(id);
            }
        }

        public override Comment Update(Comment obj)
        {
            using (var conn = _sqlLite.GetConnection())
            {
                var a = conn.Find<Comment>(obj.CommentID);
                a.UpdateModel(obj);
                conn.Update(a);

                return a;
            }
        }
    }
}

