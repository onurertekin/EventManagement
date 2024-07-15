using DatabaseModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Operations
{
    public class AuthenticationOperations
    {
        private readonly MainDbContext mainDbContext;
        public AuthenticationOperations(MainDbContext mainDbContext)
        {
            this.mainDbContext = mainDbContext;
        }

        public string Authentication(string email, string password)
        {
            #region Validate User

            var user = mainDbContext.Participants.Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            #endregion

            var token = GenerateToken(user.Id, email);
            return token;

        }

        public string GenerateToken(int UserId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyBurayaGelecekSecretKeyBurayaGelecekSecretKeyBurayaGelecek"));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.Now.AddDays(2);

            var claims = new List<Claim>();
            claims.Add(new Claim("UserId", UserId.ToString()));
            claims.Add(new Claim("Email", email));

            var jwtSecurityToken = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        }
    }
}
