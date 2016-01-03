using System;
using System.Collections.Generic;
using System.Linq;
using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;

namespace RiffSharer
{
    public abstract class AAudioRepository : ATableQueryRepository<string, Audio>
    {
        #region Virtual

        public override Audio Get(string id)
        {
            return DataSet.SingleOrDefault(i => i.AudioID == id);
        }

        public override IEnumerable<Audio> GetAllPaged(int page, int pageSize)
        {
            return DataSet.OrderBy(i => i.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region Abstract

        public abstract void DeleteAll();

        #endregion
    }
}

