using Business.Abstract;
using Business.Constans;
using Core.Entites.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.JWT;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IOperationClaimService _OperationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IMailParameterService _mailParameterService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, IMailParameterService mailParameterService, IMailService mailService, IMailTemplateService mailTemplateService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _OperationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _mailParameterService = mailParameterService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
        }
        public IResult ChangePassword(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.ChangedPassword);
        }
        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.SuccessfulLogin);
        }
        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userService.GetByMail(email));
        }
        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userService.GetById(id));
        }
        public IDataResult<User> Login(UserForLogin userForLogin)
        {
            var userToCheck = _userService.GetByMail(userForLogin.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }
        public IDataResult<User> Register(UserForRegister userForRegister, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User()
            {
                Email = userForRegister.Email,
                AddedAt = DateTime.Now,
                IsActive = true,
                MailConfirm = false,
                MailConfirmDate = DateTime.Now,
                MailConfirmValue = Guid.NewGuid().ToString(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = userForRegister.Name
            };
            _userService.Add(user);
            SendConfirmEmail(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }
        public IResult SendConfirmEmailAgain(User user)
        {
            if (user.MailConfirm == true)
            {
                return new ErrorResult(Messages.MailAlreadyConfirm);
            }

            DateTime confirmMailDate = user.MailConfirmDate;
            DateTime now = DateTime.Now;
            if (confirmMailDate.ToShortDateString() == now.ToShortDateString())
            {
                if (confirmMailDate.Hour == now.Hour && confirmMailDate.AddMinutes(5).Minute <= now.Minute)
                {
                    SendConfirmEmail(user);
                    return new SuccessResult(Messages.MailConfirmSendSuccess);
                }
                else
                {
                    return new ErrorResult(Messages.MailConfirmSendSuccessful);
                }
            }
            SendConfirmEmail(user);
            return new SuccessResult(Messages.MailConfirmSendSuccess);
        }
        void SendConfirmEmail(User user)
        {
            string subject = "Kullanıcı Kayıt Onay maili";
            string body = "Kullanıcınız sisteme dahil oldu. Kaydınızı tamamlamanız için aşağıdaki linke tıklamanız gerekmektedir.";
            //string link = "https://localhost:7154/api/auth/confirmuser?value=" + user.MailConfirmValue;
            string link = "http://localhost:4200/registerConfirm/" + user.MailConfirmValue;
            string linkDescription = "Kaydı onaylamak için tıklayın";

            var mailTemplate =  _mailTemplateService.GetByTemplateName("Kayıt");
            string templateBody =  mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{titleMessage}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(1);
            SendMailDto sendMailDto = new SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = user.Email,
                subject = "Kullanıcı Kayıt Onay maili",
                body = templateBody
            };
            _mailService.SendMail(sendMailDto);

            user.MailConfirmDate = DateTime.Now;
            _userService.Update(user);
        }
        public IResult SendForgotPasswordEmail(User user, string value)
        {
            string subject = "Şifremi Unuttum";
            string body = " şifrenizi unuttuğunuz belirttiniz.Aşağıdaki linkten şifrenizi yeniden belirleyiniz.linkin 1 saat süresi vardır. Süre sonunda kullanılamaz.";
            //string link = "https://localhost:7154/api/auth/confirmuser?value=" + value;
            string link = "http://localhost:4200/forgot-password/" + value;
            string linkDescription = "Şifre belirlemek için tıklayın";

            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt");
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{titleMessage}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(1);
            SendMailDto sendMailDto = new SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = user.Email,
                subject = subject,
                body = templateBody
            };
            _mailService.SendMail(sendMailDto);


            return new SuccessResult("Mail Başarıyla gönderildi");
        }
        public IResult Update(User user)
        {
            _userService.Update(user);
            return new SuccessResult(Messages.UserMailConfirmSuccessful);
        }
        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExist);
            }
            return new SuccessResult();
        }
        public IDataResult<User> GetByMailConvirmValue(string value)
        {
            return new SuccessDataResult<User>(_userService.GetByMailConfirmValue(value));
        }

    }
}
