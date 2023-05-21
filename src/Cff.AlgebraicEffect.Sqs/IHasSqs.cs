namespace Cff.AlgebraicEffect.Sqs;

using Amazon.SQS;
using Amazon.SQS.Model;
using Cff.AlgebraicEffect.Abstraction;
using Cff.AlgebraicEffect.Sqs.Internal;
using CommunityToolkit.HighPerformance;
using LanguageExt;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using static Schedule;
using Cff.AlgebraicEffect.Sqs.Config;

public interface IHasSqs<RT> : IHas<RT, IServiceProvider>, IHas<RT, IAmazonSQS> 
    where RT : struct, IHasSqs<RT>
{

    private static Eff<RT, IAmazonSQS> AmazonSqsEff => IHas<RT, IAmazonSQS>.Eff;
    private static Eff<RT, IServiceProvider> ServiceProviderEff => IHas<RT, IServiceProvider>.Eff;


    public static Aff<RT, Unit> SendAff<T>(object message, string? hash1 = null) where T : SqsConfig =>
        from sp in ServiceProviderEff
        let urls = sp.GetRequiredService<IOptions<T>>().Value.SqsConfigs.Map(x => x.Url).ToArr()
        from _1 in SendAff(urls, message, hash1)
        select unit;


    public static Aff<RT, Unit> SendAff(Arr<string> urls, object message, string? hash1 = null) =>
        from sqs in AmazonSqsEff
        from ct in cancelToken<RT>()
        let json = TypedJsonSerializer.Serialize(message)
        let hash = hash1 switch
        {
            { } v => Math.Abs(v.GetDjb2HashCode()),
            _ => Math.Abs(json.GetDjb2HashCode())
        }
        let req = new SendMessageRequest
        {
            QueueUrl = urls[hash % urls.Count],
            MessageBody = json,
            MessageGroupId = $"{hash}"
        }
        from _1 in Aff(sqs.SendMessageAsync(req, ct).ToValue).RetryWhile
        (
            fibonacci(1 * sec) | recurs(5),
            error => error.ToException() switch
            {
                AmazonSQSException { Message: "Request is throttled." } => true,
                AmazonSQSException { StatusCode: System.Net.HttpStatusCode.InternalServerError } => true,
                _ => false
            }
        )
        select unit;
}