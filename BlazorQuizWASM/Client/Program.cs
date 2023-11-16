using Blazor.FileReader;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace BlazorQuizWASM.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient("BlazorQuizWASM.ServerAPI", client => client.BaseAddress = new Uri("https://localhost:7105/"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorQuizWASM.ServerAPI"));

            builder.Services.AddFileReaderService(options => options.UseWasmSharedBuffer = false);

            builder.Services.AddMudServices();

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}