using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ExamSystemAPI.Helper
{
    public class ClaimHelper
    {
        private readonly UserManager<User> userManager;

        public ClaimHelper(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// 生成claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Claim>> Model2ClaimsAsync(User user) {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            if(user.UserName != null)
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim("JWTVersion", user.JWTVersion.ToString()));
            return claims;
        }
    }
}
