using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoftbinatorProject.Infrastructure.Data;
using SoftbinatorProject.Infrastructure.Data.Seeders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftbinatorProject.Api.Services;
using Microsoft.OpenApi.Models;
using SoftbinatorProject.Api.AuthorizationHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using SoftbinatorProject.Core.Constants;

namespace SoftbinatorProject.Api
{
    public static class ServiceExtension
    {
        public static void AddSeeders(this IServiceCollection services) {
            services.AddTransient<InitialSeeder>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserCourseService, UserCourseService>();
            services.AddScoped<IRoleApplicationService, RoleApplicationService>();
        }

        public static void AddAuthorizationHandler(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
        }

        public static void AddAzureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddMicrosoftIdentityWebApi(options => { }, options =>
             {

                 options.Instance = JObject.Parse(configuration["AzureAd"])["Instance"].ToString();
                 options.ClientId = JObject.Parse(configuration["AzureAd"])["ClientId"].ToString();
                 options.Domain = JObject.Parse(configuration["AzureAd"])["Domain"].ToString();
                 options.SignUpSignInPolicyId = JObject.Parse(configuration["AzureAd"])["SignUpSignInPolicyId"].ToString();
                 options.EditProfilePolicyId = JObject.Parse(configuration["AzureAd"])["EditProfilePolicyId"].ToString();
                 options.CallbackPath = JObject.Parse(configuration["AzureAd"])["CallbackPath"].ToString();
             });
        }

        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                options =>
                    options.UseSqlServer(
                        configuration["AzureDbConnection"]
                        , x => x.MigrationsAssembly("SoftbinatorProject.Infrastructure")
                        )
                );
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = PlatformConstants.Title, 
                    Version = "v1",
                    Description = PlatformConstants.Description,
                    Contact = new OpenApiContact
                    {
                        Name = "Grecu Cristian",
                        Email = "cristian.grecu@s.unibuc.ro",
                    }
                });
                
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://softbinatorprojeect.b2clogin.com/softbinatorprojeect.onmicrosoft.com/b2c_1_signupsignin/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri("https://softbinatorprojeect.b2clogin.com/softbinatorprojeect.onmicrosoft.com/b2c_1_signupsignin/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                //{ "https://softbinatorprojeect.onmicrosoft.com/api/demo.write", "Create Account" },
                                {"https://softbinatorprojeect.onmicrosoft.com/api/demo.read","Access Api" }
                            }
                        }
                    }
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new []{ "https://softbinatorprojeect.onmicrosoft.com/api/demo.read" }
                    }
                });

                c.IncludeXmlComments("SoftbinatorProject.Api.XML");

                c.EnableAnnotations();
            });
        }
    }
}
