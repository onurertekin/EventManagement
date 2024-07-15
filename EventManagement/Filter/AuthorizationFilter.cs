using DomainService.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Host.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class AuthorizableAttribute : TypeFilterAttribute
    {

        public AuthorizableAttribute() : base(typeof(AuthorizableFilter)) { }
    }

    public class AuthorizableFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Token"].FirstOrDefault();
            if (token == null || token == "null")
                throw new BusinessException(401, "TokenRequired");

            #region Validate Token

            //burada token bizim verdiğimiz bir token mı?
            bool isValid = ValidateToken(token);
            if (!isValid)
                throw new BusinessException(401, "InvalidToken");

            #endregion

            #region Example Read Token

            //var payload = GetTokenPayload(token);
            //var personId = Convert.ToInt32(payload["PersonId"]);

            #endregion
        }

        private bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SecretKeyBurayaGelecekSecretKeyBurayaGelecekSecretKeyBurayaGelecek");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private JwtPayload GetTokenPayload(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            return jwtSecurityToken.Payload;
        }
    }
}
