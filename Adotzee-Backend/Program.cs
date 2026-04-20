
using Adotzee_Backend.Data;
using Adotzee_Backend.Mapper;
using Adotzee_Backend.Repository;
using Adotzee_Backend.Repository.AddonRepos;
using Adotzee_Backend.Repository.CollegeRepos;
using Adotzee_Backend.Repository.CoursesRepositories;
using Adotzee_Backend.Services.AddonsServices;
using Adotzee_Backend.Services.CollegeServices;
using Adotzee_Backend.Services.CourseServices;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (!builder.Environment.IsDevelopment())
            {
                builder.WebHost.ConfigureKestrel(serverOptions =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
                    serverOptions.ListenAnyIP(Int32.Parse(port));
                });
            }




            // Add services to the container.
            builder.Services.AddScoped<IAddonsService, AddonsService>();
            builder.Services.AddScoped<ICollegeService, CollegeService>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            builder.Services.AddScoped<IAddonRepository, AddonRepository>();
            builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetValue<string>("DATABASE_URL")
                                       ?? builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null));
            });

            builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);


            var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? Array.Empty<string>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
