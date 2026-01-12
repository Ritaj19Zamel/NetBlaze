using Microsoft.AspNetCore.Mvc;
using NetBlaze.SharedKernel.Dtos.Fido.Requests;
using NetBlaze.SharedKernel.Dtos.Fido.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Application.Interfaces.General
{
    public interface IFidoService
    {
        Task<ApiResponse<FidoRegisterOptionsResponseDto>> StartFidoRegisterAsync(FidoLoginOptionsRequestDto request);
        Task<ApiResponse<object>> CompleteFidoRegisterAsync(FidoRegisterCompleteRequestDto dto);
        Task<ApiResponse<FidoLoginOptionsResponseDto>> StartFidoLoginAsync(FidoLoginOptionsRequestDto request);

        Task<ApiResponse<object>> CompleteFidoLoginAsync(FidoLoginCompleteRequestDto dto);
    }
}
