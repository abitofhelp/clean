// SOLUTION: Clean
// PROJECT: Clean.Configuration
// FILE: Program.cs
// CREATED: Mike Gardner

namespace Clean.Configuration
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>   A program. This class cannot be inherited. </summary>
    internal sealed class Program
    {
        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Builds web host. </summary>
        ///
        /// <param name="args"> An array of command-line argument strings. </param>
        ///
        /// <returns>   An IWebHost. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .Build();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <param name="args"> An array of command-line argument strings. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void Main(string[] args)
        {
            BuildWebHost(args)
                .Run();
        }

        #endregion
    }
}