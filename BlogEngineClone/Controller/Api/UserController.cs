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
using Azure.Identity;
using BlogEngineClone.Data;
using Microsoft.Identity.Client;

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
        private readonly BlogEngineCloneContext _dbcontext;

        private readonly string followdatafilepath = "..\\BlogEngineClone\\Areas\\Blog\\Pages\\FollowData.json";

        public bool followed;

        private Dictionary<string, HashSet<string>> FollowData;

        public UserController(ILogger<UserController> logger,
                              SignInManager<BlogEngineCloneUser> signinmanager,
                              IHttpContextAccessor contextAccessor,
                              UserManager<BlogEngineCloneUser> usermanager,
                              IConfiguration configuration,
                              IHttpClientFactory httpClientFactory,
                              BlogEngineCloneContext blogEngineCloneContext) 
        {
            _logger = logger;
            _signInManager = signinmanager;
            _contextAccessor = contextAccessor;
            _usermanager = usermanager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _dbcontext = blogEngineCloneContext;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel.InputModel Request)
        {
            var testuser = FindUserFromDBContext(Request);

            if (testuser != null)
            {
                var tokenUser = AuthenticateToken(testuser);

                var userInfo = new { UserID = testuser.Id, UserName = testuser.name, UserEmaail = testuser.Email, UserToken = tokenUser };

                var jsonResult = JsonConvert.SerializeObject(userInfo);


                //var jsonResult = JsonConvert.SerializeObject(userInfo, new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //});
                return new JsonResult(userInfo);


            }

            return BadRequest("Can't find User");
        }

        private BlogEngineCloneUser FindUserFromDBContext(LoginModel.InputModel User)
        {
            var findUser = _dbcontext.Users.FirstOrDefault(s => s.Email == User.Email);

            if (findUser != null)
            {
                return findUser;
            }
            else return null;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Current")]
        public async Task<IActionResult> GetUserInfo([FromBody] LoginModel.InputModel Request)
        {
            var context = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            BlogEngineCloneUser usertest = new BlogEngineCloneUser();

            var result = AuthenticateToken(usertest);
            
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
        private string AuthenticateToken(BlogEngineCloneUser user)
        {

            if(user != null)
            {
                var claims = GetClaims(user.Id);

                var token = GenerateToken(claims);

                return token;
            }

            //if (user)
            //{
            //    var userToken = Token(user).ToJson();



                
            //    var jsonObject = JObject.Parse(userToken);

            //    var TokenResult = jsonObject["Value"]?.ToString();

            //    return TokenResult;
            //}

            return string.Empty;
        }

        private List<Claim> GetClaims(string userid)
        {
           var userclaim =  _usermanager.FindByIdAsync(userid).Result;

           if(userclaim!=null)
           {
                var claims =  _usermanager.GetClaimsAsync(userclaim).Result;
                
                var listclaim = claims.ToList();

                return listclaim;
           }
           return new List<Claim>();
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


















        //public async Task<IActionResult> GetUserAsync(string userid)
        //{
        //    var apiBaseUrl = _configuration["ApiBaseUrl"];
        //    var apiEndPoint = "/api/User/Current";

        //    var httpclient = _httpClientFactory.CreateClient();

        //    var accesstoken = await HttpContext.GetTokenAsync("accesstoken");
        //    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
        //    Console.WriteLine(accesstoken);


        //    var apiUrl = $"{apiBaseUrl}{apiEndPoint}?userId={userid}";

        //    //var response = await httpclient.GetAsync(apiUrl);

        //    HttpContent content = new StringContent("123");

        //    var response = await httpclient.PostAsync(apiUrl, content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var usertoken = await GetUserAsync(userid);
        //        return new JsonResult(new
        //        {
        //            Token = usertoken
        //        });
        //        return RedirectToPage("/Identity/Account/Logout");
        //    }
        //    return Ok();
        //}

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
        [Route("LoadUser/{userID}")]
        public async Task<IActionResult> LoadUserWall([FromBody] string WallID)
        {
            var context =  _contextAccessor.HttpContext.User.Identity;

            var idstring = Convert.ToString(WallID);

            var list = _dbcontext.Users.ToList();

            var user = list.Where(s => s.Id == idstring).FirstOrDefault();

            var checker = await _usermanager.IsInRoleAsync(user, "Blogger");

            if (checker)
            {
                return new JsonResult(new
                {
                    userID = user.Id,
                    userName = user.UserName,
                    message = "This is a blogger"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    UserID = user.Id,
                    userName = user.UserName,
                    message = "This is not a blogger"
                });
            }
            return Ok();
        }



        [HttpPost]
        [Route("FollowBtn")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CheckFollowed(string userID, [FromBody] string targetID)
        {
            var userlist = LoadFollowData().Follow.ToList();
            var follow = FindOrCreateFollow(userID, userlist);

            var isFollowed = follow.FollowingList.Contains(targetID);
            return Ok(isFollowed);
        }

        [HttpPost]
        [Route("Follow")]
        public IActionResult FollowUser([FromBody] Follow request)
        {
            var data = LoadFollowData();

            if (IsUnfollowRequest(request))
            {
                Unfollow(request.UserID, request.FollowingID, data.Follow);
            }
            else
            {
                Follow(request.UserID, request.FollowingID, data.Follow);
            }

            SaveData(data);

            return Ok(IsUnfollowRequest(request) ? "Unfollowed successfully" : "Followed successfully");
        }
        private void Follow(string userId, string targetId, List<Follow> followList)
        {
            var userFollow = FindOrCreateFollow(userId, followList);
            var targetFollow = FindOrCreateFollow(targetId, followList);

            followList.Remove(userFollow);
            followList.Remove(targetFollow);

            userFollow.FollowingID = targetId;
            userFollow.FollowingList.Add(targetId);

            targetFollow.FollowerID = userId;
            targetFollow.FollowerList.Add(userId);

            followList.Add(userFollow);
            followList.Add(targetFollow);
        }
        private void Unfollow(string userId, string targetId, List<Follow> followList)
        {
            var userFollow = FindOrCreateFollow(userId, followList);
            var targetFollow = FindOrCreateFollow(targetId, followList);

            followList.Remove(userFollow);
            followList.Remove(targetFollow);

            userFollow.FollowingList.Remove(targetId);
            targetFollow.FollowerList.Remove(userId);

            followList.Add(userFollow);
            followList.Add(targetFollow);
        }
        private Follow FindOrCreateFollow(string userId, List<Follow> followList)
        {
            return followList.FirstOrDefault(f => f.UserID == userId) ?? new Follow { UserID = userId };
        }
        private bool IsUnfollowRequest(Follow request)
        {
            // Check if either FollowingID or FollowerID is null or empty
            return string.IsNullOrEmpty(request.FollowingID) || string.IsNullOrEmpty(request.FollowerID);
        }
        private FollowContainer LoadFollowData()
        {
            string jsondata = System.IO.File.ReadAllText(followdatafilepath);
            var Followdatajson = JsonConvert.DeserializeObject<FollowContainer>(jsondata);
            return Followdatajson;
        }
        private void SaveData(FollowContainer data)
        {
            try
            {
                var savedata = JsonConvert.SerializeObject(data);
                System.IO.File.WriteAllText(followdatafilepath, savedata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        private void SaveDataToFile()
        {
            try
            {
                string jsondata = JsonConvert.SerializeObject(FollowData, Formatting.Indented);
                System.IO.File.WriteAllText(followdatafilepath, jsondata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }


        //[HttpPost]
        //[Route("Json")]
        //public IActionResult CheckFollow([FromBody] Follow Followdata)
        //{
        //    try
        //    {
        //        if (System.IO.File.Exists(followdatafilepath))
        //        {
        //            string Listjsondata = System.IO.File.ReadAllText(followdatafilepath);

        //            var followDataJson = JsonConvert.SerializeObject(Followdata);

        //            var listfollowjson = JsonConvert.DeserializeObject<FollowContainer>(Listjsondata);

        //            var listfollow = listfollowjson.Follow;

        //            var checker = listfollow.Where(s => s.UserID == Followdata.UserID).FirstOrDefault();

        //            if (checker!=null)
        //            {
        //                listfollowjson.Follow.Remove(checker);
        //                checker = Followdata;
        //                listfollowjson.Follow.Add(checker);
        //            }


        //            var savedata = JsonConvert.SerializeObject(listfollowjson);

        //            System.IO.File.WriteAllText(followdatafilepath, savedata);

        //            return new JsonResult(new
        //            {
        //                UserID = Followdata.UserID,
        //                FollowerID = Followdata.FollowerList,
        //                FollowingID = Followdata.FollowingList,
        //            });

        //        }
        //        else
        //        {
        //            return BadRequest("Not Readable");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return BadRequest("File not Exist");
        //}












    }
}
