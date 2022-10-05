using Grpc.Core;
using GrpcGreater;

namespace GrpcGreater.Services;

public class GreeterService : Greeter.GreeterBase
{
    //private readonly ILogger<GreeterService> _logger;

    public GreeterService()
    {
        //_logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
