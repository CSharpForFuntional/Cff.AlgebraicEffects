using System.Threading;
using System.Threading.Tasks;

namespace Cff.Service.Sqs.Abstractions;

public interface ISubscribeSqs<in T>
{
    Task HandleAsync(T dto);

    int VisibleTimeOutSec => 30;
}
