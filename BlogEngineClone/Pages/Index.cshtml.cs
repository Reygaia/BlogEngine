using BlogEngineClone.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Identity.Client;
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
            return Page();
        }

        //public async Task<IActionResult> GetUserAsync(string userid)
        //{
        //    var apiBaseUrl = _configuration["ApiBaseUrl"];
        //    var apiEndPoint = "/api/User/Current";

        //    var httpclient = _httpClientFactory.cli;

        //    var accesstoken = await HttpContext.GetTokenAsync("accesstoken");
        //    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
        //    Console.WriteLine(accesstoken);

        //    var apiUrl = $"{apiBaseUrl}{apiEndPoint}?userId={userid}";

        //    //var response = await httpclient.GetAsync(apiUrl);

        //    HttpContent content = new StringContent("123");

        //    var response = await httpclient.PostAsync(apiUrl,content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var usertoken = await GetUserAsync(userid);
        //        return new JsonResult(new
        //        {
        //            Token = usertoken
        //        });
        //        return RedirectToPage("/Identity/Account/Logout");
        //    }

        //    return Page();
        //}

      


    }
}



