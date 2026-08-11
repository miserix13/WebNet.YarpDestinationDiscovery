using StackExchange.Redis;
using System.Collections.ObjectModel;
using Take.Elephant.Redis;
using Take.Elephant.Redis.Converters;

namespace WebNet.YarpDestinationDiscovery.Providers
{
    public class RedisProvider<TKey> : IAsyncDisposable, IDestinationProvider<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private readonly ConnectionMultiplexer multiplexer;
        private readonly RedisHashMap<TKey, Destination> hashMap;

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            this.hashMap.Dispose();
            await this.multiplexer.DisposeAsync();
        }

        public async Task<ReadOnlyDictionary<TKey, Destination>> GetAllAsync()
        {
            Dictionary<TKey, Destination> pairs = [];

            await foreach (var key in await this.hashMap.GetKeysAsync())
            {
                pairs.Add(key, await this.hashMap.GetValueOrDefaultAsync(key));
            }

            return pairs.AsReadOnly();
        }

        public async Task<Destination> GetDestinationAsync(TKey key)
        {
            return await this.hashMap.GetValueOrDefaultAsync(key);
        }

        public async Task RegisterAsync(TKey key, Destination value)
        {
            await this.hashMap.TryAddAsync(key, value, true);
        }

        public async Task UnregisterAsync(TKey key)
        {
            await this.hashMap.TryRemoveAsync(key);
        }

        public RedisProvider(ConnectionMultiplexer connectionMultiplexer, string mapName, int db = 0) :
            base()
        {
            this.multiplexer = connectionMultiplexer;
            this.hashMap = new(mapName, new ValueRedisDictionaryConverter<Destination>(), this.multiplexer, db);
        }
    }
}
