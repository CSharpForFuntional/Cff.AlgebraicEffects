using Cff.AlgebraicEffect.Extensions;
using Cff.AlgebraicEffect.Sqs.Config;
using CommunityToolkit.HighPerformance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Text;

namespace Cff.AlgebraicEffect.Sqs.Tests;


public class SqsSpec
{
    [Fact]
    public async Task Work()
    {
        using var appsettings = Encoding.Default.GetBytes("""
            TestApp2: *default
            TestApp1: &default
              SmsUrl: https://sms.ap-northeast-2.amazonaws.com
              SqsConfigs: 
              - Url : https://sqs.ap-northeast-2.amazonaws.com/123456789012/test-1
              - Url : https://sqs.ap-northeast-2.amazonaws.com/123456789012/test-2
            """).AsMemory().AsStream();

        var builder = Host.CreateDefaultBuilder()
                          .ConfigureAppConfiguration((context, config) =>
                          {
                              config.Sources.Clear();
                              _ = config.AddYamlStream(appsettings);
                          })
                          .UseSqs()
                          .UseSms();
                      
        var app = builder.Build();

        var sqsOption = app.Services.GetRequiredService<IOptions<SqsConfigurations>>().Value;
        var smsOption = app.Services.GetRequiredService<IOptions<SmsConfigurations>>().Value;

        
    }
}