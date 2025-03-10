using Application.Common.Extensions;
using Application.Infrastructure.Clients;
using Application.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Emotes", client =>
        {
            client.BaseAddress = configuration.GetRequiredValue<Uri>("7TV:BaseAddress");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddStandardResilienceHandler();

        services.AddHttpClient("Images").AddStandardResilienceHandler();
        
        services.AddTransient<EmoteClient>();
        services.AddTransient<ImageClient>();

        services.AddSingleton<ImageService>();
        services.AddSingleton<EmoteService>();
        
        
    }
}