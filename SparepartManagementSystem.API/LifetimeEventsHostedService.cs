using Serilog;
using ILogger = Serilog.ILogger;

namespace SparepartManagementSystem.API
{
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger _logger = Log.ForContext<LifetimeEventsHostedService>();

        public LifetimeEventsHostedService(IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.Information("AXFinMobile.API has been started.");
        }

        private void OnStopping()
        {
            _logger.Information("AXFinMobile.API is stopping.");
        }

        private void OnStopped()
        {
            _logger.Information("AXFinMobile.API has been stopped.");
        }
    }
}
