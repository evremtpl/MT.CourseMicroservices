
using StackExchange.Redis;

namespace MT.FreeCourse.Basket.Services.Concrete
{
    public class RedisService
    {

        private readonly string _host;
        private readonly int _port;
        private ConnectionMultiplexer _connectionMultiplexer;
        public RedisService(int port, string host)
        {
            _port = port;
            _host = host;
        }

        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        public IDatabase GetDb(int dbNumber = 0) => _connectionMultiplexer.GetDatabase(dbNumber);
    }
}
