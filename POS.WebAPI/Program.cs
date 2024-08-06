using NLog.Web;
using NLog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Services.UserServices;
using POS.Services.ProductServices;
using POS.API.Middleware;
using System.Text;
using POS.Data;
using POS.Repositories.UserRepository;
using POS.Repositories.ProductRepository;
using POS.Repositories.CategoryRepository;
using POS.Services.CategoryServices;
using POS.Repositories.SaleRepository;
using POS.Services.SaleServices;
using POS.WebAPI.Middleware;
using POS.AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Azure.KeyVault;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");


try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration);

    //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //    .AddJwtBearer(options =>
    //    {
    //        options.TokenValidationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ValidateIssuerSigningKey = true,
    //            ValidIssuer = "http://localhost:5082",
    //            ValidAudience = "http://localhost:5082",
    //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTcyMTc0NjE1NiwiaWF0IjoxNzIxNzQ2MTU2fQ.nKTyShgurV7Jy3yY_awDf-khRZDxMq8JpuTN0b7nGFE"))
    //        };
    //    });

    

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

    //builder.Services.AddDbContext<DBContext>(options =>
    //    options.UseInMemoryDatabase("InMemoryDb"));


    if (builder.Environment.IsDevelopment())
    {
        var keyVaultUri = builder.Configuration.GetSection("KeyVault:KeyVaultURL").Value;
        var clientId = builder.Configuration.GetSection("KeyVault:ClientId").Value;
        var clientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret").Value;
        var directoryId = builder.Configuration.GetSection("KeyVault:DirectoryId").Value;

        var credential = new ClientSecretCredential(directoryId, clientId, clientSecret);
        var secretClient = new SecretClient(new Uri(keyVaultUri.ToString()), credential);

        var endpointurl = secretClient.GetSecret("EndpointUrl");
        var primaryKey = secretClient.GetSecret("PrimaryKey");

        builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
        {
            return new CosmosClient(endpointurl.Value.Value, primaryKey.Value.Value);
        });

    }


    //builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
    //{
    //    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    //    var endpointUrl = configuration["CosmosDb:EndpointUrl"];
    //    var primaryKey = configuration["CosmosDb:PrimaryKey"];
    //    return new CosmosClient(endpointUrl, primaryKey);
    //});


    builder.Services.AddScoped<IUserRepository, UserCosmosRepository>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddScoped<IProductRepository, ProductCosmosRepository>();
    builder.Services.AddScoped<IProductManagementService, ProductManagementService>();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseMiddleware<KeyAuthenticationMiddleware>();
    //app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}


