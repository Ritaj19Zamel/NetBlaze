using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region Validation
        private async Task<ApiResponse<string>?> ValidatePolicyAsync(PolicyValidation validation, CancellationToken cancellationToken)
        {
            if (validation.PolicyType != PolicyType.WorkingHours && validation.WorkStartTime >= validation.WorkEndTime)
                return ApiResponse<string>.ReturnFailureResponse(Messages.InvalidWorkTimeRange, HttpStatusCode.BadRequest);

            if (validation.PolicyType == PolicyType.WorkingHours && validation.RequiredHours <= 0)
                return ApiResponse<string>.ReturnFailureResponse(Messages.InvalidRequiredHours, HttpStatusCode.BadRequest);


            var policies = await _unitOfWork.Repository.GetMultipleAsync<Policy, PolicyValidation>(
                true,
                p => p.PolicyType == validation.PolicyType,
                p => new PolicyValidation
                {
                    PolicyId = p.Id,
                    PolicyCode = p.PolicyCode,
                    PolicyType = p.PolicyType,
                    WorkStartTime = p.WorkStartTime,
                    WorkEndTime = p.WorkEndTime,
                    RequiredHours = p.RequiredHours
                },
                cancellationToken);

            if (policies.Any(p => p.PolicyCode == validation.PolicyCode &&
                  (validation.IgnoreId == null || p.PolicyId != validation.IgnoreId)))
                return ApiResponse<string>.ReturnFailureResponse(Messages.PolicyCodeExists, HttpStatusCode.BadRequest);


            if (validation.PolicyType != PolicyType.WorkingHours)
            {
                bool overlaps = policies.Any(p =>p.PolicyId != validation.IgnoreId &&
                    p.WorkStartTime < validation.WorkEndTime && validation.WorkStartTime < p.WorkEndTime);

                if (overlaps)
                    return ApiResponse<string>.ReturnFailureResponse(Messages.PolicyOverlap, HttpStatusCode.BadRequest);

            }

            return null; 
        }

        #endregion

        public async Task<ApiResponse<string>> CreateAsync(CreatePolicyRequestDto createPolicyRequestDto, CancellationToken cancellationToken = default)
        {
            var validation = await ValidatePolicyAsync(new PolicyValidation
            {
                PolicyCode = createPolicyRequestDto.PolicyCode,
                PolicyType = createPolicyRequestDto.PolicyType,
                WorkStartTime = createPolicyRequestDto.WorkStartTime,
                WorkEndTime = createPolicyRequestDto.WorkEndTime,
                RequiredHours = createPolicyRequestDto.RequiredHours,
                IgnoreId = null
            },cancellationToken);

            if (validation != null)
                return validation;
            var policy = new Policy
            {
                PolicyName = createPolicyRequestDto.PolicyName,
                PolicyCode = createPolicyRequestDto.PolicyCode,
                PolicyType = createPolicyRequestDto.PolicyType,
                WorkStartTime = createPolicyRequestDto.WorkStartTime,
                WorkEndTime = createPolicyRequestDto.WorkEndTime,
                ActionValue = createPolicyRequestDto.ActionValue,
                RequiredHours = createPolicyRequestDto.RequiredHours,
            };
            await _unitOfWork.Repository.AddAsync(policy, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<string>.ReturnSuccessResponse(Messages.PolicyCreated, Messages.PolicyCreated);
        }
        public async Task<ApiResponse<List<GetPolicyResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var policies = await _unitOfWork.Repository.GetMultipleAsync<Policy,GetPolicyResponseDto>(true,
                p => new GetPolicyResponseDto()
                {
                    PolicyName = p.PolicyName,
                    PolicyCode = p.PolicyCode,
                    PolicyType = p.PolicyType,
                    WorkStartTime = p.WorkStartTime,
                    WorkEndTime = p.WorkEndTime,
                    ActionValue = p.ActionValue,
                    RequiredHours = p.RequiredHours,

                }, cancellationToken);
            if (policies == null || policies.Count == 0)
                return ApiResponse<List<GetPolicyResponseDto>>.ReturnFailureResponse(Messages.NoPolicies, HttpStatusCode.NotFound);

            return ApiResponse<List<GetPolicyResponseDto>>.ReturnSuccessResponse(policies);
        }
        public async Task<ApiResponse<GetPolicyResponseDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var policy = await _unitOfWork.Repository.GetByIdAsync<Policy, GetPolicyResponseDto>(true,id,
                p => new GetPolicyResponseDto()
                {
                    PolicyName = p.PolicyName,
                    PolicyCode = p.PolicyCode,
                    PolicyType = p.PolicyType,
                    WorkStartTime = p.WorkStartTime,
                    WorkEndTime = p.WorkEndTime,
                    ActionValue = p.ActionValue,
                    RequiredHours = p.RequiredHours,

                }, cancellationToken);
            if (policy == null)
                return ApiResponse<GetPolicyResponseDto>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);

            return ApiResponse<GetPolicyResponseDto>.ReturnSuccessResponse(policy);
        }
        public async Task<ApiResponse<string>> UpdateAsync(long id, UpdatePolicyRequestDto updatePolicyRequestDto, CancellationToken cancellationToken = default)
        {
            var policy = await _unitOfWork.Repository.GetByIdAsync<Policy>(false, id, cancellationToken);

            if (policy == null)
                return ApiResponse<string>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);
            var validation = await ValidatePolicyAsync(new PolicyValidation
            {
                PolicyId = id,
                IgnoreId = id,
                PolicyCode = updatePolicyRequestDto.PolicyCode,
                PolicyType = updatePolicyRequestDto.PolicyType,
                WorkStartTime = updatePolicyRequestDto.WorkStartTime,
                WorkEndTime = updatePolicyRequestDto.WorkEndTime,
                RequiredHours = updatePolicyRequestDto.RequiredHours
            },
            cancellationToken);
            if (validation != null)
                return validation;

            policy.PolicyName = updatePolicyRequestDto.PolicyName;
            policy.PolicyCode = updatePolicyRequestDto.PolicyCode;
            policy.WorkStartTime = updatePolicyRequestDto.WorkStartTime;
            policy.WorkEndTime = updatePolicyRequestDto.WorkEndTime;
            policy.PolicyType = updatePolicyRequestDto.PolicyType;
            policy.ActionValue = updatePolicyRequestDto.ActionValue;
            policy.RequiredHours = updatePolicyRequestDto.RequiredHours;

            await _unitOfWork.Repository.UpdateAsync(policy, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<string>.ReturnSuccessResponse(Messages.PolicyUpdated, Messages.PolicyUpdated);
        }
        public async Task<ApiResponse<string>> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var policy = await _unitOfWork.Repository.GetByIdAsync<Policy>(false, id, cancellationToken);

            if (policy == null)
                return ApiResponse<string>.ReturnFailureResponse(Messages.PolicyNotFound, HttpStatusCode.NotFound);

            await _unitOfWork.Repository.SoftDeleteAsync<Policy,long>(policy, cancellationToken);
            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<string>.ReturnSuccessResponse(Messages.PolicyDeleted, Messages.PolicyDeleted);
        }
    }
    public sealed record PolicyValidation
    {
        public long PolicyId { get; set; }
        public string PolicyCode { get; set; }
        public PolicyType PolicyType { get; set; }
        public TimeOnly WorkStartTime { get; set; }
        public TimeOnly WorkEndTime { get; set; }
        public int RequiredHours { get; set; }
        public long? IgnoreId { get; set; }  
    }

}
