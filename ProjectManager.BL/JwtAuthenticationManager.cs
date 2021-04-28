using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using project_manager.common.Exceptions;
using project_manager.common.Models;
using project_manager.contract;
using project_manager.data.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace project_manager.bl
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private string _key;
        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }
        public string Authenticate(string username, string password)
        {
            try
            {
                using (ApplicationDBContext _context = new ApplicationDBContext())
                {
                    var user = _context.User
                        .Include(x => x.Roles)
                        .ThenInclude(x => x.Role)
                        .Where(x => x.UserName.ToLower() == username.Trim().ToLower() && x.Password == password.Trim()).FirstOrDefault();


                    if (user != null)
                    {
                        if (!user.IsActive)
                        {
                            throw new ServerException(403, "You aren't able to login");
                        }
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenKey = Encoding.ASCII.GetBytes(_key);
                        var claims = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email),
                            });
                        foreach (var rol in user.Roles)
                        {
                            claims.AddClaim(new Claim(ClaimTypes.Role, rol.Role.Name));
                        }
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = claims,
                            Expires = DateTime.UtcNow.AddHours(1),
                            SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(tokenKey),
                                SecurityAlgorithms.HmacSha256Signature
                                )
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        return tokenHandler.WriteToken(token);
                    }
                    else
                    {
                        throw new ServerException(401, "Incorrect credentials");
                    }
                }
            }
            catch 
            {
                throw;
            }
        }
    }
}
