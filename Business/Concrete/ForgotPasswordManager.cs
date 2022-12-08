using Business.Abstract;
using Core.Entites.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ForgotPasswordManager : IForgotPasswordService
    {
        private readonly IForgotPasswordDal _forgotPasswordDal;

        public ForgotPasswordManager(IForgotPasswordDal forgotPasswordDal)
        {
            _forgotPasswordDal = forgotPasswordDal;
        }

        public IDataResult<ForgotPassword> CreateForgotPassword(User user)
        {
            ForgotPassword forgotpassword = new ForgotPassword()
            {
                IsActive = true,
                userId = user.Id,
                SendDate = DateTime.Now,
                Value = Guid.NewGuid().ToString()
            };
            _forgotPasswordDal.Add(forgotpassword);

            return new SuccessDataResult<ForgotPassword>(forgotpassword);
        }

        public ForgotPassword GetForgotPassword(string value)
        {
            return _forgotPasswordDal.Get(p => p.Value == value);
        }

        public IDataResult<List<ForgotPassword>> GetListByUserId(int userId)
        {
            return new SuccessDataResult<List<ForgotPassword>>(_forgotPasswordDal.GetList(p => p.userId == userId && p.IsActive == true));
        }

        public void UpdateForgotPassword(ForgotPassword forgotPassword)
        {
            _forgotPasswordDal.Update(forgotPassword);
        }
    }
}
