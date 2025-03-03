using ExamSystemAPI.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExamSystemAPI.Helper
{
    public class JWTHelper
    {
        private readonly IOptionsSnapshot<JWTSettings> jwtSettingsOpt;
        private readonly string key;

        public JWTHelper(IOptionsSnapshot<JWTSettings> jwtSettingsOpt)
        {
            this.jwtSettingsOpt = jwtSettingsOpt;
            key = jwtSettingsOpt.Value.SecKey;
        }

        /// <summary>
        /// 生成jwt
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateJWT(List<Claim> claims) {
            DateTime expires = DateTime.Now.AddSeconds(jwtSettingsOpt.Value.ExpireSeconds);
            byte[] secBytes = Encoding.UTF8.GetBytes(key);
            var secKey = new SymmetricSecurityKey(secBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(claims: claims,
                expires: expires, signingCredentials: credentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }
    }
}
