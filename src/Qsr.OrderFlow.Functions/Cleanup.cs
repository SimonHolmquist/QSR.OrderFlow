using Microsoft.Azure.Functions.Worker;

namespace Qsr.OrderFlow.Functions;

public class Cleanup
{
    [Function("cleanup")]
    public static Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo timer, FunctionContext ctx)
    {
        // tu lógica de cleanup aquí
        return Task.CompletedTask;
    }
}