
using JODDBTask.Api.Configrations;
using JODDBTask.Core.Data;
using JODDBTask.Core.Helpers;
using JODDBTask.Core.IReposetory;
using JODDBTask.Core.IServieces;
using JODDBTask.Infra.Helpers;
using JODDBTask.Infra.Reposetory;
using JODDBTask.Infra.Servieces;
using Jose;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using JwtSettings = JODDBTask.Core.Helpers.JwtSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;


namespace JODDBTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var ConnectionString = builder.Configuration.GetConnectionString("JODDBTaskDbConnectionString");
            if (ConnectionString == null)
            {
                Console.WriteLine("Connection string not found.");
            }
            else
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        Console.WriteLine("Connection opened successfully.");

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while trying to open the connection: " + ex.Message);
                    }
                }

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(ConnectionString));

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAllOrigins",
                        policy => policy.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader());
                });

                builder.Services.AddControllers();

                builder.Services.AddAutoMapper(typeof(MapperConfig));

                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IUserServieces, UserServieces>();

                builder.Services.AddScoped<IJwtHelper, JwtHelper>();

                builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
                builder.Services.Configure<JwtSettings>(jwtSettingsSection);


                builder.Services.AddScoped<IExcelImporterService, ExcelImporterService>();
                builder.Services.AddScoped<IExcelDataRepository>(provider =>
                    new ExcelDataRepository(ConnectionString));

                builder.Services.Configure<KestrelServerOptions>(options =>
                {
                    options.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
                });

                var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseCors("AllowAllOrigins");

                app.UseAuthentication();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
        }
      
    }
}
