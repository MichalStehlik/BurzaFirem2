using BurzaFirem2.Constants;
using BurzaFirem2.Data;
using BurzaFirem2.Models;
using BurzaFirem2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net;
using System.Security.Claims;
using System.Text;
using tusdotnet;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Stores;

var builder = WebApplication.CreateBuilder(args);

// Logging to file
var sLog = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(sLog);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddOptions();
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RazorViewToStringRenderer>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("Upload"));
builder.Services.AddScoped<FileStorageManager>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(
    options => {
        options.Password.RequireUppercase = builder.Configuration["Password:Uppercase"] == "true";
        options.Password.RequireLowercase = builder.Configuration["Password:Lowercase"] == "true";
        options.SignIn.RequireConfirmedAccount = builder.Configuration["Password:ConfirmedAccount"] == "true";
        options.Password.RequireDigit = builder.Configuration["Password:Digit"] == "true";
        options.Password.RequireNonAlphanumeric = builder.Configuration["Password:NonAlphaNumeric"] == "true";
        options.Password.RequiredLength = Convert.ToInt32(builder.Configuration["Password:Length"]);
    }
    )
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Security.ADMIN_POLICY, policy => { policy.RequireClaim(Security.ADMIN_CLAIM,"1"); });
    options.AddPolicy(Security.EDITOR_POLICY, policy => { policy.RequireAssertion(x => x.User.HasClaim(Security.ADMIN_CLAIM, "1") || x.User.HasClaim(Security.EDITOR_CLAIM, "1")); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    //app.UseExceptionHandler("/api/error");
}
else
{
    //app.UseExceptionHandler("/api/error-development");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapTus("/" + builder.Configuration.GetValue<string>("Upload:Endpoint"), async httpContext => new()
{
    Store = new tusdotnet.Stores.TusDiskStore(Path.Combine(app.Environment.ContentRootPath, builder.Configuration.GetValue<string>("Upload:UploadPath"))),
    MaxAllowedUploadSizeInBytes = 100000000,
    Events = new()
    {
        OnAuthorizeAsync = eventContext =>
        {
            if (!eventContext.HttpContext.User.Identity.IsAuthenticated)
            {
                eventContext.FailRequest(HttpStatusCode.Unauthorized);
                return Task.CompletedTask;
            };
            if (!(eventContext.HttpContext.User.HasClaim("Admin", "1") || eventContext.HttpContext.User.HasClaim("Editor", "1")))
            {
                eventContext.FailRequest(HttpStatusCode.Forbidden);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        },

        OnBeforeCreateAsync = eventContext =>
        {
            if (!eventContext.Metadata.ContainsKey("contentType"))
            {
                eventContext.FailRequest("contentType metadata must be specified.");
            }
            string? contentType = eventContext.Metadata.FirstOrDefault(m => m.Key == "contentType").Value.GetString(System.Text.Encoding.UTF8);
            if (!contentType.StartsWith("image"))
            {
                eventContext.FailRequest("only images can be uploaded");
            }
            return Task.CompletedTask;
        },

        OnFileCompleteAsync = async eventContext =>
        {
            var fsm = httpContext.RequestServices.GetService<FileStorageManager>();
            ITusFile file = await eventContext.GetFileAsync();
            var image = await fsm.StoreTus(file, eventContext.CancellationToken);
        }
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
