

using Microsoft.AspNetCore.Mvc;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Api.Controllers
{
  
    public class VacationController : BaseNetBlazeController, IVacationService
    {
        private readonly IVacationService _vacationService;
        public VacationController(IVacationService vacationService)
        {
            _vacationService = vacationService;
        }
        [HttpPost("createvacation")]
        public async Task<ApiResponse<object>> CreateAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await _vacationService.CreateAsync(createVacationRequestDto, cancellationToken);
        }

        [HttpDelete("deletevacation")]
        public async Task<ApiResponse<object>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.DeleteAsync(id, cancellationToken);
        }
        [HttpGet("getvacations")]
        public async Task<ApiResponse<List<GetVacationResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _vacationService.GetAllAsync(cancellationToken);
        }
        [HttpGet("{id}")]
        public async Task<ApiResponse<GetVacationResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.GetByIdAsync(id, cancellationToken);
        }
        [HttpPut("{id}")]

        public async Task<ApiResponse<object>> UpdateAsync(long id, UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await (_vacationService.UpdateAsync(id, updateVacationRequestDto, cancellationToken));
        }

    }
}
