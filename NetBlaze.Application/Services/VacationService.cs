using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.General;
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
        #region HelperFunction
        private async Task<ApiResponse<object>?> ValidateVacationAsync(VacationValidationDto validation, CancellationToken cancellationToken)
        {
            if (validation.IsRecurring)
            {
                if (validation.DayName == null && validation.DayDate == null)
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.VacationInvalidRecurring, HttpStatusCode.BadRequest);
                }
                    
            }
            else
            {
                if (validation.DayDate == null)
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.VacationDateRequired, HttpStatusCode.BadRequest);
                }
                    
            }

            if (validation.IsRecurring && validation.DayName != null)
            {
                var existsWeekly = await _unitOfWork.Repository.GetSingleAsync<Vacation>(
                    true,
                    x => x.IsRecurring  &&
                         x.DayName == validation.DayName &&
                         (validation.VacationId == null || x.Id != validation.VacationId),
                    cancellationToken);

                if (existsWeekly != null)
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.WeeklyVacationExists, HttpStatusCode.BadRequest);
                }
                    
            }

            if (validation.IsRecurring && validation.DayDate != null && validation.DayName == null)
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
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.YearlyVacationExists, HttpStatusCode.BadRequest);
                }
                    
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
                {
                    return ApiResponse<object>.ReturnFailureResponse(Messages.OneTimeVacationExists, HttpStatusCode.BadRequest);
                }
                    
            }

            return null; 
        }
        #endregion
        public async Task<ApiResponse<object>> CreateVacationAsync(CreateVacationRequestDto createVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateVacationAsync(new VacationValidationDto
            {
                DayName = createVacationRequestDto.DayName,
                DayDate = createVacationRequestDto.DayDate,
                IsRecurring = createVacationRequestDto.IsRecurring,
                VacationId = null
            }, cancellationToken);

            if (validationResult != null)
            {
                return validationResult;
            }
                
            var vacation = new Vacation
            {
                DayName = createVacationRequestDto.DayName,
                DayDate = createVacationRequestDto.DayDate,
                IsVacation = createVacationRequestDto.IsVacation,
                IsRecurring = createVacationRequestDto.IsRecurring
            };

            await _unitOfWork.Repository.AddAsync(vacation, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.VacationCreated, Messages.VacationCreated);
        }
        public async Task<ApiResponse<PaginatedList<GetVacationResponseDto>>> GetAllVacationAsync(int PageNumber, int PageSize
            , CancellationToken cancellationToken = default)
        {
            var vacations = _unitOfWork.Repository
                        .GetQueryable<Vacation>()
                        .OrderBy(v => v.Id)
                        .Select(v => new GetVacationResponseDto
                        {
                            Id = v.Id,
                            DayName = v.DayName,
                            DayDate = v.DayDate,
                            IsRecurring = v.IsRecurring,
                            IsVacation = v.IsVacation,
                        });



            var result = await vacations.PaginatedListAsync(PageNumber, PageSize);

            return ApiResponse<PaginatedList<GetVacationResponseDto>>.ReturnSuccessResponse(result);


        }
        public async Task<ApiResponse<GetVacationResponseDto>> GetVacationByIdAsync(long id, CancellationToken cancellationToken = default)
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
            {
                return ApiResponse<GetVacationResponseDto>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);
            }
                
            return ApiResponse<GetVacationResponseDto>.ReturnSuccessResponse(vacation);
        }
        public async Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var vacation = await _unitOfWork.Repository.GetByIdAsync<Vacation>(false, updateVacationRequestDto.Id, cancellationToken);
            if (vacation == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);
            }

            var validationResult = await ValidateVacationAsync(new VacationValidationDto
            {
                VacationId = updateVacationRequestDto.Id,
                DayName = updateVacationRequestDto.DayName,
                DayDate = updateVacationRequestDto.DayDate,
                IsRecurring = updateVacationRequestDto.IsRecurring
            }, cancellationToken);

            if (validationResult != null)
            {
                return validationResult;
            }
                

            vacation.DayName = updateVacationRequestDto.DayName;
            vacation.DayDate = updateVacationRequestDto.DayDate;
            vacation.IsVacation = updateVacationRequestDto.IsVacation;
            vacation.IsRecurring = updateVacationRequestDto.IsRecurring;

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.VacationUpdated, Messages.VacationUpdated);

        }
        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            var vacation = await _unitOfWork.Repository.GetByIdAsync<Vacation>(false, id, cancellationToken);

            if (vacation == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.NoVacation, HttpStatusCode.NotFound);
            }

            vacation.SetIsDeletedToTrue();
            vacation.ToggleIsActive();

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.VacationDeleted, Messages.VacationDeleted);

        }

        
    }
    

}
