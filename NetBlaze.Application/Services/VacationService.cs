using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class VacationService : IVacationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public VacationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region MyRegion
        private async Task<ApiResponse<string>?> ValidateVacationAsync(VacationValidation validation, CancellationToken cancellationToken)
        {
            if (validation.IsRecurring)
            {
                // Weekly recurring MUST have DayName
                // Yearly recurring MUST have DayDate
                if (string.IsNullOrWhiteSpace(validation.DayName) && validation.DayDate == null)
                    return ApiResponse<string>.ReturnFailureResponse(Messages.VacationInvalidRecurring, HttpStatusCode.BadRequest);
            }
            else
            {
                if (validation.DayDate == null)
                    return ApiResponse<string>.ReturnFailureResponse(Messages.VacationDateRequired, HttpStatusCode.BadRequest);
            }

            if (validation.IsRecurring && !string.IsNullOrWhiteSpace(validation.DayName))
            {
                var existsWeekly = await _unitOfWork.Repository.GetSingleAsync<Vacation>(
                    true,
                    x => x.IsRecurring == true &&
                         x.DayName!.ToLower() == validation.DayName.ToLower() &&
                         (validation.VacationId == null || x.Id != validation.VacationId),
                    cancellationToken);

                if (existsWeekly != null)
                    return ApiResponse<string>.ReturnFailureResponse(Messages.WeeklyVacationExists, HttpStatusCode.BadRequest);
            }

            if (validation.IsRecurring && validation.DayDate != null && string.IsNullOrWhiteSpace(validation.DayName))
            {
                var month = validation.DayDate.Value.Month;
                var day = validation.DayDate.Value.Day;

                var existsYearly = await _unitOfWork.Repository.GetSingleAsync<Vacation>(
                    true,
                    x => x.IsRecurring == true &&
                         x.DayDate != null &&
                         x.DayDate.Value.Month == month &&
                         x.DayDate.Value.Day == day &&
                         (validation.VacationId == null || x.Id != validation.VacationId),
                    cancellationToken);

                if (existsYearly != null)
                    return ApiResponse<string>.ReturnFailureResponse(Messages.YearlyVacationExists, HttpStatusCode.BadRequest);
            }

            if (!validation.IsRecurring && validation.DayDate != null)
            {
                var existsOneTime = await _unitOfWork.Repository.GetSingleAsync<Vacation>(
                    true,
                    x => x.IsRecurring == false &&
                         x.DayDate == validation.DayDate &&
                         (validation.VacationId == null || x.Id != validation.VacationId),
                    cancellationToken);

                if (existsOneTime != null)
                    return ApiResponse<string>.ReturnFailureResponse(Messages.OneTimeVacationExists, HttpStatusCode.BadRequest);
            }

            return null; 
        }

        #endregion
        public async Task<ApiResponse<string>> CreateAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateVacationAsync(new VacationValidation
            {
                DayName = createVacationRequestDto.DayName,
                DayDate = createVacationRequestDto.DayDate,
                IsRecurring = createVacationRequestDto.IsRecurring,
                VacationId = null
            }, cancellationToken);

            if (validationResult != null)
                return validationResult;
            var vacation = new Vacation
            {
                DayName = createVacationRequestDto.DayName,
                DayDate = createVacationRequestDto.DayDate,
                IsVacation = createVacationRequestDto.IsVacation,
                IsRecurring = createVacationRequestDto.IsRecurring
            };
            await _unitOfWork.Repository.AddAsync(vacation, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync();

            return ApiResponse<string>.ReturnSuccessResponse(Messages.VacationCreated, Messages.VacationCreated);
        }
        public async Task<ApiResponse<List<GetVacationResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var vacations = await _unitOfWork.Repository.GetMultipleAsync<Vacation, GetVacationResponseDto>(true,
                v => new GetVacationResponseDto()
                {
                    Id = v.Id,
                    DayName = v.DayName,
                    DayDate = v.DayDate,
                    IsRecurring = v.IsRecurring,
                    IsVacation = v.IsVacation,
                }, cancellationToken);
            if(vacations == null)
                return ApiResponse<List<GetVacationResponseDto>>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);
            return ApiResponse<List<GetVacationResponseDto>>.ReturnSuccessResponse(vacations);
        
        }
        public async Task<ApiResponse<GetVacationResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var vacation = await _unitOfWork.Repository.GetByIdAsync<Vacation, GetVacationResponseDto>(true, id,
                v => new GetVacationResponseDto()
                {
                    Id = v.Id,
                    DayName = v.DayName,
                    DayDate = v.DayDate,
                    IsRecurring = v.IsRecurring,
                    IsVacation = v.IsVacation,
                }, cancellationToken);
            if (vacation == null)
                return ApiResponse<GetVacationResponseDto>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);
            return ApiResponse<GetVacationResponseDto>.ReturnSuccessResponse(vacation);
        }
        public async Task<ApiResponse<string>> UpdateAsync(long id, UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var vacation = await _unitOfWork.Repository.GetByIdAsync<Vacation>(false, id, cancellationToken);
            if (vacation == null)
                return ApiResponse<string>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);

            var validationResult = await ValidateVacationAsync(new VacationValidation
            {
                VacationId = id,
                DayName = updateVacationRequestDto.DayName,
                DayDate = updateVacationRequestDto.DayDate,
                IsRecurring = updateVacationRequestDto.IsRecurring
            }, cancellationToken);

            if (validationResult != null)
                return validationResult;

            vacation.DayName = updateVacationRequestDto.DayName;
            vacation.DayDate = updateVacationRequestDto.DayDate;
            vacation.IsVacation = updateVacationRequestDto.IsVacation;
            vacation.IsRecurring = updateVacationRequestDto.IsRecurring;
            await _unitOfWork.Repository.UpdateAsync(vacation, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync();
            return ApiResponse<string>.ReturnSuccessResponse(Messages.VacationUpdated, Messages.VacationUpdated);

        }
        public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var vacation = await _unitOfWork.Repository.GetByIdAsync<Vacation>(false, id, cancellationToken);

            if (vacation == null)
                return ApiResponse<string>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);
            await _unitOfWork.Repository.SoftDeleteAsync<Vacation,long>(vacation, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync();
            return ApiResponse<string>.ReturnSuccessResponse(Messages.VacationDeleted, Messages.VacationDeleted);

        }
    }
    public sealed record VacationValidation
    {
        public long? VacationId { get; set; }
        public string? DayName { get; set; }
        public DateOnly? DayDate { get; set; }
        public bool IsRecurring { get; set; }
    }

}
