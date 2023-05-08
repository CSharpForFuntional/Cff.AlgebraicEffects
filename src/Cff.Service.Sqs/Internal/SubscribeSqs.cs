using Cff.Service.Sqs.Abstractions;

namespace Cff.Service.Sqs.Internal;


public class SubscribeSqs<in T> : ISubscribeSqs<T> where T : notnull
{
    public Task HandleAsync(T dto) => throw new UnhandleDtoException(dto);
}
