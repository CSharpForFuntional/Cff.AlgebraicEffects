namespace Service.Sqs.Extensions;
using System;
using System.Collections.Generic;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Cff.Service.Sqs.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Sqs.Abstractions;
using Service.Sqs.Config;
using Service.Sqs.Internal;

public static partial class UseSqsExtension
{
    private static void TryAddDefaultAwsOptions(IConfiguration config, IServiceCollection services)
    {
        if (services.Any(x => x.ServiceType == typeof(AWSOptions)))
        {
            return;
        }

        _ = services.AddDefaultAWSOptions(config.GetAWSOptions());
    }

    public static IHostBuilder UseEffect<T, TC>
    (
        this IHostBuilder host,
        T appName,
        Func<TC, IServiceProvider, TC>? func = null
    ) where T : struct, Enum
      where TC : class
    {
        func ??= (o, sp) => o;

        _ = host.ConfigureServices((ctx, services) =>
        {
            TryAddDefaultAwsOptions(ctx.Configuration, services);

            _ = services.AddOptions<Dictionary<string, TC>>()
                        .BindConfiguration("")
                        .PostConfigure<IServiceProvider>((option, sp) =>
                        {
                            option[Enum.GetName(appName)!] = func.Invoke(option[Enum.GetName(appName)!], sp);
                        });
        });

        return host;
    }

    public static IHostBuilder UseEffectSqs<T>
    (
        this IHostBuilder host,
        T appName,
        Func<SqsOptionsContext, IServiceProvider, SqsOptionsContext>? func = null
    ) where T : struct, Enum
    {
        _ = host.UseEffect(appName, func);
        _ = host.ConfigureServices((ctx, services) =>
        {
            _ = services.AddSingleton<ISqsService, SqsService>();
            _ = services.AddHttpClient("SQS");
            _ = services.AddSingleton<SqsHttpClientFactory>();
            _ = services.AddSingleton<IAmazonSQS, AmazonSQSClient>(sp => new AmazonSQSClient(new AmazonSQSConfig
            {
                RegionEndpoint = sp.GetRequiredService<AWSOptions>().Region,
                HttpClientFactory = sp.GetRequiredService<SqsHttpClientFactory>(),
            }));

            _ = services.AddTransient(typeof(ISubscribeSqs<>), typeof(SubscribeSqs<>));
            _ = services.AddHostedServices(sp => new SqsHostedService<T>(sp, appName));
        });

        return host;
    }
}