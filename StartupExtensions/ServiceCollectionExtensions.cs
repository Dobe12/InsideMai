using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsideMaiWebApi.Data;
using InsideMaiWebApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InsideMaiWebApi.StartupExtensions
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
                    options.Password.RequiredLength = 9;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<InsideMaiContext>();
        }
    }
}
