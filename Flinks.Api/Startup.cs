using System;
using System.Net.Http;
using Flinks.BusinessLayer;
using Flinks.BusinessLayer.Builders;
using Flinks.Repositories.AccountsDetail;
using Flinks.Repositories.Login;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Flinks.Api
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
            services.AddMvc()
                .AddJsonOptions(jo => jo.SerializerSettings.Converters.Add(new StringEnumConverter()))
                .AddJsonOptions(jo => jo.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddTransient(s => new HttpClient { BaseAddress = new Uri(Configuration["BaseFlinksUri"])});
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.Configure<MockUserOptions>(Configuration.GetSection("MockUser"));
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IAccountsDetailRepository, AccountsDetailRepository>();
            services.AddTransient<IOperationAccountsBuilder, OperationAccountsBuilder>();
            services.AddTransient<IUsdAccountsBuilder, UsdAccountsBuilder>();
            services.AddTransient<IBiggestCreditTrxIdBuilder, BiggestCreditTrxIdBuilder>();
            services.AddTransient<IAccountsSummaryBuilder, AccountsSummaryBuilder>();
            services.AddTransient<IAccountsSummaryService, AccountsSummaryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}