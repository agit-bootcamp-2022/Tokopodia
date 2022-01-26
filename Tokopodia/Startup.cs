using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Helpers;
using Tokopodia.Graphql;
using Tokopodia.GraphQL.Queries;
using Tokopodia.GraphQL;
using Tokopodia.GraphQL.Mutations;
using Tokopodia.Helper;
using Tokopodia.Data.User;

namespace Tokopodia
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {

      Configuration = configuration;
      _env = env;
    }
    private readonly IWebHostEnvironment _env;
    public IConfiguration Configuration { get; }

    [Obsolete]
    public void ConfigureServices(IServiceCollection services)
    {
      if (_env.IsProduction())
      {
        Console.WriteLine("--> using Azure Db");
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
          Configuration.GetConnectionString("Connection")
        ));
      }
      else
      {
        Console.WriteLine("--> Using LocalDB");
        services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase")));
      }

      services.AddTransient<DbInitializer>();

      services
        .AddGraphQLServer()
        .AddAuthorization()
        .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
           .AddQueryType(d => d.Name("Query"))
               .AddTypeExtension<Query>()
               .AddTypeExtension<QueryProduct>()
               .AddTypeExtension<BuyerProfileQuery>()
               .AddTypeExtension<SellerProfileQuery>()
           .AddMutationType(d => d.Name("Mutation"))
               .AddTypeExtension<Mutation>()
               .AddTypeExtension<MutationProduct>()
               .AddTypeExtension<BuyerProfileMutation>()
               .AddTypeExtension<SellerProfileMutation>();

      services.AddHttpContextAccessor();
      services.AddErrorFilter<GraphQLErrorFilter>();
      services.AddScoped<IUser, UserDAL>();
      services.AddScoped<UserManager<IdentityUser>>();
      services.AddControllers();

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
          options.Password.RequiredLength = 8;
          options.Password.RequireLowercase = true;
          options.Password.RequireUppercase = true;
          options.Password.RequireNonAlphanumeric = true;
          options.Password.RequireDigit = true;
        })
        .AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();


      var appSettingSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingSection);
      var appSettings = appSettingSection.Get<AppSettings>();
      var key = Encoding.ASCII.GetBytes(appSettings.Secret);

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
                          IWebHostEnvironment env,
                          DbInitializer seeder,
                          UserManager<IdentityUser> userManager)
    {
      seeder.Initialize(userManager).Wait();
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGraphQL();
      });
    }
  }
}
