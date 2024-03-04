using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WOT_Models;

namespace WOT_Security;

public static class TokenHandler
{
    public static async Task<TokenResponse> GenerateToken(JWTSettings _jwtKEY, Login user)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.UTF8.GetBytes(_jwtKEY.Secret);
        var _TokenExpiryTimeInMinutes= Convert.ToInt64(_jwtKEY.TokenExpiryTimeInMinutes);
        var tokendesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,user.username),
                new Claim(ClaimTypes.Role,user.username)
            }),
            Issuer = _jwtKEY.ValidIssuer,
            Audience = _jwtKEY.ValidAudience,
            Expires = DateTime.UtcNow.AddMinutes(_TokenExpiryTimeInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenhandler.CreateToken(tokendesc);
        var finaltoken = tokenhandler.WriteToken(token);
        return await Task.FromResult(new TokenResponse() { Token = finaltoken, RefreshToken = await RefreshTokenGenerator(user.username) });
    }

    public static async Task<TokenResponse> GenerateRefreshToken(JWTSettings _jwtKEY, TokenResponse token)
    {
        //get db refresh token key
        var _refreshtoken = ""; // await this.context.TblRefreshtokens.FirstOrDefaultAsync(item => item.Refreshtoken == token.RefreshToken);

        if (_refreshtoken != null)
        {
            //generate token
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(_jwtKEY.Secret);
            var _TokenExpiryTimeInMinutes = Convert.ToInt64(_jwtKEY.TokenExpiryTimeInMinutes);
            SecurityToken securityToken;
            var principal = tokenhandler.ValidateToken(token.Token, new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenkey),
                ValidateIssuer = true,
                ValidateAudience = true,

            }, out securityToken);

            var _token = securityToken as JwtSecurityToken;
            if (_token != null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                string username = principal.Identity?.Name!;

                var _existdata = ""; // await this.context.TblRefreshtokens.FirstOrDefaultAsync(item => item.Userid == username && item.Refreshtoken == token.RefreshToken);
                if (_existdata != null)
                {
                    var _newtoken = new JwtSecurityToken(
                        claims: principal.Claims.ToArray(),
                        expires: DateTime.Now.AddMinutes(_TokenExpiryTimeInMinutes),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey),
                        SecurityAlgorithms.HmacSha256)
                        );

                    var _finaltoken = tokenhandler.WriteToken(_newtoken);
                    return await Task.FromResult(new TokenResponse() { Token = _finaltoken, RefreshToken = await RefreshTokenGenerator(username) });
                }
                else
                {
                    return await Task.FromResult(TOKEN_NULL_REFERENCE());
                }
            }
            else
            {
                return await Task.FromResult(TOKEN_NULL_REFERENCE());
            }
        }
        else
        {
            return await Task.FromResult(TOKEN_NULL_REFERENCE());
        }
    }
    private static TokenResponse TOKEN_NULL_REFERENCE() => new TokenResponse() { Token = "", RefreshToken = "" };

    private static async Task<string> RefreshTokenGenerator(string username)
    {
        var RandomNo = new byte[32];
        using (var RandomNoGenerator = RandomNumberGenerator.Create())
        {
            RandomNoGenerator.GetBytes(RandomNo);
            string refreshtoken = Convert.ToBase64String(RandomNo);
            /*
            var Existtoken = this.context.TblRefreshtokens.FirstOrDefaultAsync(item => item.Userid == username).Result;
            if (Existtoken != null)
            {
                Existtoken.Refreshtoken = refreshtoken;
            }
            else
            {
                await this.context.TblRefreshtokens.AddAsync(new TblRefreshtoken
                {
                    Userid = username,
                    Tokenid = new Random().Next().ToString(),
                    Refreshtoken = refreshtoken
                });
            }
            await this.context.SaveChangesAsync();
            */

            return await Task.FromResult(refreshtoken);

        }
    }
}

