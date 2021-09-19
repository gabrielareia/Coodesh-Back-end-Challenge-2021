using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.Schedule
{
    public class ScheduleImport : IHostedService, IDisposable
    {
        public delegate void WorkHandler();
        public static event WorkHandler Work;


        private const string CONFIG_KEY = "ImportTime";
        private readonly ILogger<ScheduleImport> _logger;
        private readonly IConfiguration _config;
        private Timer _timer;

        public ScheduleImport(ILogger<ScheduleImport> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting data import.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            DateTime currentTime = DateTime.Now;
            DateTime scheduledTime = ScheduleNextWork();

            //Raising an event for the data import be implemented in a specific class.
            Work?.Invoke();

            _logger.LogInformation($"\nImported new data at {currentTime}. Next data import scheduled to {scheduledTime}.\n");
        }

        private DateTime ScheduleNextWork()
        {
            string timeString = _config.GetValue<string>(CONFIG_KEY);

            DateTime scheduledTime = Convert.ToDateTime(timeString);

            DateTime currentTime = DateTime.Now;

            TimeSpan timeRemaining;

            //If it's before 23:59 we set the next import for 23:59. Else we set the next import to be 23:59 of the next day.
            if (scheduledTime > currentTime)
            {
                timeRemaining = scheduledTime - currentTime;
            }
            else
            {
                timeRemaining = scheduledTime.AddDays(1) - currentTime;
            }

            //The schedule will happen only in the due time, so we can disable the period by calling Timeout.InfiniteTimeSpan
            _timer.Change(timeRemaining, Timeout.InfiniteTimeSpan);

            return DateTime.Now.Add(timeRemaining);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping data import.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
