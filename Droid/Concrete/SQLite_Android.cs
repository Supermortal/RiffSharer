using System;

using Supermortal.Common.PCL.Abstract;
using Supermortal.Common.Droid.Concrete;
using SQLite.Net.Async;
using SQLite.Net;

namespace RiffSharer.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        public SQLiteConnection GetConnection()
        {
            return new DroidSQLite().GetConnection("RiffSharer.db3");
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return new DroidSQLite().GetAsyncConnection("RiffSharer.db3");
        }

        public void DeleteDatabase()
        {
            new DroidSQLite().DeleteDatabase("RiffSharer.db3");
        }
    }
}

