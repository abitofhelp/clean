// SOLUTION: Clean
// PROJECT: Clean.Configuration
// FILE: Startup.cs
// CREATED: Mike Gardner

namespace Clean.Configuration
{
    using System;
    using Adapter.Gateways.Repositories;
    using Clean.Adapter.Gateways.Security;
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shared.Interfaces;

    /// <summary>   A startup. </summary>
    public class Startup
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the configuration. </summary>
        ///
        /// <value> The configuration. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors / Finalizers

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="configuration">    The configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request
        /// pipeline.
        /// </summary>
        ///
        /// <param name="app">              The application. </param>
        /// <param name="env">              The environment. </param>
        /// <param name="serviceProvider">  The service provider. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
#if DEBUG

            // Using in-memory database for this simple application, which is good for TDD.  When running the API in debug mode, we will
            // load the following default data.
            var context = serviceProvider.GetService<MotorcycleContext>();
            MotorcycleContext.LoadContextWithTestData(context);
#endif
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        ///
        /// <param name="services"> The services. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void ConfigureServices(IServiceCollection services)
        {
            // Set up dependency injection for contexts.           
            services.AddDbContext<MotorcycleContext>(opt => opt.UseInMemoryDatabase("MotoMinder"));

            // Set up dependency injection.
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IDbContext, MotorcycleContext>();
        }

        #endregion
    }
}