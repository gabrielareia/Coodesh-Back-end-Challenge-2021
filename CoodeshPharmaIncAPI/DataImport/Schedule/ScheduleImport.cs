using CoodeshPharmaIncAPI.Database;
using CoodeshPharmaIncAPI.Models.Import;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoodeshPharmaIncAPI.ImportData
{
    public class ScheduleImport : IHostedService, IDisposable
    {
        public delegate Task<int> WorkHandler(IServiceScopeFactory scopeFactory);
        public static event WorkHandler Work;

        private readonly IServiceScopeFactory _scopeFactory;

        private const string CONFIG_KEY = "ImportTime";
        private readonly ILogger<ScheduleImport> _logger;
        private readonly IConfiguration _config;
        private Timer _timer;

        public ScheduleImport(ILogger<ScheduleImport> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _config = config;
            _scopeFactory = scopeFactory;

            Work += new ImportData().Populate;
        }

        #region Start
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting data import.");

            DateTime? lastImported = GetLastImported();

            if (lastImported == null || lastImported.Value < GetTimeFromSettings().AddDays(-1))
            {
                //Today, import 23:59
                StartNowAndSchedule();
            }
            else
            {
                //Yesterday, import again now.
                ScheduleToConfiguredTime();
            }

            return Task.CompletedTask;
        }

        private void ScheduleToConfiguredTime()
        {
            TimeSpan scheduledTime = CalculateNextImport();
            _timer = new Timer(DoWork, null, scheduledTime, Timeout.InfiniteTimeSpan);
            _logger.LogInformation($"\nImport scheduled to happen {scheduledTime.ToString(@"hh\:mm\:ss")} from now.");
        }

        private void StartNowAndSchedule()
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, Timeout.InfiniteTimeSpan);
        }

        private DateTime? GetLastImported()
        {
            DateTime? lastImported;

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                PharmaContext ctx = scope.ServiceProvider.GetRequiredService<PharmaContext>();

                lastImported = GetDateFromDB(ctx);
            }

            return lastImported;
        }
        #endregion

        #region DoSomeWork
        private async void DoWork(object state)
        {
            _logger.LogInformation($"\nStarting Work");
            _logger.LogInformation($"\nImport initiated at: {DateTime.Now.ToString("HH:mm:ss")}\n");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Raising an event for the data import be implemented in a specific class.
            int importedItemsCount = await Work?.Invoke(_scopeFactory);

            stopwatch.Stop();

            //Reset Timer.            
            DateTime scheduledTime = ScheduleNextWork();
            DateTime currentTime = DateTime.Now;

            //Update "last updated" in db for the next time.
            UpdateTimeDb(currentTime);

            _logger.LogInformation($"\n{ stopwatch.ElapsedMilliseconds} ms to execute.\n" +
                $"Imported {importedItemsCount} new random users at {currentTime}.\n" +
                $"Next data import scheduled to {scheduledTime.ToString("dd/MM/yyyy HH:mm:ss")}, " +
                $"{(scheduledTime - currentTime).ToString(@"hh\:mm\:ss")} hours from now.\n");
        }

        private void UpdateTimeDb(DateTime currentTime)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                PharmaContext ctx = scope.ServiceProvider.GetRequiredService<PharmaContext>();

                UpdateDateInDB(currentTime, ctx);
            }
        }

        private DateTime ScheduleNextWork()
        {
            TimeSpan timeRemaining = CalculateNextImport();

            //The schedule will happen only in the due time, so we can disable the period by calling Timeout.InfiniteTimeSpan
            _timer.Change(timeRemaining, Timeout.InfiniteTimeSpan);


            return DateTime.Now.Add(timeRemaining);
        }

        private TimeSpan CalculateNextImport()
        {
            DateTime scheduledTime = GetTimeFromSettings();

            DateTime currentTime = DateTime.Now;

            TimeSpan timeRemaining;

            //If it's before 23:59 we set the next import for 23:59.
            //Else we set the next import to be 23:59 of the next day.
            if (scheduledTime > currentTime)
            {
                timeRemaining = scheduledTime - currentTime;
            }
            else
            {
                timeRemaining = scheduledTime.AddDays(1) - currentTime;
            }

            return timeRemaining;
        }

        private DateTime GetTimeFromSettings()
        {
            string timeString = _config.GetValue<string>(CONFIG_KEY);

            DateTime scheduledTime = Convert.ToDateTime(timeString);
            return scheduledTime;
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

        #region Shared

        public DateTime? GetDateFromDB(PharmaContext ctx)
        {
            return ctx.ImportInfo.FirstOrDefault()?.LastImported;
        }

        public void UpdateDateInDB(DateTime time, PharmaContext ctx)
        {
            ImportInfo info = ctx.ImportInfo.FirstOrDefault();

            if (info == null)
            {
                info = new ImportInfo() { LastImported = time };
                ctx.ImportInfo.Add(info);
            }
            else
            {
                info.LastImported = time;
                ctx.ImportInfo.Update(info);
            }

            ctx.SaveChanges();
        }
        #endregion
    }
}
