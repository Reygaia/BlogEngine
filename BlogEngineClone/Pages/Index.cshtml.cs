using BlogEngineClone.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace BlogEngineClone.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public BlogEngineCloneUser blogger {  get; set; }
        public BlogEngineCloneUser user { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

        }

        public async Task<IActionResult> OnGet()
        {
            var user = HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userid = user.FindFirstValue(ClaimTypes.NameIdentifier);

                foreach (var claim in user.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }


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

                var token = this.getToken(authClaims);

                //var tokenline = new JwtSecurityTokenHandler().WriteToken(token);

                //Console.WriteLine(tokenline);


            }


            return Page();
        }

        public async Task<IActionResult> GetUserAsync(string userid)
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var apiEndPoint = "/api/User/Current";

            var httpclient = _httpClientFactory.CreateClient();

            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            Console.WriteLine(accesstoken);

            var apiUrl = $"{apiBaseUrl}{apiEndPoint}?userId={userid}";

            var response = await httpclient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Identity/Account/Logout");
            }

            return Page();
        }

        private JwtSecurityToken getToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(2),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

    }
}
