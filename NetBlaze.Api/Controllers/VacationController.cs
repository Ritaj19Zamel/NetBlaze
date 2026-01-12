

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.HRAndManager)]
    public class VacationController : BaseNetBlazeController, IVacationService
    {
        private readonly IVacationService _vacationService;
        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }
        [HttpPost("CreateVacation")]
        public async Task<ApiResponse<object>> CreateVacationAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await _vacationService.CreateVacationAsync(createVacationRequestDto, cancellationToken);
        }

        [HttpDelete("DeleteVacation/{id}")]
        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.DeleteVacationAsync(id, cancellationToken);
        }
        [HttpGet("GetVacations")]
        public async Task<ApiResponse<PaginatedList<GetVacationResponseDto>>> GetAllVacationAsync(int PageNumber, int PageSize, CancellationToken cancellationToken = default)
        {
            return await _vacationService.GetAllVacationAsync(PageNumber, PageSize, cancellationToken);
        }

        [HttpGet("GetVacationById/{id}")]
        public async Task<ApiResponse<GetVacationResponseDto>> GetVacationByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.GetVacationByIdAsync(id, cancellationToken);
        }
        [HttpPut("UpdateVacation")]

        public async Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await (_vacationService.UpdateVacationAsync(updateVacationRequestDto, cancellationToken));
        }

        
    }
}
