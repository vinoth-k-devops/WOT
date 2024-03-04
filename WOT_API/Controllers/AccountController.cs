using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WOT_Common;
using WOT_Models;
using WOT_Security;

namespace WOT_API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly JWTSettings _jwtSettings;
        public AccountController(IOptions<JWTSettings> options)
        {
            _jwtSettings = options.Value;
        }
        [Authorize]
        [HttpGet("Encryption/{PlainText}")]
        public async Task<string> Encryption(string PlainText)
        {
            return await CryptoFunctions.Encryption(PlainText);
        }
        [Authorize]
        [HttpGet()]
        public async Task<string> GETDBID()
        {
            return await Task.FromResult(Request.Headers["Authorization"].ToString());
        }
        [HttpGet("DBConnection/{ConnId}")]
        public async Task<string> DBConnection(string ConnId)
        {
            return await Task.FromResult(WOTCSModel.GetConnectionString(ConnId));
        }
        [HttpGet("Decryption/{CipherText}")]
        public async Task<string> Decryption(string CipherText)
        {
            return await CryptoFunctions.Decryption(CipherText);
        }
        [HttpGet("MD5/{PlainText}")]
        public async Task<string> MD5(string PlainText)
        {
            return await CryptoFunctions.CreateMD5(PlainText);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login userCred)
        {
            var re = Request.Headers;
            

            if (userCred.username == "admin" && (await CryptoFunctions.CreateMD5(userCred.password) == "21232F297A57A5A743894A0E4A801FC3"))
            {
                var token = await TokenHandler.GenerateToken(_jwtSettings, userCred);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}

