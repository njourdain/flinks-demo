using System;
using System.Net.Http;
using Flinks.BusinessLayer;
using Flinks.Repositories.Login;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

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
            services.AddMvc().AddJsonOptions(jo => jo.SerializerSettings.Converters.Add(new StringEnumConverter(true)));
            services.AddTransient(s => new HttpClient { BaseAddress = new Uri(Configuration["BaseFlinksUri"])});
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.Configure<MockUserOptions>(Configuration.GetSection("MockUser"));
            services.AddTransient<ILoginService, LoginService>();
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