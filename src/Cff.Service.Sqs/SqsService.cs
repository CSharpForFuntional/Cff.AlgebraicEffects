using Amazon.SQS;
using Amazon.SQS.Model;
using Cff.Service.Sqs.Abstractions;
using CommunityToolkit.HighPerformance;

namespace Cff.Service.Sqs;

internal sealed class SqsService : ISqsService
{
    public SqsService(IAmazonSQS amazonSQS, SqsOptions option)
    {
        AmazonSQS = amazonSQS;
        Option = option;
    }

    public IAmazonSQS AmazonSQS { get; }
    public SqsOptions Option { get; }

    public async Task SendMessageAsync<T>(T AppName, object message) where T : struct, Enum
    {
        var json = TypedJsonSerializer.Serialize(message);
        var hash = Math.Abs(json.GetDjb2HashCode());
        var urls = Option.Value[Enum.GetName(AppName)!].SqsConfigs.Select(x => x.Url).ToArray();

        while (true)
        {
            try
            {
                _ = await AmazonSQS.SendMessageAsync(new SendMessageRequest
                {
                    QueueUrl = urls[hash % urls.Length],
                    MessageBody = json,
                    MessageGroupId = $"{hash}",
                });

                return;
            }
            catch (AmazonSQSException ex) when (ex.Message == "Request is throttled.")
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
            }
        }
    }
}
