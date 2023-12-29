using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogEngineClone.Data;
using BlogEngineClone.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Libs.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlogEngineCloneContextConnection") ?? throw new InvalidOperationException("Connection string 'BlogEngineCloneContextConnection' not found.");


builder.Services.AddDbContext<BlogEngineCloneContext>(options => options.UseSqlServer(connectionString));



//builder.Services.AddDefaultIdentity<BlogEngineCloneUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<BlogEngineCloneContext>();

builder.Services.AddIdentity<BlogEngineCloneUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<BlogEngineCloneContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<BlogEngineCloneUser, IdentityRole, BlogEngineCloneContext>>()
                .AddRoleStore<RoleStore<IdentityRole, BlogEngineCloneContext>>()
                .AddUserManager<UserManager<BlogEngineCloneUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<BlogEngineCloneUser>>();

builder.Services.AddTransient<IEmailSender,EmailSender>();

builder.Services.AddHttpClient();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
});

// Add services to the container.
builder.Services.AddRazorPages();

//builder.Services.AddAuthentication();

Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = configuration["JWT:ValidIssuer"],
        ValidAudience = configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});



var app = builder.Build();

IdentityModelEventSource.ShowPII = true;


using (var scope = app.Services.CreateScope())
{
    var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Blogger", "User" };
    foreach (var role in roles)
    {
        if (!await rolemanager.RoleExistsAsync(role))
        {
            await rolemanager.CreateAsync(new IdentityRole { Name = role });
        }
    }
}



using (var scope = app.Services.CreateScope())
{
    var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<BlogEngineCloneUser>>();

    //admin
    string email = "admin@gmail.com";
    string password = "Admin123";

    if (await usermanager.FindByEmailAsync(email) == null)
    {
        var user = new BlogEngineCloneUser();
        user.Email = email;
        user.UserName = email;
        user.name = "Admin";

        await usermanager.CreateAsync(user, password);

        await usermanager.AddToRoleAsync(user, "Admin");

        var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Using the user ID as the NameIdentifier claim
            new Claim(ClaimTypes.Name, "Admin"), // Example: User name
            new Claim(ClaimTypes.Email, "admin@gmail.com"), // Example: User email
            new Claim(ClaimTypes.Role,"Admin")
            // Add other claims as needed

            };

        // Create a ClaimsIdentity
        var identity = new ClaimsIdentity(claims, "custom", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        // Optionally, create a ClaimsPrincipal
        var principal = new ClaimsPrincipal(identity);

        await usermanager.AddClaimsAsync(user, claims);
    }

    //blogger

    string email1 = "blogger@gmail.com";
    string password1 = "blogger123";

    if (await usermanager.FindByEmailAsync(email1) == null)
    {
        var user = new BlogEngineCloneUser();
        user.Email = email1;
        user.UserName = email1;
        user.name = "Blogger";


        await usermanager.CreateAsync(user, password1);

        await usermanager.AddToRoleAsync(user, "Blogger");


        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Using the user ID as the NameIdentifier claim
            new Claim(ClaimTypes.Name, "Blogger"), // Example: User name
            new Claim(ClaimTypes.Email, "Blogger@gmail.com"), // Example: User email
            new Claim(ClaimTypes.Role,"Blogger")
        };

        // Create a ClaimsIdentity
        var identity = new ClaimsIdentity(claims, "Cookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        // Optionally, create a ClaimsPrincipal
        var principal = new ClaimsPrincipal(identity);

        await usermanager.AddClaimsAsync(user, claims);

    }

    //user
    string Useremail1 = "User1@gmail.com";
    string Userpassword1 = "User1123";

    if (await usermanager.FindByEmailAsync(Useremail1) == null)
    {
        var user = new BlogEngineCloneUser();
        user.Email = Useremail1;
        user.UserName = Userpassword1;
        user.name = "User";


        await usermanager.CreateAsync(user, Userpassword1);

        await usermanager.AddToRoleAsync(user, "User");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Using the user ID as the NameIdentifier claim
            new Claim(ClaimTypes.Name, "User"), // Example: User name
            new Claim(ClaimTypes.Email, "User@gmail.com"), // Example: User email
            new Claim(ClaimTypes.Role,"User")
        };

        // Create a ClaimsIdentity
        var identity = new ClaimsIdentity(claims, "Cookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        // Optionally, create a ClaimsPrincipal
        var principal = new ClaimsPrincipal(identity);
        await usermanager.AddClaimsAsync(user, claims);

    }
}


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();


    app.MapControllerRoute(
            name: "default",
            pattern: "{controller = Home}/{action = Index}/{id?}"
            );

    app.MapRazorPages();


    app.Run();
