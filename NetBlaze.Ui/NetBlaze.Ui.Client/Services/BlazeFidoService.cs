using Fido2NetLib;
using Fido2NetLib.Objects;
using NetBlaze.SharedKernel.Dtos.Fido.Requests;
using NetBlaze.SharedKernel.Dtos.Fido.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Net.Http.Json;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazeFidoService : BaseBlazService
    {
        public BlazeFidoService(
            ExternalHttpClientWrapper http,
            CentralizedSnackbarProvider snackbar)
            : base(http, snackbar) { }

        public async Task<ApiResponse<FidoRegisterOptionsResponseDto>> StartRegisterAsync(long userId)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<object, ApiResponse<FidoRegisterOptionsResponseDto>>(
                    ApiRelativePaths.FIDO_REGISTER_OPTIONS,
                    new FidoLoginOptionsRequestDto(userId));
        }

        public async Task<ApiResponse<object>> CompleteRegisterAsync(FidoRegisterCompleteRequestDto request)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<FidoRegisterCompleteRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.FIDO_REGISTER_COMPLETE,
                    request);
        }

        public async Task<ApiResponse<FidoLoginOptionsResponseDto>>StartFidoLoginAsync(long userId)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<object, ApiResponse<FidoLoginOptionsResponseDto>>(
                    ApiRelativePaths.FIDO_LOGIN_OPTIONS,
                    new FidoLoginOptionsRequestDto(userId));
        }

        public async Task<ApiResponse<object>>CompleteFidoLoginAsync(FidoLoginCompleteRequestDto dto)
        {
            return await _externalHttpClientWrapper
                .PostAsJsonAsync<FidoLoginCompleteRequestDto, ApiResponse<object>>(
                    ApiRelativePaths.FIDO_LOGIN_COMPLETE,
                    dto);
        }

    }
}
