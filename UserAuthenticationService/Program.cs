using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserAuthenticationService;
using UserAuthenticationService.Core.Hubs;
using UserAuthenticationService.Core.Repositories;
using UserAuthenticationService.Core.Services.Authentication;
using UserAuthenticationService.Core.Services.MatchesServices;
using UserAuthenticationService.Core.Services.RefreshTokenServices;
using UserAuthenticationService.Core.Services.UserClaims;
using UserAuthenticationService.Core.Services.UserServices;
using UserAuthenticationService.Data.Data;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Environment.GetEnvironmentVariable("KEY");
Console.WriteLine($"--> Key is {key}");
var tokenValidationParams = new TokenValidationParameters
{
    ClockSkew = TimeSpan.Zero,
    ValidateIssuer = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidateAudience = false,
    ValidIssuer =  jwtSettings.GetSection("Issuer").Value,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
};

// Add services to the container.
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMatchesServices, MatchesServices>();
builder.Services.AddScoped<IUserClaims, UserClaims>();
builder.Services.AddScoped<IRefreshToken, RefreshTokenServices>();
builder.Services.AddSingleton(tokenValidationParams);
builder.Services.ConfigureJwt(tokenValidationParams);
builder.Services.AddAuthentication();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureIdentityOptions();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAutoMapper();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection(op);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
