using System.Collections.ObjectModel;

namespace WebNet.YarpDestinationDiscovery
{
    public interface IDestinationProvider<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        Task<ReadOnlyDictionary<TKey, Destination>> GetAllAsync();
        Task<Destination> GetDestinationAsync(TKey key);
        Task RegisterAsync(TKey key, Destination value);
        Task UnregisterAsync(TKey key);
    }
}
