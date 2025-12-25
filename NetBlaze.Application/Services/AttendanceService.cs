

using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Views;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
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
       
        public async Task<ApiResponse<object>> AddAttendanceAsync(CancellationToken cancellationToken = default)
        {
            
            if (!_userContext.IsAuthenticated || _userContext.UserId == 0)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var todayDate = DateOnly.FromDateTime(DateTime.Now);

            bool isVacation = await IsTodayVacationAsync(todayDate, cancellationToken);

            if (isVacation)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.TodayIsVacation, HttpStatusCode.BadRequest);
            }
                
            var todayTime = TimeOnly.FromDateTime(DateTime.Now);

            var attendance = new EmployeeAttendence
            {
                UserId = _userContext.UserId,
                AttendDate = todayDate,
                AttendTime = todayTime
            };

            await _unitOfWork.Repository.AddAsync<EmployeeAttendence>(attendance, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.AttendanceRecorded, Messages.AttendanceRecorded);
        }


        public async Task<ApiResponse<PaginatedList<GetAttendanceResponseDto>>> GetAttendanceReportAsync(GetAttendanceRequestDto getAttendanceRequestDto
            , CancellationToken cancellationToken = default)
        {
            var attendanceRecords =  _unitOfWork.Repository.GetQueryable<AttendanceView>().AsNoTracking()
                 .Where(a => a.AttendDate >= getAttendanceRequestDto.From && 
                 a.AttendDate <= getAttendanceRequestDto.To)
                 .OrderBy(a => a.AttendDate)
                 .Select(a => new GetAttendanceResponseDto
                 {
                     UserId = a.UserId,
                     DisplayName = a.DisplayName,
                     Date = a.AttendDate,
                     CheckIn = a.CheckIn,
                     CheckOut = a.CheckOut
                 });

            if (attendanceRecords == null || !attendanceRecords.Any())
            {
                return ApiResponse<PaginatedList<GetAttendanceResponseDto>>.ReturnFailureResponse(Messages.NoAttendanceRecordsNotFound, HttpStatusCode.NotFound);
            }

            var result = await attendanceRecords.PaginatedListAsync(getAttendanceRequestDto.Pagination.PageNumber
                , getAttendanceRequestDto.Pagination.PageSize);

            return ApiResponse<PaginatedList<GetAttendanceResponseDto>>.ReturnSuccessResponse(result);
        }
        
        public async Task<ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>>GetCheckInViolations(GetCheckInViolationsRequestDto getCheckInViolationsRequestDto
            , CancellationToken cancellationToken = default)
        {
            var violationRecords = _unitOfWork.Repository.GetQueryable<CheckInViolationView>().AsNoTracking()
                .Where(v => v.AttendDate >= getCheckInViolationsRequestDto.FromDate && 
                v.AttendDate <= getCheckInViolationsRequestDto.ToDate)
                .GroupBy(v => new
                {
                    v.UserId,
                    v.UserName,
                    v.PolicyId,
                    v.PolicyName,
                    v.PolicyCode
                })
                .Select(v => new GetCheckInViolationResponseDto
                {
                    UserId = v.Key.UserId,
                    UserName = v.Key.UserName,
                    PolicyId = v.Key.PolicyId,
                    PolicyName = v.Key.PolicyName,
                    PolicyCode = v.Key.PolicyCode,
                    ViolationsCount = v.Count(),
                    TotalViolationValue = v.Sum(x => x.ViolationValue),
                })
                .OrderBy(x => x.UserName);

            if (violationRecords == null || !violationRecords.Any())
            {
                return ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>.ReturnFailureResponse(Messages.ViolationRecordsNotFound, HttpStatusCode.NotFound);
            }

            var result = await violationRecords.PaginatedListAsync(getCheckInViolationsRequestDto.Pagination.PageNumber
                , getCheckInViolationsRequestDto.Pagination.PageSize);

            return ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>.ReturnSuccessResponse(result);

        }

        public async Task<object> ApprovePolicyRequestAsync(ApprovePolicyRequestDto approvePolicyRequestDto,
            CancellationToken cancellationToken)
        {
            var violations = await _unitOfWork.Repository
                            .GetQueryable<CheckInViolationView>()
                            .Where(v =>
                                v.UserId == approvePolicyRequestDto.UserId &&
                                v.PolicyId == approvePolicyRequestDto.PolicyId &&
                                v.AttendDate >= approvePolicyRequestDto.FromDate &&
                                v.AttendDate <= approvePolicyRequestDto.ToDate)
                            .Select(v => v.AttendanceId)
                            .ToListAsync(cancellationToken);

            if (!violations.Any())
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.PolicyAlreadyReviewed, HttpStatusCode.BadRequest);
            }

            var policyActions = violations.Select(attendanceId =>
                                   new AttendencePolicyAction
                                   {
                                       AttendenceId = attendanceId,
                                       PolicyId = approvePolicyRequestDto.PolicyId,
                                       IsApplied = approvePolicyRequestDto.IsApplied,
                                       Clarification = approvePolicyRequestDto.Clarification
                                   }).ToList();

            _unitOfWork.Repository.AddRange(policyActions);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.PolicyReviewRecorded);
        }

    }
}
