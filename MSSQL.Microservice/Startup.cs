using System;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MSSQL.Microservice.Hubs;

namespace MSSQL.Microservice
{
	public class Startup
	{
		public Startup(IWebHostEnvironment environment, IConfiguration configuration)
		{
			this.HostingEnvironment = environment;
			this.Configuration = configuration;
		}

		public IWebHostEnvironment HostingEnvironment { get; }

		public IConfiguration Configuration { get; }


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSignalR().AddMessagePackProtocol();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app /*, IHostApplicationLifetime lifetime*/)
		{
			app.UseDeveloperExceptionPage();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseWebSockets();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("hello", async context =>
					{
						await context.Response.WriteAsync("Hello World!");
					});
				endpoints.MapHub<ChannelHub>("/ChannelHub", options => { });
				endpoints.MapControllerRoute(name: "default", pattern: "{controller=Channel}/{action=Info}");
			});


			//if (service.Config.ConfigFileSettings.LogHttpRequest)
			//	app.UseMiddleware<RequestLoggingMiddleware>(_logger);
		}


		private string ApplicationInfo()
		{
			IWebHostEnvironment env = this.HostingEnvironment;

			var sb = new StringBuilder()
			.AppendFormat("\tApplicationName             : {0}", env.ApplicationName).AppendLine()
			.AppendFormat("\tEnvironmentName             : {0}", env.EnvironmentName).AppendLine()
			.AppendFormat("\tBaseDirectory               : {0}", AppDomain.CurrentDomain.BaseDirectory).AppendLine()
			.AppendFormat("\tCurrentDirectory            : {0}", Directory.GetCurrentDirectory()).AppendLine()
			.AppendFormat("\tContentRootPath             : {0}", env.ContentRootPath).AppendLine()
			.AppendFormat("\tWebRootPath                 : {0}", env.WebRootPath).AppendLine();
			return sb.ToString();
		}
	}
}
