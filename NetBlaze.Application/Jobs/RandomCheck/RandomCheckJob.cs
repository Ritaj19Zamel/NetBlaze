using Hangfire;
using Microsoft.AspNetCore.Identity;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Application.Jobs.RandomCheck
{
    public class RandomCheckJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRandomChecksService _randomChecksService;
        private readonly UserManager<User> _userManager;
        private readonly IWorkingDayService _workingDayService;

        public RandomCheckJob(
            IUnitOfWork unitOfWork,
            IRandomChecksService randomChecksService,
            UserManager<User> userManager,
            IWorkingDayService workingDayService)
        {
            _unitOfWork = unitOfWork;
            _randomChecksService = randomChecksService;
            _userManager = userManager;
            _workingDayService = workingDayService;
        }

 
        public async Task GenerateTodaySchedules()
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            if ((bool)await _workingDayService.IsVacationDayAsync(today))
            {
                return;
            }
               

            var config = await _unitOfWork.Repository
                .GetSingleAsync<RandomCheckAutoConfig>(
                    false,
                    x => x.IsEnabled &&
                         today >= x.FromDate &&
                         today <= x.ToDate);

            if (config == null)
            {
                return;
            }
                

            var windowStart = DateTime.Today.Add(config.FromTime);
            var windowEnd = DateTime.Today.Add(config.ToTime);

            if (windowEnd <= DateTime.Now)
            {
                return;

            }

            if (windowStart < DateTime.Now)
                windowStart = DateTime.Now;

            var randomTimes = GenerateRandomDistributedTimes(
                windowStart,
                windowEnd,
                config.ChecksPerDay);

            foreach (var time in randomTimes)
            {
                BackgroundJob.Schedule<RandomCheckJob>(
                    job => job.ExecuteRandomCheck(),
                    time);
            }
        }

       
        public async Task ExecuteRandomCheck()
        {
            if ((bool)await _workingDayService.IsVacationDayAsync(
                    DateOnly.FromDateTime(DateTime.Now)))
            {
                return;
            }
                

            await _randomChecksService.GenerateOTP(new()
            {
                SendToAllEmployees = true,
                UserIds = []
            });
        }

       
        private List<DateTime> GenerateRandomDistributedTimes(
            DateTime start,
            DateTime end,
            int count)
        {
            if (count <= 0 || start >= end)
            {
                return [];
            }
                

            var random = new Random();
            var result = new List<DateTime>();

            var totalSeconds = (end - start).TotalSeconds;
            var segmentSize = totalSeconds / count;

            for (int i = 0; i < count; i++)
            {
                var segmentStart = start.AddSeconds(segmentSize * i);
                var segmentEnd = start.AddSeconds(segmentSize * (i + 1));

                var randomOffset =
                    random.NextDouble() * (segmentEnd - segmentStart).TotalSeconds;

                result.Add(segmentStart.AddSeconds(randomOffset));
            }

            return result.OrderBy(x => x).ToList();
        }
    }
}
