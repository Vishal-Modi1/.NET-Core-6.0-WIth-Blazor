using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Configuration;
using DataModels.Constants;
using DataModels.Enums;
using DataModels.Entities;

namespace FSMAPI.Utilities
{
    public class JWTTokenGenerator
    {
        private readonly ConfigurationSettings _configurationSettings;
        private readonly HttpContext _httpContext;

        public JWTTokenGenerator(HttpContext httpContext)
        {
            _configurationSettings = ConfigurationSettings.Instance;
            _httpContext = httpContext;
        }

        public string Generate(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaimTypes.CompanyId, user.CompanyId.ToString()),
                new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(CustomClaimTypes.RoleName, user.RoleName.ToString()),
            };

            //roles.ForEach(role =>
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //});


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationSettings.JWTKey));
            DateTime expires = DateTime.Now.AddDays(_configurationSettings.JWTExpireDays);

            //  DateTime expires = DateTime.Now.AddSeconds(15);

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configurationSettings.JWTIssuer,
            //    _configurationSettings.JWTIssuer,
            null,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetClaimValue(string claimType)
        {
            ClaimsPrincipal cp = _httpContext.User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();

            return claimValue;
        }
    }
}
