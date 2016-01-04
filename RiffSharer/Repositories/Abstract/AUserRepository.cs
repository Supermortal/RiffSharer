using System;
using System.Collections.Generic;
using System.Linq;
using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;

namespace RiffSharer.Repositories.Abstract
{
    public abstract class AUserRepository : ATableQueryRepository<string, User>
    {
        #region Virtual

        public override User Get(string id)
        {
            return DataSet.SingleOrDefault(i => i.UserID == id);
        }

        public override IEnumerable<User> GetAllPaged(int page, int pageSize)
        {
            return DataSet.OrderBy(i => i.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region Abstract

        #endregion
    }
}

