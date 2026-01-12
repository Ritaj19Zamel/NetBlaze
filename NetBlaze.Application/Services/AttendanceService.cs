

using Microsoft.EntityFrameworkCore;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Views;
using NetBlaze.SharedKernel.Dtos.Attendence.Requests;
using NetBlaze.SharedKernel.Dtos.Attendence.Responses;
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
        private readonly IWorkingDayService _workingDayService;


        public AttendanceService(IUserContext userContext,IUnitOfWork unitOfWork, IWorkingDayService workingDayService)
        {
            _userContext = userContext;
            _unitOfWork = unitOfWork;
            _workingDayService = workingDayService;
        }

        
        public async Task<ApiResponse<object>> AddAttendanceAsync(CancellationToken cancellationToken = default)
        {
            
            if (!_userContext.IsAuthenticated || _userContext.UserId == 0)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var todayDate = DateOnly.FromDateTime(DateTime.Now);

            bool isVacation = (bool)await _workingDayService.IsVacationDayAsync(todayDate, cancellationToken);

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
            var baseQuery = _unitOfWork.Repository
                                .GetQueryable<CheckInViolationView>()
                                .AsNoTracking()
                                .Where(v =>
                                    v.AttendDate >= getCheckInViolationsRequestDto.FromDate &&
                                    v.AttendDate <= getCheckInViolationsRequestDto.ToDate);

            var groupedQuery =
                baseQuery
                .GroupBy(v => new
                {
                    v.UserId,
                    v.UserName,
                    v.PolicyId,
                    v.PolicyName,
                    v.PolicyCode
                })
                .Select(g => new GetCheckInViolationResponseDto
                {
                    UserId = g.Key.UserId,
                    UserName = g.Key.UserName,
                    PolicyId = g.Key.PolicyId,
                    PolicyName = g.Key.PolicyName,
                    PolicyCode = g.Key.PolicyCode,

                    ViolationStatus =
                        g.Any(x => x.IsApplied == null) ? ViolationStatus.Pending :
                        g.Any(x => x.IsApplied == true) ? ViolationStatus.Applied :
                        ViolationStatus.Rejected,

                    ViolationsCount = g.Count(),
                    TotalViolationValue = g.Sum(x => x.ViolationValue)
                });

            if (getCheckInViolationsRequestDto.ViolationStatus.HasValue)
            {
                groupedQuery =
                    groupedQuery.Where(x => x.ViolationStatus == getCheckInViolationsRequestDto.ViolationStatus.Value);
            }

            groupedQuery = groupedQuery.OrderBy(x => x.UserName);

            if (!await groupedQuery.AnyAsync(cancellationToken))
            {
                return ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>
                    .ReturnFailureResponse(
                        Messages.ViolationRecordsNotFound,
                        HttpStatusCode.NotFound);
            }



            var result = await groupedQuery.PaginatedListAsync(getCheckInViolationsRequestDto.Pagination.PageNumber
                , getCheckInViolationsRequestDto.Pagination.PageSize);

            return ApiResponse<PaginatedList<GetCheckInViolationResponseDto>>.ReturnSuccessResponse(result);

        }

        public async Task<ApiResponse<object>> ApprovePolicyRequestAsync(ApprovePolicyRequestDto approvePolicyRequestDto,
            CancellationToken cancellationToken)
        {
            var violations = await _unitOfWork.Repository
                            .GetQueryable<CheckInViolationView>()
                            .Where(v =>
                                v.UserId == approvePolicyRequestDto.UserId &&
                                v.PolicyId == approvePolicyRequestDto.PolicyId &&
                                v.AttendDate >= approvePolicyRequestDto.FromDate &&
                                v.AttendDate <= approvePolicyRequestDto.ToDate)
                            .Select(v => new AttendencePolicyAction
                            {
                                AttendenceId = v.AttendanceId,
                                PolicyId = approvePolicyRequestDto.PolicyId,
                                IsApplied = approvePolicyRequestDto.IsApplied,
                                Clarification = approvePolicyRequestDto.Clarification
                            })
                            .ToListAsync(cancellationToken);

            if (!violations.Any())
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.PolicyAlreadyReviewed, HttpStatusCode.BadRequest);
            }

            await _unitOfWork.Repository.AddRangeAsync(violations);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.PolicyReviewRecorded);
        }

        public async Task<ApiResponse<GetTodayAttendanceResponseDto>> GetTodayAttendanceAsync(CancellationToken cancellationToken = default)
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId == 0)
            {
                return ApiResponse<GetTodayAttendanceResponseDto>.ReturnFailureResponse(Messages.InvalidToken, HttpStatusCode.Unauthorized);
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            var attendance = await _unitOfWork.Repository
                .GetQueryable<AttendanceView>()
                .AsNoTracking()
                .Where(a =>
                    a.UserId == _userContext.UserId &&
                    a.AttendDate == today)
                .OrderBy(a => a.AttendDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (attendance == null)
            {
                return ApiResponse<GetTodayAttendanceResponseDto>
                    .ReturnSuccessResponse(new GetTodayAttendanceResponseDto
                    {
                        Date = today,
                        CheckIn = null,
                        CheckOut = null
                    });
            }

            return ApiResponse<GetTodayAttendanceResponseDto>
                .ReturnSuccessResponse(new GetTodayAttendanceResponseDto
                {
                    Date = attendance.AttendDate,
                    CheckIn = attendance.CheckIn,
                    CheckOut = attendance.CheckOut
                });
        }


    }

}
