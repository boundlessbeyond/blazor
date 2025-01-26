using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blazor.Notifications.Api.SignalR;

public class DateTimeNotifier : BackgroundService
{
    private ILogger<DateTimeNotifier> logger;
    private IHubContext<NotificationsHub, INotificationClient> context;

    public DateTimeNotifier(ILogger<DateTimeNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context)
    {
        this.logger = logger;
        this.context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var now = DateTime.Now;
            logger.LogInformation("Time now {Service} {Time}", nameof(DateTimeNotifier), now);
            await context.Clients.
                User("27521a04-1879-439b-85d9-52a89997a293")
                .ReceiveNotification($"Now: {now}");
        }
    }
}
