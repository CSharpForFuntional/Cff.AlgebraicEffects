using k8s;

namespace Cff.AlgebraicEffect.K8s.Tests;

public class UnitTest1
{
    [Fact]
    public async Task  Test1()
    {
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();

        

        IKubernetes client = new Kubernetes(config)
        {
            
        };

        var pods = await client.CoreV1.ListNamespacedPodAsync("kube-system");
        var cpus = await client.CustomObjects.GetNamespacedCustomObjectStatusAsync("metrics.k8s.io", "v1beta1", "kube-system", "pods", string.Empty);

    }
}