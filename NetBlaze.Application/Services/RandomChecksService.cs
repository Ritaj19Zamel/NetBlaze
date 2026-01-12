using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Jobs.RandomCheck;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.Email;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class RandomChecksService : IRandomChecksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OtpSettings _otpSettings;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly IUserContext _userContext;
        private readonly IWorkingDayService _workingDayService;


        public RandomChecksService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IConfiguration configuration,
            IEmailService emailService,
            IUserContext userContext,
            IWorkingDayService workingDayService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailService = emailService;
            _otpSettings = configuration.GetSection(nameof(OtpSettings)).Get<OtpSettings>()!;
            _userContext = userContext;
            _workingDayService = workingDayService;
        }


        #region HelperFunction
        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
        #endregion

        public async Task<ApiResponse<object>> SaveAutoRandomCheckConfig(AutoRandomCheckRequestDto dto,
            CancellationToken cancellationToken)
        {
            if (dto.FromDate > dto.ToDate)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidDateRange);
            }

            if (dto.FromTime >= dto.ToTime)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidTimeRange);
            }

            if (dto.ChecksPerDay <= 0)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidTimeRange);
            }

            var oldConfigs = _unitOfWork.Repository
                .GetQueryable<RandomCheckAutoConfig>()
                .Where(x => x.IsEnabled);

            foreach (var c in oldConfigs)
                c.IsEnabled = false;

            var config = new RandomCheckAutoConfig
            {
                IsEnabled = dto.IsEnabled,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                FromTime = dto.FromTime,
                ToTime = dto.ToTime,
                ChecksPerDay = dto.ChecksPerDay,
               
            };

            await _unitOfWork.Repository.AddAsync(config, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            BackgroundJob.Enqueue<RandomCheckJob>(job => job.GenerateTodaySchedules());

            return ApiResponse<object>.ReturnSuccessResponse(Messages.ConfigurationSaved);
        }
        public async Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto dto, CancellationToken cancellationToken = default)
        {
            if((bool)await _workingDayService.IsVacationDayAsync(DateOnly.FromDateTime(DateTime.Now), cancellationToken))
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.TodayIsVacation, HttpStatusCode.BadRequest);
            }

            var roleName = AppRoles.Employee.ToString();

            List<OtpTargetUserDto> users;

            if (dto.SendToAllEmployees)
            {
                var employees = await _userManager.GetUsersInRoleAsync(roleName);

                users = employees
                    .Select(u => new OtpTargetUserDto
                    {
                        Id = u.Id,
                        DisplayName = u.DisplayName,
                        Email = u.Email!
                    })
                    .ToList();
            }
            else
            {
                if (!dto.UserIds.Any())
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.NoUsersProvided, HttpStatusCode.BadRequest);
                }
                    

                var employees = await _userManager.GetUsersInRoleAsync(roleName);

                users = employees
                    .Where(u => dto.UserIds.Contains(u.Id))
                    .Select(u => new OtpTargetUserDto
                    {
                        Id = u.Id,
                        DisplayName = u.DisplayName,
                        Email = u.Email!
                    })
                    .ToList();
            }

            if (!users.Any())
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.UserNotFound, HttpStatusCode.NotFound);
            }


            var now = DateTime.UtcNow;
            var expiryMinutes = _otpSettings.ExpiryInMinutes;
            var verifyLink = _otpSettings.VerifyBaseUrl;

            var emails = new List<EmailMessageDto>();

            foreach (var user in users)
            {
                var otp = GenerateOtp();

                var randomCheck = new RandomChecks
                {
                    UserId = user.Id,
                    OTP = otp,
                    CreationTime = now,
                    ExpirationTime = now.AddMinutes(expiryMinutes),
                    Ischecked = false
                };

                await _unitOfWork.Repository.AddAsync(randomCheck, cancellationToken);

               
                emails.Add(new EmailMessageDto
                {
                    To = user.Email,
                    Subject = Messages.RandomCheckOtpSubject,
                    Body = $"""
                        <p>{Messages.Hello} {user.DisplayName}</p>
                        <p>{Messages.YourOtpCode}: <b>{otp}</b></p>
                        <p>
                            <a href="{verifyLink}">
                                {Messages.VerifyOtp}
                            </a>
                        </p>
                        """
                });
            }

            await _emailService.SendBulkAsync(emails);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>
                .ReturnSuccessResponse(Messages.OtpSentSuccessfully);
        }
        public async Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default)
        {
            var UserId = _userContext.UserId;
            var otp = await _unitOfWork.Repository.GetSingleAsync<RandomChecks>(
                        false,
                        x => x.UserId == UserId &&
                             x.OTP == verifyOTPRequestDto.OTP &&
                             x.Ischecked == false,
                        cancellationToken);

            if (otp == null)
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidOTP, HttpStatusCode.BadRequest);

            if (otp.ExpirationTime < DateTime.UtcNow)
                return ApiResponse<object>.ReturnFailureResponse(Messages.ExpiryOTP, HttpStatusCode.BadRequest);

            otp.Ischecked = true;
            otp.CheckDateTime = DateTime.UtcNow;

            await _unitOfWork.Repository.UpdateAsync(otp, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.Checked);
        }
        public async Task<ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>> GetUserChecksAsync(GetUserRandomChecksRequestDto getUserRandomChecksRequestDto,
            CancellationToken cancellationToken = default)
        {
            var checks = _unitOfWork.Repository
                       .GetQueryable<RandomChecks>()
                       .Include(c => c.User)
                       .Where(c => c.UserId == getUserRandomChecksRequestDto.UserId &&
                       c.CreationTime >= getUserRandomChecksRequestDto.From &&
                       c.CreationTime <= getUserRandomChecksRequestDto.To)
                       .Select(c => new GetUserRandomChecksResponseDto
                       {
                           Id = c.Id,
                           UserId = c.UserId,
                           UserName = c.User.UserName!,
                           CreationTime = c.CreationTime,
                           ExpirationTime = c.ExpirationTime,
                           CheckDateTime = c.CheckDateTime,
                           IsChecked = c.Ischecked
                       });
            if(checks == null || !checks.Any())
            {
                return ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>.ReturnFailureResponse(Messages.NoChecksFound, HttpStatusCode.NotFound);
            }
            var result = await PaginatedList<GetUserRandomChecksResponseDto>
                                  .CreateAsync(checks,
                                  getUserRandomChecksRequestDto.Pagination.PageNumber,
                                  getUserRandomChecksRequestDto.Pagination.PageSize);
            return ApiResponse<PaginatedList<GetUserRandomChecksResponseDto>>.ReturnSuccessResponse(result);
        }
    }
}
