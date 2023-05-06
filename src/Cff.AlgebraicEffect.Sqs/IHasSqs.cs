namespace Cff.AlgebraicEffect.Sqs;

using Amazon.SQS;
using Amazon.SQS.Model;
using Cff.AlgebraicEffect.Abstraction;
using Cff.AlgebraicEffect.Sqs.Internal;
using CommunityToolkit.HighPerformance;
using LanguageExt;
using static Schedule;

public interface IHasSqs<RT> : IHas<RT, IAmazonSQS> where RT : struct, IHasSqs<RT>
{
    public Aff<RT, Unit> SendAff(IList<string> urls, object message, string? hash1 = null) =>
        from sqs in Eff
        from ct in cancelToken<RT>()
        let json = TypedJsonSerializer.Serialize(message)
        let hash = hash1 switch
        {
            { } v => Math.Abs(v.GetDjb2HashCode()),
            _ => Math.Abs(json.GetDjb2HashCode())
        }
        from _1 in Aff(() => sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = urls[hash % urls.Count],
            MessageBody = json,
            MessageGroupId = $"{hash}"
        }, ct).ToValue()).RetryWhile
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