using System;
using System.Collections.Generic;
using System.Linq;

using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;

namespace RiffSharer.Repositories.Abstract
{
    public abstract class ARatingRepository : ATableQueryRepository<string, Rating>
    {
        #region Virtual

        public override Rating Get(string id)
        {
            return DataSet.SingleOrDefault(i => i.RatingID == id);
        }

        public override IEnumerable<Rating> GetAllPaged(int page, int pageSize)
        {
            return DataSet.OrderBy(i => i.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region Abstract

        public abstract void DeleteAll();

        #endregion
    }
}

