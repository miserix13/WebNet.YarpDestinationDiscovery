using System.Text.Json;

namespace WebNet.YarpDestinationDiscovery
{
    public record Destination
    {
        public string Address { get; set; }
        public string Health { get; set; }
        public string Host { get; set; }
        public Dictionary<string, string> Metadata { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Destination>(this);
        }

        public static Destination Parse(string value)
        {
            return JsonSerializer.Deserialize<Destination>(value);
        }

        public Yarp.ReverseProxy.Configuration.DestinationConfig GetDestinationConfig()
        {
            return new() { Address = this.Address, Health = this.Health, Host = this.Host, Metadata = this.Metadata };
        }

        public static Destination GetDestination(Yarp.ReverseProxy.Configuration.DestinationConfig config)
        {
            return new() { Address = config.Address, Health = config.Health, Host = config.Host, Metadata = config.Metadata.ToDictionary() };
        }
    }
}
