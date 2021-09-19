using CoodeshPharmaIncAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.Schedule
{
    public partial class ScheduleImport : IHostedService, IDisposable
    {
        // public delegate Task<int> WorkHandler();
        // public static event WorkHandler Work;

        private readonly HttpClient _client;
        //private readonly PharmaContext _ctx;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string CONFIG_KEY = "ImportTime";
        private readonly ILogger<ScheduleImport> _logger;
        private readonly IConfiguration _config;
        private Timer _timer;

        public ScheduleImport(ILogger<ScheduleImport> logger, IConfiguration config, 
            HttpClient client, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _config = config;
            _client = client;
            _scopeFactory = scopeFactory;
        }

        #region Start
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting data import.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }
        #endregion

        #region DoSomeWork
        private async void DoWork(object state)
        {
            DateTime currentTime = DateTime.Now;
            DateTime scheduledTime = ScheduleNextWork();

            // //Raising an event for the data import be implemented in a specific class.
            // int importedItems = await Work?.Invoke();

            var s = new Stopwatch();
            s.Start();

            // >>>>> TEM COISA Q SERIA TRUNCATED, PRECISA MEXER AMANHA. <<<<<
            // >>>>> O TITULO DO NOME ACEITA "MONSIEUR", MUDAR A QUANTIDADE ACEITA<<<<<
            int importedItems = await Populate();
            s.Stop();

            _logger.LogInformation($"\n\nIt took { s.ElapsedMilliseconds} ms to execute\n" + 
                $"Imported {importedItems} new User data at {currentTime}.\n" +
                $"Next data import scheduled to {scheduledTime.ToString("dd/MM/yyyy HH:mm:ss")}, " +
                $"{(scheduledTime - currentTime).ToString(@"hh\:mm\:ss")} hours from now.\n");
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
        #endregion

        #region End
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
        #endregion
    }
}
