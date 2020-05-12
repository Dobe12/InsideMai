using System;
using InsideMai.Data;
using InsideMai.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InsideMai.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConnectDb(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<InsideMaiContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddConfiguredJwt(this IServiceCollection services, byte[] key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void AddConfiguredIdentity(this IServiceCollection services, string connection)
        {
            services.AddDbContext<InsideMaiContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequiredLength = 3;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<InsideMaiContext>();
        }
    }
}
