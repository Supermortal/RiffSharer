using System;
using System.Collections.Generic;
using System.Linq;

using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;

namespace RiffSharer.Repositories.Abstract
{
    public abstract class ARiffRepository : ATableQueryRepository<string, Riff>
    {
        #region Virtual

        public override Riff Get(string id)
        {
            return DataSet.SingleOrDefault(i => i.RiffID == id);
        }

        public override IEnumerable<Riff> GetAllPaged(int page, int pageSize)
        {
            return DataSet.OrderBy(i => i.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public virtual IEnumerable<Riff> GetAllForUser(string userId)
        {
            return DataSet.Where(i => i.UserID == userId);
        }

        public virtual IEnumerable<Riff> GetAllForUserPaged(string userId, int page, int pageSize)
        {
            return DataSet.OrderBy(i => i.DateCreated).Where(i => i.UserID == userId).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public virtual int GetCountForUser(string userId)
        {
            return DataSet.Count(i => i.UserID == userId);
        }

        #endregion

        #region Abstract

        public abstract void DeleteAll();

        #endregion
    }
}

