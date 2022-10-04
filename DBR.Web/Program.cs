using System.IO.Compression;
using DBR.Core.Domain;
using DBR.Infrastructure.Context;
using DBR.Web.CompositionRoot;
using DBR.Web.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddDataProtection().SetApplicationName("DBR").PersistKeysToFileSystem(new DirectoryInfo(isDevelopment ? @"e:\dataprotectionkeys" : @"d:\web\localuser\faluf.com\DataProtectionKeys"));
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<DBRContext>(OptionsHelper.ContextOptionsBuilder(builder));
builder.Services.AddIdentity<Member, IdentityRole<Guid>>(OptionsHelper.IdentityOptions()).AddEntityFrameworkStores<DBRContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(OptionsHelper.AuthOptions()).AddJwtBearer(OptionsHelper.BearerOptions(builder));

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.AddResponseCompression(OptionsHelper.CompressionOptions());

builder.Services.ConfigureDBRServices();
builder.Services.AddHttpClient();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<ProtectedLocalStorage, ProtectedLocalStorage>();
builder.Services.AddScoped<ProtectedSessionStorage, ProtectedSessionStorage>();

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(OptionsHelper.SwaggerGenOptions());

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
	endpoint.MapControllers();
	endpoint.MapBlazorHub();
	endpoint.MapFallbackToPage("/_Host");
});

app.Run();