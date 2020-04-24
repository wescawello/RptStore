using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PtcERPWeb.Helpers;

namespace PtcERPWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtHelpers jwt;
        private readonly PTCERPContext _context;

        public TokenController(JwtHelpers jwt, PTCERPContext context)
        {
            this.jwt = jwt;
            this._context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<RtLoginViewModel> SignIn(LoginViewModel login)
        {
            if (ValidateUser(login))
            {
               

                return new RtLoginViewModel
                {
                    Username = login.Username,
                    Password = login.Password,




                    Token = jwt.GenerateToken(login.Username)
                };
            }
            else
            {
                return BadRequest("帳密錯誤");
            }
        }

        private bool ValidateUser(LoginViewModel login)
        {
            var user = _context.Users.FirstOrDefault(o => o.Code == login.Username);
            if (user != null)
            {
                if (DecryptUserPWDTo16(user.Password) == login.Password)
                {
                    return true;
                }
                return true;
            }
            return false;
            // TODO
        }

        [HttpGet]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }
        [HttpGet]
        public IActionResult GetProFile()
        {
            return Ok(new { Photo=""});
        }
        [HttpGet]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        [HttpGet]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }
        public static string DecryptUserPWDTo16(string strPWD)
        {
            string DescEncryptString = strPWD.ToUpper(); // 先轉換成大寫，再加密
            string TempString = string.Empty;
            // 定義ASCII Code
            for (int i = DescEncryptString.Length / 2 - 1; i >= 0; i--)
            {
                string Data;
                Data = DescEncryptString.Substring(i * 2, 2);
                int value = Convert.ToInt32(Data, 16);
                // 4.轉成Byte
                byte[] DescByte = new byte[1];
                DescByte[0] = Convert.ToByte(value);
                TempString = TempString + System.Text.Encoding.ASCII.GetString(DescByte, 0, 1);
            }
            // 回傳字串
            return TempString;
        }
    }

    public class LoginViewModel
    {
        [JsonPropertyName("username")]

        public string Username { get; set; }
        [ JsonPropertyName("password")]
        public string Password { get; set; }
    }
    public class RtLoginViewModel : LoginViewModel
    {
        [ JsonPropertyName("token")]
        public string Token { get; set; }
    }
}