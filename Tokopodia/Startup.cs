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
using System.Text;
using Tokopodia.Data;
using Tokopodia.Helpers;
using Tokopodia.Graphql;
using Tokopodia.GraphQL.Queries;
using Tokopodia.GraphQL;
using Tokopodia.GraphQL.Mutations;

namespace Tokopodia
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {

      Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase")));

      services.AddTransient<DbInitializer>();

      services
        .AddGraphQLServer()
        .AddAuthorization()
           .AddQueryType(d => d.Name("Query"))
               .AddTypeExtension<Query>()
               .AddTypeExtension<ProductQuery>()
               .AddTypeExtension<QueryCart>()
               .AddTypeExtension<BuyerProfileQuery>()
               .AddTypeExtension<SellerProfileQuery>()
           .AddMutationType(d => d.Name("Mutation"))
               .AddTypeExtension<Mutation>()
               .AddTypeExtension<ProductMutation>()
               .AddTypeExtension<BuyerProfileMutation>()
               .AddTypeExtension<SellerProfileMutation>();

            services.AddHttpContextAccessor();


      services.AddControllers();
          
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
          
      services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
          options.Password.RequiredLength = 8;
          options.Password.RequireLowercase = true;
          options.Password.RequireUppercase = true;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireDigit = false;
        }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();


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
