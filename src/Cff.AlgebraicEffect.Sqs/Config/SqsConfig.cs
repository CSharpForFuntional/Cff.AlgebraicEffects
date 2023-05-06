using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cff.AlgebraicEffect.Sqs.Config;


public abstract record SqsConfig
{
    public required IList<SqsConfigItem> SqsConfigs { get; init; } = new List<SqsConfigItem>();

    public record SqsConfigItem
    {
        public required string Url { get; init; }
        public required string Region { get; init; }
    }
}

