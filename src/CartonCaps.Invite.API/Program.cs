using CartonCaps.Invite.API.Authorization;
using CartonCaps.Invite.API.Filters;
using CartonCaps.Invite.API.Infrastructure;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.API.Services;
using CartonCaps.Invite.Data;
using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddScoped<UnhandledExceptionFilter>();
builder.Services.AddScoped<ResourceNotFoundExceptionFilter>();
builder.Services.AddScoped<ReferralSourceNotFoundExceptionFilter>();
builder.Services.AddScoped<UnauthorizedAccessExceptionFilter>();
builder.Services.AddScoped<ArgumentExceptionFilter>();
builder.Services.AddScoped<UniqueRedemptionExceptionFilter>();
builder.Services.AddScoped<ReferralCodeNotFoundExceptionFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UnhandledExceptionFilter>();
    options.Filters.Add<ResourceNotFoundExceptionFilter>();
    options.Filters.Add<ReferralSourceNotFoundExceptionFilter>();
    options.Filters.Add<UnauthorizedAccessExceptionFilter>();
    options.Filters.Add<ArgumentExceptionFilter>();
    options.Filters.Add<UniqueRedemptionExceptionFilter>();
    options.Filters.Add<ReferralCodeNotFoundExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Invite API", Version = "v1" });
    c.SchemaFilter<DtoJsonStringEnumSchemaFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<InviteContext>(options => options
.UseSqlite("Data Source=invite.db")
.UseLazyLoadingProxies()
);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IRedemptionsService, RedemptionsService>();
builder.Services.AddScoped<IReferralsService, ReferralsService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IReferralSourcesRepository, ReferralSourcesRepository>();
builder.Services.AddScoped<IReferralsRepository, ReferralsRepository>();
builder.Services.AddScoped<IRedemptionsRepository, RedemptionsRepository>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddSingleton<IHttpClientFactory, FakeHttpClientFactory>();
builder.Services.AddSingleton<IProfileManagementApiClient, ProfileManagementApiClient>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = "sub";
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("carton-caps-secret-for-code-challenge")),
            ValidateIssuer = true,
            ValidIssuer = "https://fake-auth0.local/",
            ValidateAudience = true,
            ValidAudience = "https://fake-api/",
            ValidateLifetime = true,
            NameClaimType = "sub"
        };
        options.MapInboundClaims = false;
    });



builder.Services.AddAuthorizationBuilder()
    .AddPolicy("OwnerOnly", policy =>
        policy.Requirements.Add(new OwnershipRequirement()));

builder.Services.AddScoped<IAuthorizationHandler, OwnershipHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InviteContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.MapScalarApiReference(options =>
{
    options.Servers = Array.Empty<ScalarServer>();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
