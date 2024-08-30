using EDI.Controllers;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace EDI.Utilities.UserUtilities
{
    public class Jwt
    {
        private readonly IConfiguration _configuration;
        private readonly ErrorController _errorController;

        public Jwt(IConfiguration configuration)
        {
            _configuration = configuration;
            _errorController = new ErrorController(configuration);
        }
        public async Task<string?> JwtGenerator(List<Claim> claims, SqlConnection connection)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Secret").Value));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                    issuer: _configuration.GetSection("Jwt:Issuer").Value,
                    audience: _configuration.GetSection("Jwt:Audience").Value,
                    claims: claims,
                    expires: DateTime.Now.AddHours(8),
                    signingCredentials: credentials
                );

                string jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            catch (Exception ex)
            {
                string controllerName = this.GetType().Name;
                string FuntionName = MethodBase.GetCurrentMethod().Name;
                _errorController.Insert(controllerName, FuntionName, ex.Message, ex.StackTrace!);
                return null;
            } 
        }
    }
}
