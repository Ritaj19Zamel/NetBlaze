using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.Department.Requests;
using NetBlaze.SharedKernel.Dtos.Department.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> CreateDepartmentAsync(CreateDepartmentRequestDto dto, CancellationToken cancellationToken = default)
        {
            var exists = await _unitOfWork.Repository
                .AnyAsync<Department>(d =>
                    d.DepartmentName.Trim().ToLower() == dto.Name.Trim().ToLower() &&
                    !d.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.DepartmentAlreadyExists, HttpStatusCode.Conflict);
            }

            var department = new Department
            {
                DepartmentName = dto.Name.Trim()
            };

            await _unitOfWork.Repository.AddAsync(department, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.DepartmentCreatedSuccessfully);


        }

        public async Task<ApiResponse<List<GetDepartmentResponseDto>>> GetAllDepartmentAsync(CancellationToken cancellationToken = default)
        {
            var departments = await _unitOfWork.Repository
                .GetMultipleAsync<Department,GetDepartmentResponseDto>(true,
                     e => new GetDepartmentResponseDto()
                     {
                         Id = e.Id,
                         Name = e.DepartmentName
                     },cancellationToken);

            if(departments == null)
            {
                return ApiResponse<List<GetDepartmentResponseDto>>.ReturnFailureResponse(Messages.NoDepartments, HttpStatusCode.NotFound);
            }
               
            return ApiResponse<List<GetDepartmentResponseDto>>.ReturnSuccessResponse(departments);
        }

        public async Task<ApiResponse<GetDepartmentResponseDto>> GetDepartmentByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var department = await _unitOfWork.Repository
                .GetByIdAsync<Department, GetDepartmentResponseDto>(true,
                             id,
                             e => new GetDepartmentResponseDto
                             {
                                 Id = e.Id,
                                 Name = e.DepartmentName
                             },cancellationToken);

            if(department == null)
            {
                return ApiResponse<GetDepartmentResponseDto>.ReturnFailureResponse(Messages.DepartmentNotExists, HttpStatusCode.NotFound);
            }    
                
            return ApiResponse<GetDepartmentResponseDto>.ReturnSuccessResponse(department);
        }

        public async Task<ApiResponse<object>> ToggleDepartmentStatusAsync(long id, CancellationToken cancellationToken = default)
        {
            var department = await _unitOfWork.Repository.GetByIdAsync<Department>(false, id, cancellationToken);

            if (department == null || department.IsDeleted)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.DepartmentNotExists, HttpStatusCode.NotFound);
            }
                

            department.ToggleIsActive();

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.DepartmentStatusUpdatedSuccessfully);
        }

        public async Task<ApiResponse<object>> UpdateDepartmentAsync(
            UpdateDepartmentRequestDto dto,
            CancellationToken cancellationToken = default)
        {
            var department = await _unitOfWork.Repository
                .GetByIdAsync<Department>(false, dto.Id, cancellationToken);

            if (department == null || department.IsDeleted)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.DepartmentNotExists, HttpStatusCode.NotFound);
            }

            var exists = await _unitOfWork.Repository
               .AnyAsync<Department>(d =>
                   d.DepartmentName.Trim().ToLower() == dto.Name.Trim().ToLower() &&
                   !d.IsDeleted &&
                   d.Id != dto.Id,
                   cancellationToken);

            if (exists)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.DepartmentAlreadyExists, HttpStatusCode.Conflict);
            }

            department.DepartmentName = dto.Name.Trim();

            _unitOfWork.Repository.Update(department);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.DepartmentUpdatedSuccessfully);
        }

        public async Task<ApiResponse<object>> DeleteDepartmentAsync(long id,
            CancellationToken cancellationToken = default)
        {
            var department = await _unitOfWork.Repository.GetByIdAsync<Department>(false, id, cancellationToken);

            if (department == null || department.IsDeleted)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.DepartmentNotExists, HttpStatusCode.NotFound);
            }

            department.SetIsDeletedToTrue();
            department.ToggleIsActive();

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(Messages.DepartmentDeletedSuccessfully);
        }
    }
}
