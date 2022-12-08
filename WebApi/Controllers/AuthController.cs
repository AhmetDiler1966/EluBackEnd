using Business.Abstract;
using Core.Utilities.Hashing;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IForgotPasswordService _forgotPasswordService;
        public AuthController(IAuthService authService, IForgotPasswordService forgotPasswordService)
        {
            _authService = authService;
            _forgotPasswordService = forgotPasswordService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegister userForRegister)
        {
            var userExists = _authService.UserExists(userForRegister.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _authService.Register(userForRegister, userForRegister.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(userExists.Message);
        }
        [HttpPost("login")]
        public IActionResult Login(UserForLogin userForLogin)
        {
            var userToLogin = _authService.Login(userForLogin);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            if (userToLogin.Data.IsActive)
            {
                var result = _authService.CreateAccessToken(userToLogin.Data);
                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Kullanıcı Aktif değil.");
        }
        
        [HttpGet("forgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            var user = _authService.GetByEmail(email).Data;

            if (user == null)
            {
                return BadRequest("Kullanıcı Bulunamadı");
            }
            var list = _forgotPasswordService.GetListByUserId(user.Id).Data;
            foreach (var item in list)
            {
                item.IsActive = false;
                _forgotPasswordService.UpdateForgotPassword(item);
            }
            var forgotPassword = _forgotPasswordService.CreateForgotPassword(user).Data;
            var result = _authService.SendForgotPasswordEmail(user, forgotPassword.Value);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("changeforgotpassword")]
        public IActionResult ChangeForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var forgotPasswordResult = _forgotPasswordService.GetForgotPassword(forgotPasswordDto.Value);

            forgotPasswordResult.IsActive = false;
            _forgotPasswordService.UpdateForgotPassword(forgotPasswordResult);
            var userResult = _authService.GetById(forgotPasswordResult.userId).Data;
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(forgotPasswordDto.Password, out passwordHash, out passwordSalt);
            userResult.PasswordHash = passwordHash;
            userResult.PasswordSalt = passwordSalt;
            var result = _authService.ChangePassword(userResult);
            if (result.Success)
            {
                return Ok("Kullanıcı şifresi başarıyla değiştirildi.");
            }
            return BadRequest(result.Message);
        }
        [HttpGet("forgotPasswordLinkCheck")]
        public IActionResult ForgotPasswordLinkCheck(string value)
        {
            var result = _forgotPasswordService.GetForgotPassword(value);

            if (result == null)
            {
                return BadRequest("Tıkladığınız link geçersiz!");
            }
            if (result.IsActive == true)
            {
                DateTime date1 = DateTime.Now.AddHours(-1);
                DateTime date2 = DateTime.Now;
                if (result.SendDate >= date1 && result.SendDate <= date2)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Tıkladığınız link geçersiz!");
                }
            }
            else
            {
                return BadRequest("Tıkladığınız link geçersiz!");
            }
        }
        [HttpGet("sendconfirmemail")]
        public IActionResult SendConfirmEmail(string email)
        {
            var user = _authService.GetByEmail(email).Data;
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı!");
            }
            if (user.MailConfirm)
            {
                return BadRequest("Kullanıcın maili onaylı!");
            }
            var result = _authService.SendConfirmEmailAgain(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("confirmuser")]
        public IActionResult ConfirmUser(string value)
        {
            var user = _authService.GetByMailConvirmValue(value).Data;
            if (user.MailConfirm)
            {
                return BadRequest("Kullanıcı mailiniz zaten onaylı.Aynı maili tekrar onaylayamazsınız!");
            }
            user.MailConfirm = true;
            user.MailConfirmDate = DateTime.Now;
            var result = _authService.Update(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
