using System;
using Etherscan.Domain.Models;
using Etherscan.Infrastructure;
using Etherscan.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi
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
            services.AddCors();

            var connectionString = Configuration.GetConnectionString("EtherscanConnection");
            var mySqlVersion = new MySqlServerVersion(new Version(8, 0, 25));
            services.AddDbContext<EtherscanContext>(option =>
                option.UseMySql(connectionString, mySqlVersion));
            
            services.AddControllers();

            
            services.AddTransient<EtherscanContext>();
            services.AddTransient<IRepository<Token>, TokenRepository>();
            services.AddTransient<TokenRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}