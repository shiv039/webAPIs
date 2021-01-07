using System;
using System.Collections.Generic;
using CKEditor_TokenEndPoint.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Configuration;
using System.Web.Http.Cors;


namespace CKEditor_TokenEndPoint.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenController : ApiController
    {
        static readonly string environmentId = ConfigurationManager.AppSettings["EnvironmentID"];
        static readonly string accessKey = ConfigurationManager.AppSettings["AccessKey"];

        [ActionName("Token")]

        [HttpGet]
        public string createCSToken(string Subject, string name, string role, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(accessKey));

            var signingCredentials = new SigningCredentials(securityKey, "HS256");
            var header = new JwtHeader(signingCredentials);

            var dateTimeOffset = new DateTimeOffset(DateTime.UtcNow);

            var payload = new JwtPayload
            {
                { "aud", environmentId },
                { "iat", dateTimeOffset.ToUnixTimeSeconds() },
                { "sub", Subject },
                { "user", new Dictionary<string, string> {
                    { "email", email },
                    { "name", name }
                } },
                { "auth", new Dictionary<string, object> {
                    { "collaboration", new Dictionary<string, object> {
                        { "*", new Dictionary<string, string> {
                            { "role", role }
                        } }
                    } }
                } }
            };

            var securityToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(securityToken);
        }
    }
}
