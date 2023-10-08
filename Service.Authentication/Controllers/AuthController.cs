using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Authentication.Request;
using System.Text;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Service.Authentication.Service;

namespace Service.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        string otherServiceUrl = "http://localhost:5287/api/User";


        private readonly EmailProducer _emailProducer;
        public AuthController(EmailProducer emailProducer)
        {
            _emailProducer = emailProducer;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRequest request)
        {
            using (var httpClient = new HttpClient())
            {

                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(otherServiceUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    EmailRequest emailRequest = new EmailRequest
                    {
                        ToEmail = request.Email,
                        Subject = $"Welcome to Our Website",
                        Message = $"Hello {request.FullName},\\n\\nThank you for registering on our website!"
                    };
                    _emailProducer.SendEmailRequest(emailRequest);
                    return Ok(request);
                }
                else
                {
                    return BadRequest("Something went wrong");
                }
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            using (var httpClient = new HttpClient())
            {

                string requestUrl = $"{otherServiceUrl}?email={request.Email}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<UserRequest>(await response.Content.ReadAsStringAsync());
                    if (user is not null)
                    {
                        var verify = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
                        if (verify)
                        {
                            string token = CreateToken(user);
                            return Ok(token);
                        }
                    }
                }
            }

            return BadRequest("Incorrect email or password");
        }

        private string CreateToken(UserRequest user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asfsafsasafjsafjksafksafsafsafsafasfasfafasfsafasfsafsafassaf"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds,
                issuer: "asd"
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
