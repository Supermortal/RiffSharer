using System;
using System.Collections.Generic;
using System.Linq;

using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;

namespace RiffSharer.Repositories.Abstract
{
    public abstract class ACommentRepository : ATableQueryRepository<string, Comment>
    {
        #region Virtual

        public override Comment Get(string id)
        {
            return DataSet.SingleOrDefault(i => i.CommentID == id);
        }

        public override IEnumerable<Comment> GetAllPaged(int page, int pageSize)
        {
            return DataSet.OrderBy(i => i.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
        }

        #endregion

        #region Abstract

        public abstract void DeleteAll();

        #endregion
    }
}

