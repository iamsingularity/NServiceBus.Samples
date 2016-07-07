
namespace NSB06Retries.Sender
{
    using NServiceBus;

    /*
        This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
        can be found here: http://particular.net/articles/the-nservicebus-host
    */
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<JsonSerializer>();
            configuration.UseTransport<RabbitMQTransport>()
                .ConnectionString("host=localhost");

            configuration.EnableInstallers();
            configuration.Conventions()
                .DefiningCommandsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Commands" ) )
                .DefiningEventsAs( t => t.Namespace != null && t.Namespace.EndsWith( ".Events" ) );
        }
    }

    public class Startup : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            for(int i = 0; i < 100; i++)
            //while(true)
            {
                this.Bus.Send("NSB06Retries.Receiver", new NSB06Retries.Messages.Commands.SampleCommand());
            }
        }

        public void Stop()
        {

        }
    }
}
