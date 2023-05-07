using Cff.AlgebraicEffect.Sqs.Config;
using LanguageExt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cff.AlgebraicEffect.Extensions;

public static class HostExtensions
{
    public static IHostBuilder UseSqs(this IHostBuilder hostBuilder) 
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            _ = services.AddOptions<SqsConfigurations>().BindConfiguration("");
        });
    }
    public static IHostBuilder UseSms(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            _ = services.AddOptions<SmsConfigurations>().BindConfiguration("");
        });
    }
}
