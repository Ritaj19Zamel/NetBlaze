using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class RandomChecksService : IRandomChecksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OtpSettings _otpSettings;
        public RandomChecksService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _otpSettings = configuration.GetSection(nameof(OtpSettings)).Get<OtpSettings>()!;
        }

        #region HelperFunction
        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
        #endregion

        public async Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default)
        {
            if (generateOtpRequestDto.UserIds == null || !generateOtpRequestDto.UserIds.Any())
                return ApiResponse<object>.ReturnFailureResponse(Messages.NoUsersProvided, HttpStatusCode.BadRequest);

            var existingUsers = await _unitOfWork.Repository
                                .GetMultipleAsync<User, long>(
                                    true,
                                    u => generateOtpRequestDto.UserIds.Contains(u.Id),
                                    u => u.Id,
                                    cancellationToken
                                );

            int expiryMinutes = _otpSettings.ExpiryInMinutes;
            var otps = new List<RandomChecks>();

            string otpCode = GenerateOtp();
            
            foreach (var userId in existingUsers)
            {
                otps.Add(new RandomChecks
                {
                    UserId = userId,
                    OTP = GenerateOtp(),
                    CreationTime = DateTime.UtcNow,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(expiryMinutes),
                    Ischecked = false
                });
            }

            await _unitOfWork.Repository.AddRangeAsync(otps, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            //Send OTP to user via email
            return ApiResponse<object>.ReturnSuccessResponse(Messages.Checked);

        }

        public async Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default)
        {
            var otp = await _unitOfWork.Repository.GetSingleAsync<RandomChecks>(
                        false,
                        x => x.UserId == verifyOTPRequestDto.UserId &&
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
                           OTP = c.OTP,
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
