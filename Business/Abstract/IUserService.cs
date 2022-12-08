using Core.Entites.Concrete;
using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        IResult Add(User user);
        IResult Update(User user);
        User GetById(int id);
        User GetByMail(string email);
        IDataResult<User> GetByIdToResult(int id);
        User GetByMailConfirmValue(string value);
    }
}
