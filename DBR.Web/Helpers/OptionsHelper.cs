using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DBR.Web.Helpers;

public static class OptionsHelper
{
	public static Action<IdentityOptions> IdentityOptions()
	{
		return options =>
		{
			options.Password.RequiredLength = 6;
			options.Password.RequireLowercase = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireDigit = false;
			options.Lockout.MaxFailedAccessAttempts = 5;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			options.User.RequireUniqueEmail = true;
			options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ1234567890- ";
		};
	}

	public static Action<AuthenticationOptions> AuthOptions()
	{
		return options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		};
	}

	public static Action<JwtBearerOptions> BearerOptions(WebApplicationBuilder builder)
	{
		return options =>
		{
			options.TokenValidationParameters.ValidateIssuerSigningKey = true;
			options.TokenValidationParameters.ValidateLifetime = false;
			options.TokenValidationParameters.ValidateAudience = true;
			options.TokenValidationParameters.ValidateIssuer = true;
			options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
			options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:Audience"];
			options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:Issuer"];
			options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
		};
	}

	public static Action<ResponseCompressionOptions> CompressionOptions()
	{
		return options =>
		{
			options.EnableForHttps = true;
			options.Providers.Add<GzipCompressionProvider>();
		};
	}

	public static Action<SwaggerGenOptions> SwaggerGenOptions()
	{
		OpenApiSecurityScheme jwtSecurityScheme = new()
		{
			BearerFormat = "JWT",
			Name = "JWT Authentication",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.Http,
			Scheme = JwtBearerDefaults.AuthenticationScheme,
			Description = "Indsæt din bearer token i nedenstående felt.",
			Reference = new OpenApiReference
			{
				Id = JwtBearerDefaults.AuthenticationScheme,
				Type = ReferenceType.SecurityScheme
			}
		};

		return options =>
		{
			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
			options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
		};
	}
}