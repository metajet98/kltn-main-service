using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using main_service.Databases;
using main_service.RestApi.Response;
using main_service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace main_service.CronJob
{
    public class NotificationJob : CronJobService
    {
        private readonly ILogger<NotificationJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationJob(IScheduleConfig<NotificationJob> config, ILogger<NotificationJob> logger, IServiceScopeFactory scopeFactory) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NotificationJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} NotificationJob is working.");
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                var fcmService = scope.ServiceProvider.GetRequiredService<FcmService>();
                var now = DateTime.Now;
                var scheduleFcmData = FcmData.CreateFcmData("schedule_notification", null);
                var userCalenderFcmData = FcmData.CreateFcmData("user_calender_notification", null);

                var schedules = dbContext.MaintenanceSchedule
                    .Include(x => x.UserVehicle).ThenInclude(x => x.User)
                    .Where(x =>
                        x.Date != null && x.Date.Value.Day == now.Day && x.Date.Value.Month == now.Month &&
                        x.Date.Value.Year == now.Year)
                    .ToList();
                _logger.LogInformation($"Total schedule: {schedules.Count}");
                
                var scheduleMessages = schedules.Select(x => new
                {
                    UserId = x.UserVehicle.User.Id,
                    Notidication = FcmData.CreateFcmNotification(x.Title ?? "", x.Content ?? "", null),
                }).ToList();
                
                scheduleMessages.ForEach(obj =>
                {
                    fcmService.SendMessage(obj.UserId, scheduleFcmData, obj.Notidication);
                });
                
                var userCalenders = dbContext.CustomerCalender
                    .Include(x => x.User)
                    .Include(x => x.Branch)
                    .Where(x =>
                        x.Time.Day == now.Day && x.Time.Month == now.Month &&
                        x.Time.Year == now.Year)
                    .ToList();
                
                _logger.LogInformation($"Total user calender: {userCalenders.Count}");

                var userCalenderMessages = userCalenders.Select(x => new
                {
                    UserId = x.User.Id,
                    Notidication = FcmData.CreateFcmNotification("Đến hẹn đã đặt", $"Bạn có một lịch hẹn đã đặt vào {x.Time.ToLongTimeString()}, hãy đến chi nhánh {x.Branch.Name} ngay", null),
                }).ToList();
                
                userCalenderMessages.ForEach(obj =>
                {
                    fcmService.SendMessage(obj.UserId, userCalenderFcmData, obj.Notidication);
                });
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NotificationJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}