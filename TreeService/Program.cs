
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TreeService.Auth;
using TreeService.Data;
using TreeService.Repositories;
using TreeService.Services;

namespace TreeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("Jwt"));

            builder.Services.AddScoped<ITreeNodeRepository, TreeNodeRepository>();
            builder.Services.AddScoped<ITreeNodeService, TreeNodeService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
