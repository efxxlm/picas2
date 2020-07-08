using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using asivamosffie.services;
using asivamosffie.services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using asivamosffie.services.Filters;
using FluentValidation.AspNetCore;

namespace asivamosffie.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencyInjection(services);
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = appSettings.asivamosffieIssuerJwt,
                            ValidAudience = appSettings.asivamosffieAudienceJwt,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes("asivamosffie@2020application"))

                        };
                    }
                );

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
           .ConfigureApiBehaviorOptions(options =>
           {
                //options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
       

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddDbContext<model.Models.devAsiVamosFFIEContext>(options
              => options.UseSqlServer(Configuration.GetConnectionString("asivamosffieDatabase")));

          //Agregar Interfaces y clases
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IUser, UserService>();
            services.AddTransient<IAutenticacionService, AutenticacionService>();
            services.AddTransient<ICofinancingService, CofinancingService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IProjectService, ProjectService>();
            // services.AddTransient<IUnitOfWork, UnitOfWork>(); // Unidad de trabajo
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(MyAllowSpecificOrigins);
            //app.UseCors(policy => policy.WithHeaders(HeaderNames.CacheControl));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
