using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using WebReaper.Domain;
using WebReaper.Scheduler.Abstract;

namespace WebReaper.Scheduler.Concrete;

public class RedisScheduler : IScheduler
{
    private readonly string _queueName;
    private readonly ILogger _logger;
    private static ConnectionMultiplexer redis;
    
    public RedisScheduler(string connectionString, string queueName, ILogger logger)
    {
        _queueName = queueName;
        _logger = logger;
        redis = ConnectionMultiplexer.Connect(connectionString, config =>
        {
            config.AbortOnConnectFail = false;

            config.AsyncTimeout = 180000;
            config.SyncTimeout = 180000;

            config.ReconnectRetryPolicy = new ExponentialRetry(10000);
        });
    }

    public async IAsyncEnumerable<Job> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Start {nameof(RedisScheduler)}.{nameof(GetAllAsync)}");
        
        var db = redis.GetDatabase();

        while (!cancellationToken.IsCancellationRequested)
        {
            var rawResult = await db.ListLeftPopAsync(_queueName);

            if (!rawResult.HasValue)
            {
                await Task.Delay(300, cancellationToken);
                continue;
            }
            
            var job = JsonConvert.DeserializeObject<Job>(rawResult);

            yield return job;
        }
    }

    public async Task AddAsync(Job job, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Start {nameof(RedisScheduler)}.{nameof(AddAsync)}");
        
        var db = redis.GetDatabase();
        await db.ListRightPushAsync(_queueName, SerializeToJson(job));
    }

    public async Task AddAsync(IEnumerable<Job> jobs, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Start {nameof(RedisScheduler)}.{nameof(AddAsync)} with multiple jobs");
        
        IDatabase db = redis!.GetDatabase();
        
        foreach (var job in jobs)
        {
            await db.ListRightPushAsync(_queueName, SerializeToJson(job));
        }
    }
    
    private static string SerializeToJson(Job job) => JsonConvert.SerializeObject(job, Formatting.None);
}