using AuthenticationApi.Business.Services;
using AuthenticationApi.Common.Contracts.Data;
using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Data;
using AuthenticationApi.Data.Repositories;
using AuthenticationApi.Model.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration configuration = new ConfigurationManager();
IConfigurationService configurationService = new ConfigurationService(configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configurationService.MainConnection));

// Add services to the container.
builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();
builder.Services.AddTransient<ILoginProviderRepository, LoginProviderRepository>();
builder.Services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<IDataContext, ApplicationDbContext>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IConfigurationService, ConfigurationService>();
builder.Services.AddTransient<IDatabaseConfigurationService, DatabaseConfigurationService>();
builder.Services.AddTransient<ICryptoService, CryptoService>();
builder.Services.AddTransient<ITemplateService, TemplateService>();
builder.Services.AddTransient<IMailService, MailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    
}

app.UseRouting();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

app.UseEndpoints(x => x.MapControllers());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
