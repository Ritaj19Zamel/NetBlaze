

using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        

        public AttendanceService(IUserContext userContext,IUnitOfWork unitOfWork)
        {
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }
        #region HelperFunction
        private async Task<bool> IsTodayVacationAsync(DateOnly todayDate, CancellationToken cancellationToken)
        {
            var todayName = DateTime.Now.DayOfWeek;

            var weeklyVacation = await _unitOfWork.Repository.GetSingleAsync<Vacation>(true,
                v =>v.IsRecurring == true && 
                v.DayName == todayName
                ,cancellationToken);

            if (weeklyVacation != null)
            {
                return true;
            }   
            var vacation = await _unitOfWork.Repository.GetSingleAsync<Vacation>(true,
                v => v.DayDate != null && 
                v.DayDate == todayDate
                , cancellationToken);

            return vacation != null;
        }
        #endregion
        #region AttendencePolicy
        private async Task ApplyAbsencePolicyAsync(long userId, DateOnly date, CancellationToken cancellationToken)
        {
            var dayPolicy = await _unitOfWork.Repository.GetSingleAsync<Policy>(
                true, p => p.PolicyType == PolicyType.Absence,
                cancellationToken);

            var action = new AttendencePolicyAction
            {
                PolicyId = dayPolicy.Id,
                AttendenceId = 0,
                IsApplied = true,
                Clarification = $"Absent on {date}"
            };

            await _unitOfWork.Repository.AddAsync(action, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);
        }
        //private async Task<Policy?> DetermineLatePolicyAsync(TimeOnly attendTime, CancellationToken cancellationToken)
        //{
        //    var basePolicy = await _unitOfWork.Repository.GetSingleAsync<Policy>(true,
        //        p => p.PolicyType == PolicyType.WorkingHours, cancellationToken);
        //    if (basePolicy == null)
        //        return null;
        //    var startTime = basePolicy.WorkStartTime;
        //    var minutesLate = (attendTime.ToTimeSpan() - startTime.ToTimeSpan()).TotalMinutes;
        //}

        #endregion
        public async Task<ApiResponse<string>> AddAttendanceAsync(CancellationToken cancellationToken = default)
        {
            
            if (!_userContext.IsAuthenticated || _userContext.UserId == 0)
            {
                return ApiResponse<string>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }
            //Check if today is Vacation-----------------------------------------------------------------------
            var todayDate = DateOnly.FromDateTime(DateTime.Now);

            bool isVacation = await IsTodayVacationAsync(todayDate, cancellationToken);

            if (isVacation)
            {
                return ApiResponse<string>.ReturnFailureResponse(Messages.TodayIsVacation, HttpStatusCode.BadRequest);
            }
                
            ///---------------------------------------------------------------------------------------------------
            var todayTime = TimeOnly.FromDateTime(DateTime.Now);

            var attendance = new EmployeeAttendence
            {
                UserId = _userContext.UserId,
                AttendDate = todayDate,
                AttendTime = todayTime
            };

            await _unitOfWork.Repository.AddAsync<EmployeeAttendence>(attendance, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<string>.ReturnSuccessResponse(Messages.AttendanceRecorded, Messages.AttendanceRecorded);
        }
        public async Task ProcessDailyAttendanceAsync(long userId, DateOnly date, CancellationToken cancellationToken)
        {
            var attendence = await _unitOfWork.Repository.GetMultipleAsync<EmployeeAttendence>(
                true,
                x => x.UserId == userId && x.AttendDate == date,
                cancellationToken);
            if (attendence.Count ==0)
            {
                await ApplyAbsencePolicyAsync(userId, date, cancellationToken);
                return;
            }
            var checkIn = attendence.MinBy(x => x.AttendTime);
            var checkOut = attendence.MaxBy(x => x.AttendTime);

        }
    }
}
