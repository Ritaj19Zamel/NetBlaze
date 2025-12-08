

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
        [HttpPost()]
        public async Task<ApiResponse<string>> CreateAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await _vacationService.CreateAsync(createVacationRequestDto, cancellationToken);
        }

        [HttpDelete()]
        public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.DeleteAsync(id, cancellationToken);
        }
        [HttpGet("vacations")]
        public async Task<ApiResponse<List<GetVacationResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _vacationService.GetAllAsync(cancellationToken);
        }
        [HttpGet("{id:long}")]
        public async Task<ApiResponse<GetVacationResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _vacationService.GetByIdAsync(id, cancellationToken);
        }
        [HttpPut("{id:long}")]

        public async Task<ApiResponse<string>> UpdateAsync(long id, UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            return await (_vacationService.UpdateAsync(id, updateVacationRequestDto, cancellationToken));
        }
    }
}
