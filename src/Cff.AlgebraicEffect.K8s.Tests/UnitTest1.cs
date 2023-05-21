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
        var cpus = await client.GetKubernetesPodsMetricsByNamespaceAsync("kube-system");

    }
}