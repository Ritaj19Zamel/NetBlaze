using Microsoft.Extensions.Configuration;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.OTP.Requests;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class RandomChecksService : IRandomChecksService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OtpSettings _otpSettings;

        #region HelperFunction
        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
        #endregion

        public RandomChecksService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork; 
            _otpSettings = configuration.GetSection(nameof(OtpSettings)).Get<OtpSettings>()!;
        }

        public async Task<ApiResponse<object>> GenerateOTP(GenerateOtpRequestDto generateOtpRequestDto, CancellationToken cancellationToken = default)
        {
            int expireMinutes = _otpSettings.ExpiryInMinutes;

            var randomCheck = await _unitOfWork.Repository.AddAsync<RandomChecks>(new RandomChecks()
            {
                UserId = generateOtpRequestDto.UserId,
                CreationTIme = DateTime.Now,
                Ischecked = false,
                ExpirationTime = DateTime.Now.AddMinutes(expireMinutes),
                OTP = GenerateOtp()
            });
            await _unitOfWork.Repository.CompleteAsync();
            return ApiResponse<object>.ReturnSuccessResponse(Messages.Checked);

        }


        public async Task<ApiResponse<object>> VerifyOTP(VerifyOTPRequestDto verifyOTPRequestDto, CancellationToken cancellationToken = default)
        {
            var otp = await _unitOfWork.Repository.GetSingleAsync<RandomChecks>(false,
                o => o.UserId == verifyOTPRequestDto.UserId &&
                o.OTP == verifyOTPRequestDto.OTP &&
                !o.Ischecked);
            if (otp == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.InvalidOTP, HttpStatusCode.BadRequest);
            }
            if (otp.ExpirationTime < DateTime.UtcNow)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.ExpiryOTP, HttpStatusCode.BadRequest);
            }
            otp.CheckDateTime = DateTime.UtcNow;
            otp.Ischecked = true;
            await _unitOfWork.Repository.CompleteAsync();
            return ApiResponse<object>.ReturnSuccessResponse(Messages.Checked);
        }
    }
}
