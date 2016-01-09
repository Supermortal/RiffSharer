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

        public User FindByUsername(string userName)
        {
            userName = userName.ToLower();
            return DataSet.SingleOrDefault(i => i.UserName.ToLower() == userName);
        }

        public User FindByEmail(string email)
        {
            email = email.ToLower();
            return DataSet.SingleOrDefault(i => i.Email.ToLower() == email);
        }

        public bool CheckUsername(string userName)
        {
            userName = userName.ToLower();
            return DataSet.Any(i => i.UserName.ToLower() == userName);
        }

        public bool CheckEmail(string email)
        {
            email = email.ToLower();
            return DataSet.Any(i => i.Email.ToLower() == email);
        }

        #endregion

        #region Abstract

        #endregion
    }
}

