using System.Threading.Tasks;
using GarageGroup.Infra;
using Microsoft.Extensions.Hosting;

namespace GarageGroup.Platform.PushNotification;

static class Program
{
    static Task Main()
        =>
        FunctionHost.CreateFunctionsWorkerBuilderStandard(
            useHostConfiguration: false,
            configure: ApplicationService.Configure)
        .Build()
        .RunAsync();
}