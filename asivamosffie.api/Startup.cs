using asivamosffie.services;
using asivamosffie.services.Filters;
using asivamosffie.services.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.IO;
using asivamosffie.api.Helpers;
using asivamosffie.api.Controllers;

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
            services.Configure<asivamosffie.model.AditionalModels.MailSettings>(appSettingsSection);
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
                            ClockSkew = TimeSpan.Zero,
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
               options.SuppressModelStateInvalidFilter = true;
           });

            #region A gregado pora implementacion de descargas de PDF
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            #endregion


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
         
        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddDbContext<model.Models.devAsiVamosFFIEContext>(options
              => options.UseSqlServer(Configuration.GetConnectionString("asivamosffieDatabase")));
            //Agregar Interfaces y clases
            services.AddTransient<IRegisterContractualLiquidationRequestService, RegisterContractualLiquidationRequestService>();
            services.AddTransient<IParametricService, ParametricService>();
            services.AddTransient<ICreateRolesService, CreateRolesService>();
            services.AddTransient<IRegisterValidateSpinOrderService, RegisterValidateSpinOrderService>();
            services.AddTransient<IRegisterProjectETCService, RegisterProjectETCService>();
            services.AddTransient<IValidateFulfilmentFinalReportService, ValidateFulfilmentFinalReportService>();
            services.AddTransient<IGenerateSpinOrderService, GenerateSpinOrderService>();
            services.AddTransient<IPaymentRequierementsService, PaymentRequierementsService>();
            services.AddTransient<IRegisterValidatePaymentRequierementsService, RegisterValidatePaymentRequierementsService>();
            services.AddTransient<IManageCheckListService, ManageCheckListService>();
            services.AddTransient<IValidateFinalReportService, ValidateFinalReportService>();
            services.AddTransient<IVerifyFinalReportService, VerifyFinalReportService>();
            services.AddTransient<IRegisterFinalReportService, RegisterFinalReportService>();
            services.AddTransient<IValidateWeeklyProgressService, ValidateWeeklyProgressService>();
            services.AddTransient<ICheckWeeklyProgressService, CheckWeeklyProgressService>();
            services.AddTransient<IRegisterWeeklyProgressService, RegisterWeeklyProgressService>();
            services.AddTransient<IRegisterPersonalProgrammingService, RegisterPersonalProgrammingService>();
            services.AddTransient<IApprovePreConstructionPhase1Service, ApprovePreConstructionPhase1Service>();
            services.AddTransient<IRegisterPreContructionPhase1Service, RegisterPreContructionPhase1Service>();
            services.AddTransient<IRegisterContractsAndContractualModificationsService, RegisterContractsAndContractualModificationsService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<IUser, UserService>();
            services.AddTransient<IAutenticacionService, AutenticacionService>();
            services.AddTransient<ICofinancingService, CofinancingService>();
            services.AddTransient<IContributorService, ContributorService>();
            services.AddTransient<ISourceFundingService, SourceFundingService>();
            services.AddTransient<ICommitteeSessionFiduciarioService, CommitteeSessionFiduciarioService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICofinancingContributorService, CofinancingContributorService>();
            services.AddTransient<IBankAccountService, BankAccountService>();
            services.AddTransient<IProjectContractingService, ProjectContractingService>();
            services.AddTransient<ISelectionProcessService, SelectionProcessService>();
            services.AddTransient<ISelectionProcessScheduleService, SelectionProcessScheduleService>();
            services.AddTransient<IGuaranteePolicyService, GuaranteePolicyService>();
            services.AddTransient<IResourceControlService, ResourceControlService>();
            services.AddTransient<IBudgetAvailabilityService, BudgetAvailabilityService>();
            services.AddTransient<IRequestBudgetAvailabilityService, RequestBudgetAvailabilityService>();
            services.AddTransient<IManageContractualProcessesService, ManageContractualProcessesService>();
            services.AddTransient<IManagementCommitteeReportService, ManagementCommitteeReportService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IDailyFollowUpService, DailyFollowUpService>();
            services.AddTransient<IManagementCommitteeReportService, ManagementCommitteeReportService>();
            services.AddTransient<IContractualControversy, ContractualControversyService>();
            services.AddTransient<IRegisterSessionTechnicalCommitteeService, RegisterSessionTechnicalCommitteeService>();
            services.AddTransient<IRegisterContractsAndContractualModificationsService, RegisterContractsAndContractualModificationsService>();
            services.AddTransient<IManagePreContructionActPhase1Service, ManagePreContructionActPhase1Service>();
            services.AddTransient<IVerifyPreConstructionRequirementsPhase1Service, VerifyPreConstructionRequirementsPhase1Service>();
            services.AddTransient<IJudicialDefense, JudicialDefenseService>();
            services.AddTransient<IRegisterSessionTechnicalCommitteeService, RegisterSessionTechnicalCommitteeService>();
            services.AddTransient<IManageContractualProcessesService, ManageContractualProcessesService>();
            services.AddTransient<ITechnicalRequirementsConstructionPhaseService, TechnicalRequirementsConstructionPhaseService>();
            services.AddTransient<ITechnicalCheckConstructionPhase2Service, TechnicalCheckConstructionPhase2Service>();
            services.AddTransient<IActBeginService, ActBeginService>();
            services.AddTransient<IRegisterPayPerformanceService, RegisterPayPerformanceService>();
            services.AddTransient<IResourceControlService, ResourceControlService>();
            services.AddTransient<IContractualNoveltyService, ContractualNoveltyService>();
            services.AddTransient<IDerivativeActionService, DerivativeActionService>();
            services.AddTransient<IMonitoringURL, MonitoringURLService>();

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
