using Cff.AlgebraicEffect.Abstraction;
using k8s;

namespace Cff.AlgebraicEffect.K8s;

public interface IHasK8s<RT> : IHas<RT, IKubernetes> where RT: struct, IHas<RT, IKubernetes>
{
    
}

