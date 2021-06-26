using System;
using System.Linq;
using System.Text;
using JWTSample.DataContext;
using JWTSample.Interface;
using JWTSample.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace JWTSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                // In memory Database
                services.AddDbContext<SampleDbContext>(options => options.UseInMemoryDatabase(databaseName: "Sample"));

                services.AddScoped<ILoginService, LoginService>();
                services.AddScoped<IUserService, UserService>();

                // JWT Token Validation
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    var aud = Configuration.GetSection("JWTTokenConfig:Aud").Value;
                    var iss = Configuration.GetSection("JWTTokenConfig:Iss").Value;
                    var secretKey = Encoding.ASCII.GetBytes(Configuration.GetSection("JWTTokenConfig:Secret").Value);
                    var signingKey = new SymmetricSecurityKey(secretKey);

                    var tokenValidationParam = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = iss,
                        ValidAudience = aud,
                        IssuerSigningKey = signingKey,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    opt.RequireHttpsMetadata = false;                    
                    opt.TokenValidationParameters = tokenValidationParam;
                });

                // Swagger for Api Documentation
                services.AddSwaggerGen(s =>
                {
                    s.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "1.0",
                        Title = "JWT Sample APi",
                        Description = "JWT Authentication APi Demo"
                    });

                    s.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Please enter 'Bearer' followed by JWT Token value",
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });

                    s.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey,
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                Description = "Please enter 'Bearer' followed by JWT Token value",
                                Scheme = "oauth2",
                                Reference = new OpenApiReference
                                {
                                    Id = JwtBearerDefaults.AuthenticationScheme,
                                    Type = ReferenceType.SecurityScheme
                                }
                            }, Enumerable.Empty<string>().ToList()
                        }
                    });
                });

                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger().UseSwaggerUI();

            app.UseMvc();
        }
    }
}
