using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.AlgebraicEffect.Sqs.Config;


public class SqsConfigurations : Dictionary<string, SqsConfig>
{
}

public class SmsConfigurations : Dictionary<string, ISmsConfig>
{
}

public record SqsConfig
{
    public required List<SqsConfigItem> SqsConfigs { get; init; } 

    public record SqsConfigItem
    {
        public required string Url { get; init; }
        public required string Region { get; init; }
    }
}

public record ISmsConfig
{
    public required string SmsUrl { get; init; }
}