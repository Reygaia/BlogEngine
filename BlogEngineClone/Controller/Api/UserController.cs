using BlogEngineClone.Areas.Identity.Data;
using BlogEngineClone.Areas.Identity.Pages.Account;
using Libs.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NuGet.Protocol;
using Newtonsoft.Json.Linq;

namespace BlogEngineClone.Controller.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<BlogEngineCloneUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<BlogEngineCloneUser> _usermanager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string datafilepath = "FollowData.json";
        private Dictionary<string, HashSet<string>> FollowData;

        public UserController(ILogger<UserController> logger,
                              SignInManager<BlogEngineCloneUser> signinmanager,
                              IHttpContextAccessor contextAccessor,
                              UserManager<BlogEngineCloneUser> usermanager,
                              IConfiguration configuration,
                              IHttpClientFactory httpClientFactory) 
        {
            _logger = logger;
            _signInManager = signinmanager;
            _contextAccessor = contextAccessor;
            _usermanager = usermanager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Current")]
        public async Task<IActionResult> GetUserInfo([FromBody] LoginModel.InputModel Request)
        {
            var context = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var result = AuthenticateToken();

            var user = await _usermanager.FindByEmailAsync(Request.Email);

            if(!string.IsNullOrEmpty(result))
            {
                var UserResponse = new { UserID = user.Id, UserName = user.name, Token = result };
                
                var jsonUser = JsonConvert.SerializeObject(UserResponse);

                return new JsonResult(UserResponse);
            }

            int x = 1;

            return Ok();
            
        }
        private string AuthenticateToken()
        {
            var user = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            if (user.IsAuthenticated)
            {
                var userToken = Token(user).ToJson();
                
                var jsonObject = JObject.Parse(userToken);

                var TokenResult = jsonObject["Value"]?.ToString();

                return TokenResult;
            }

            return string.Empty;
        }


        private IActionResult Token(ClaimsIdentity user)
        {
            var usernameidentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            var userrole = user.FindFirst(ClaimTypes.Role)?.Value;
            var useremail = user.FindFirst(ClaimTypes.Email)?.Value;

            var authClaims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, usernameidentifier),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, userrole),
            new Claim(ClaimTypes.Email, useremail),
            };

            var token = GenerateToken(authClaims);

            return Ok(token);

        }

        private string GenerateToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(48)),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<IActionResult> GetUserAsync(string userid)
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var apiEndPoint = "/api/User/Current";

            var httpclient = _httpClientFactory.CreateClient();

            var accesstoken = await HttpContext.GetTokenAsync("accesstoken");
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            Console.WriteLine(accesstoken);


            var apiUrl = $"{apiBaseUrl}{apiEndPoint}?userId={userid}";

            //var response = await httpclient.GetAsync(apiUrl);

            HttpContent content = new StringContent("123");

            var response = await httpclient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var usertoken = await GetUserAsync(userid);
                return new JsonResult(new
                {
                    Token = usertoken
                });
                return RedirectToPage("/Identity/Account/Logout");
            }
            return Ok();
        }

        //[HttpGet]
        //[Route("Protected")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Protected()
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                UserID = userID,
                UserName = username,
            });
        }

      









        [HttpPost]
        [Route("Follow")]
        public IActionResult FollowUser([FromBody] Follow request)
        {
            if (request == null
                || string.IsNullOrEmpty(request.UserID)
                || string.IsNullOrEmpty(request.FollowerID)
                || string.IsNullOrEmpty(request.FollowerID))
            {
                return BadRequest(new { message = "Invalid request" });
            }

            if (!FollowData.ContainsKey(request.FollowerID))
            {

                FollowData[request.FollowerID] = new HashSet<string>();
            }

            FollowData[request.FollowerID].Add(request.FollowerID);

            SaveDataToFile();

            return Ok(new
            {
                message = $"{request.FollowerID} is now following {request.FollowingID}"
            });
        }

        [HttpPost]
        [Route("Unfollow")]
        public IActionResult UnFollowUser([FromBody] Follow request)
        {
            return Ok(new
            {
                message = "success"
            });
        }


        private void LoadDataFromFile()
        {
            try
            {
                if (System.IO.File.Exists(datafilepath))
                {
                    string jsondata = System.IO.File.ReadAllText(datafilepath);
                    FollowData = JsonConvert.DeserializeObject<Dictionary<string, HashSet<string>>>(jsondata);
                }
                else
                {
                    FollowData = new Dictionary<string, HashSet<string>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from file:{ex.Message}");
                FollowData = new Dictionary<string, HashSet<string>>();
            }
        }


        private void SaveDataToFile()
        {
            try
            {
                string jsondata = JsonConvert.SerializeObject(FollowData, Formatting.Indented);
                System.IO.File.WriteAllText(datafilepath, jsondata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        












    }
}
