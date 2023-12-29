using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogEngineClone.Data;
using BlogEngineClone.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Libs.Services;
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

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller = Home}/{action = Index}/{id?}"
    );

app.MapRazorPages();


app.Run();
