namespace TestDevFrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddDirectoryBrowser();

            var app = builder.Build();
            app.UseStaticFiles();  

            //app.MapGet("/htmlpage.html", () => "Hello World!");
            app.MapFallbackToFile("htmlpage.html");

            app.Run();
        }
    }
}
