using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using APIserviciosV2.Models;

namespace APIserviciosV2.Controllers
{
    public class TokenController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate(Usuario user)
        {
            if (user == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            bool isCredentialValid = (user.Username == "admin" && user.Password == "12345");

            if (isCredentialValid)
            {
                var token = GenerateTokenJWT();
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }

        private Token GenerateTokenJWT()
        {
            var now = DateTime.Now;

            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentrials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                notBefore: now,
                expires: now.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentrials
                );

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            Token token = new Token();
            token.AccessToken = jwtTokenString;
            token.ExpiresIn = Convert.ToInt32(expireTime) * 60;

            return token;
        }

    }
}