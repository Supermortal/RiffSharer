using System;

using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;

namespace RiffSharer.Repositories.Abstract
{
    public abstract class ASavedUserRepository : ATableQueryRepository<int, SavedUser>
    {

        public override System.Collections.Generic.IEnumerable<SavedUser> GetAllPaged(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        #region Abstract

        public abstract SavedUser Get();

        public abstract SavedUser InsertOrUpdate(SavedUser user);

        public abstract SavedUser Delete();

        #endregion
    }
}

