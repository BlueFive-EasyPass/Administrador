using Adm.Interface;
using dotenv.net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Adm.Services
{
    public class Auth : IAuth
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IDictionary<string, string> _envVariables;

        public Auth()
        {
            _envVariables = DotEnv.Read();
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_envVariables["key"]));
            _issuer = _envVariables["issuer"];
            _audience = _envVariables["audience"];
        }
        public string CreateToken(IAdministradorDTO administrador)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("Id", administrador.Id.ToString() ?? ""),
            new Claim("Email", administrador.Email ?? ""),
            new Claim("Senha", administrador.Senha ?? ""),
            new Claim("Level", administrador.Level.ToString() ?? "")
                ]),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
