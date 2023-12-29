using BlogEngineClone.Areas.Identity.Data;
using BlogEngineClone.Areas.Identity.Pages.Account;
using Libs.Entity;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        private readonly string datafilepath = "FollowData.json";
        private Dictionary<string, HashSet<string>> FollowData;

        public UserController(ILogger<UserController> logger,
                              SignInManager<BlogEngineCloneUser> signinmanager,
                              IHttpContextAccessor contextAccessor,
                              UserManager<BlogEngineCloneUser> usermanager,
                              IConfiguration configuration) 
        {
            _logger = logger;
            _signInManager = signinmanager;
            _contextAccessor = contextAccessor;
            _usermanager = usermanager;
            _configuration = configuration;

        }

        [HttpGet]
        [Route("Current")]
        public async Task<IActionResult> GetUserInfo([FromBody] LoginModel.InputModel LoginModel)
        {

            return Ok(new
            {
                message = "Success"
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
