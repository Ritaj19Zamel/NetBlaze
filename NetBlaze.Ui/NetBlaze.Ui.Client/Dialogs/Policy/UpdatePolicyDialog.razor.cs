using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.SharedKernel.Dtos.Policy.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Policy
{
    public partial class UpdatePolicyDialog
    {
        [Inject] BlazePolicyService BlazePolicyService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public GetPolicyResponseDto GetPolicyResponseDto { get; set; } = null!;

        private UpdatePolicyRequestDto _updatePolicyRequestDto = new();

        private TimeSpan? _workStartTimeProxy;
        private TimeSpan? _workEndTimeProxy;


        protected override void OnInitialized()
        {
            if (GetPolicyResponseDto is null)
                return;

            _updatePolicyRequestDto = new UpdatePolicyRequestDto
            {
                Id = GetPolicyResponseDto.Id,
                PolicyName = GetPolicyResponseDto.PolicyName,
                PolicyCode = GetPolicyResponseDto.PolicyCode,
                PolicyType = GetPolicyResponseDto.PolicyType,
                ActionValue = GetPolicyResponseDto.ActionValue ?? 0,
                RequiredHours = GetPolicyResponseDto.RequiredHours ?? 0,
                WorkStartTime = GetPolicyResponseDto.WorkStartTime ?? TimeOnly.MinValue,
                WorkEndTime = GetPolicyResponseDto.WorkEndTime ?? TimeOnly.MinValue
            };

            _workStartTimeProxy = _updatePolicyRequestDto.WorkStartTime.ToTimeSpan();
            _workEndTimeProxy = _updatePolicyRequestDto.WorkEndTime.ToTimeSpan();
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            _updatePolicyRequestDto.WorkStartTime =
                _workStartTimeProxy.HasValue
                    ? TimeOnly.FromTimeSpan(_workStartTimeProxy.Value)
                    : TimeOnly.MinValue;

            _updatePolicyRequestDto.WorkEndTime =
                _workEndTimeProxy.HasValue
                    ? TimeOnly.FromTimeSpan(_workEndTimeProxy.Value)
                    : TimeOnly.MinValue;

            var response = await BlazePolicyService.UpdatePolicyAsync(_updatePolicyRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_updatePolicyRequestDto));
            }
        }


        private void OnWorkStartTimeChanged(TimeSpan? value)
        {
            _workStartTimeProxy = value;
        }

        private void OnWorkEndTimeChanged(TimeSpan? value)
        {
            _workEndTimeProxy = value;
        }



    }
}
