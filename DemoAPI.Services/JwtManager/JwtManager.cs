using DemoAPI.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoAPI.Services.JwtManager
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _config;

        public JwtManager(IConfiguration config)
        {
            _config = config;
        }

        //  private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claim = new[]
                    {
                        new Claim(ClaimTypes.Name,user.first_name),
                        new Claim(ClaimTypes.Role,user.Role)
                    };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claim,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public ClaimsPrincipal GetPrincipal(string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        //        if (jwtToken == null)
        //            return null;

        //        // var symmetricKey = Convert.FromBase64String(Secret);

        //        var validationParameters = new TokenValidationParameters()
        //        {
        //            RequireExpirationTime = true,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            //  IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
        //        };

        //        SecurityToken securityToken;
        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

        //        return principal;
        //    }

        //    catch (Exception)
        //    {
        //        //should write log
        //        return null;
        //    }
        //}
    }
}