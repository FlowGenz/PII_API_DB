using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using API_DbAccess;
using Microsoft.AspNetCore.Identity;
using api.Options;

namespace API {
    public class Startup {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly string secretKey = "eyJhbkdhgdkfs36fds4f684sdfds4fd4sdf684fd6d4s6f8ds4fd2ds468fds/fsdsfds646fds4?fsdfdfs6UcJq1B_pkpc";
        private readonly string issuer = "y/B?D(G+KbPeShVmY?fessdffsd654564dfs65/?df34dfs68f4ds?ffdds3438/rrdThWmZq4t7w9z$C&F)J@NcRfUjXn2r5u8x/A%D*G-KaPdSgVkYp3s6v9y$B&E(H+MbQeThWmZq4t7w!z%C*F-J@NcRfUjXn2r5u8x/A?D(G+KbPoQlVhMlN3UmR5Zl92Z3VnX2hmNThfNzk4LTk4Xzc4X1JjX0hrIiwiYXVkIjoiaHR0cHM6Ly93aW5lc2VydmljZS5henVyZXdlYnNpdGVzLm5Hk_Rc";
        private readonly string audience = "https://localhost:10839/";

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<PII_DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DressDatabase")));
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            /*services.AddApiVersioning(
                options => {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });*/

            //Generation token
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            services.Configure<JwtIssuerOptions>(options => {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            //connection with token
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services
                .AddAuthentication(
                options => {
                    options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, options => {
                    options.Audience = audience;
                    options.ClaimsIssuer = issuer;
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.SaveToken = true;
                });

            // Add Cross Origin Resource support
            services.AddCors(
                options => {
                    options.AddPolicy(MyAllowSpecificOrigins, builder => {
                        builder.WithOrigins("http://localhost:64342",
                                "https://wineservice.azurewebsites.net",
                                "http://localhost:4200",
                                "https://localhost:44311")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(
                c => {
                    c.SwaggerDoc("v1", new OpenApiInfo {
                        Version = "v1",
                        Title = "Wine API",
                        Description = "An API about wine in ASP.NET Core Web API"
                    });

                });
            //Identity security
            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PII_DBContext>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c => c.SerializeAsV2 = true);

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            //configuring Cross Origin Resource Sharing policy
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
