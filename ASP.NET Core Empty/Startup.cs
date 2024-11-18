using ASP.NET_Core_Empty.Middlewares;

namespace ASP.NET_Core_Empty
{
    public class Startup
    {
        //private IWebHostEnvironment _env;

        //public Startup(IWebHostEnvironment env)
        //{
        //    _env = env;
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            void About(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
                });
            }

            void Config(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
                });
            }

            Console.WriteLine($"Launching project from: {env.ContentRootPath}");

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Поддержка статических файлов
            app.UseStaticFiles();

            // Подключаем логирвоание с использованием ПО промежуточного слоя
            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });

            app.Map("/about", About);
            app.Map("/config", Config);

            // Добавим в конвейер запросов обработчик самым простым способом
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync($"Page not found");
            //});

            app.UseStatusCodePages();
        }
    }
}
