using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidTime.Api.GRPCServices;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Auth;
using RapidTime.Core.Repositories;
using RapidTime.Data;
using RapidTime.Services;

namespace RapidTime.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddDbContext<RapidTimeDbContext>(opts =>
                opts.UseNpgsql(
                    Configuration.GetConnectionString("Default"),
                    x => x.MigrationsAssembly("RapidTime.Data")));
            
            services.AddScoped<RapidTimeDbContext>();
            // User changed to full path to avoid conflicts with generated classes from GRPC
            services.AddIdentity<RapidTime.Core.Models.Auth.User, Role>()
                .AddEntityFrameworkStores<RapidTimeDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUnitofWork, UnitofWork>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<IAddressAggregateService, AddressAggregateService>();
            services.AddTransient<IAssignmentTypeService, AssignmentTypeService>();
            services.AddTransient<IAssignmentService, AssignmentService>();
            services.AddTransient<ICompanyTypeService, CompanyTypeService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICustomerContactService, CustomerContactService>();
            services.AddTransient<IPriceService, PriceService>();
            services.AddTransient<IRepository<BaseEntity>, Repository<BaseEntity>>();
            services.AddTransient<IAssignmentRepository, AssignmentRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ITimeRegistrationService, TimeRegistrationService>();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AssignmentTypeGrpcService>();
                endpoints.MapGrpcService<AssignmentGrpcService>();
                endpoints.MapGrpcService<CityGrpcService>();
                endpoints.MapGrpcService<CountryGrpcService>();
                endpoints.MapGrpcService<AddressAggregateGrpcService>();
                endpoints.MapGrpcService<CustomerGrpcService>();
                endpoints.MapGrpcService<ContactGrpcService>();
                endpoints.MapGrpcService<CompanyTypeGrpcService>();
                endpoints.MapGrpcService<PriceGrpcService>();
                endpoints.MapGrpcService<TimeRegistrationGrpcService>();
                endpoints.MapGrpcService<UserGrpcService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }
    }
}