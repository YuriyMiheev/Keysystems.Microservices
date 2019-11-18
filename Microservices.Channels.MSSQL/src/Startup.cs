using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microservices.Channels.Configuration;
using Microservices.Channels.MSSQL.Controllers;
using Microservices.Channels.MSSQL.Hubs;
using Microservices.Channels.MSSQL.Configuration;

namespace Microservices.Channels.MSSQL
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
		public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
		{
			lifetime.ApplicationStarted.Register(() =>
				{
					var channelService = app.ApplicationServices.GetRequiredService<IChannelService>();
					var autostart = this.Configuration.GetValue<bool>("autostart");
					if (autostart)
						channelService.Open();
				});
			lifetime.ApplicationStopping.Register(() =>
				{
					var channelService = app.ApplicationServices.GetRequiredService<IChannelService>();
					channelService.Close();
				});

			app.UseDeveloperExceptionPage();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseWebSockets();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHub<ChannelHub>("/ChannelHub");
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
