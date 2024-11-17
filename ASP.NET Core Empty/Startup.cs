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

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //Используем метод Use, чтобы запрос передавался дальше по конвейеру
            app.Use(async (context, next) =>
            {
                // Строка для публикации в лог
                string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

                // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
                string logFilePath = Path.Combine(env.ContentRootPath, "Logs", "RequestLog.txt");

                // Используем асинхронную запись в файл
                await File.AppendAllTextAsync(logFilePath, logMessage);

                await next.Invoke();
            });

            //Добавляем компонент для логирования запросов с использованием метода Use.
            app.Use(async (context, next) =>
            {
                // Для логирования данных о запросе используем свойства объекта HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");

                await next.Invoke();
            });

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
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });
        }
    }
}
