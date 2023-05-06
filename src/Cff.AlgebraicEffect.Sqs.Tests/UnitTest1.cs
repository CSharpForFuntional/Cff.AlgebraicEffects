using Cff.AlgebraicEffect.Sqs.Config;
using CommunityToolkit.HighPerformance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Text;

namespace Cff.AlgebraicEffect.Sqs.Tests;

public record TestApp : SqsConfig;

public class SqsSpec
{
    [Fact]
    public async Task Work()
    {
        using var appsettings = Encoding.Default.GetBytes("""
            TestApp: &default
              SqsConfigs: 
              - Url : "https://sqs.ap-northeast-2.amazonaws.com/123456789012/test-1"
              - Url : "https://sqs.ap-northeast-2.amazonaws.com/123456789012/test-2"
            TestApp2: *default
            """).AsMemory().AsStream();


        var host = Host.CreateApplicationBuilder();

        host.Configuration.AddYamlStream(appsettings);

        host.Services.AddOptions<TestApp>().BindConfiguration("TestApp2");

        var app = host.Build();

        var option = app.Services.GetService<IOptions<TestApp>>();
    }
}