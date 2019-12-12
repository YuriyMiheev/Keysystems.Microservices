using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Bus
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
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o => { });
			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseStaticFiles(new StaticFileOptions() { ServeUnknownFileTypes = true });
			app.UseRouting();
			//app.UseAuthorization();
			//app.UseAuthentication();
			//app.UseCookiePolicy();

			app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllerRoute(name: "Admin", pattern: "{controller=Admin}/{action=Home}");
				});
		}
	}
}
