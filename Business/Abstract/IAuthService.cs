using Core.Entites.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegister userForRegister, string password);
        IDataResult<User> Login(UserForLogin userForLogin);
        IDataResult<User> GetByEmail(string email);
        IResult UserExists(string email);
        IResult Update(User user);
        IResult ChangePassword(User user);
        IDataResult<User> GetById(int id);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IResult SendForgotPasswordEmail(User user, string value);
        IResult SendConfirmEmailAgain(User user);
        IDataResult<User> GetByMailConvirmValue(string value);
    }
}
