using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Policy.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Policy
{
    public partial class AddPolicyDialog
    {
        [Inject] BlazePolicyService BlazePolicyService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        private CreatePolicyRequestDto _createPolicyRequestDto = new();

        private TimeSpan? _workStartTimeProxy;
        private TimeSpan? _workEndTimeProxy;


        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            if (!_workStartTimeProxy.HasValue || !_workEndTimeProxy.HasValue)
                return;

            _createPolicyRequestDto.WorkStartTime =
                TimeOnly.FromTimeSpan(_workStartTimeProxy.Value);

            _createPolicyRequestDto.WorkEndTime =
                TimeOnly.FromTimeSpan(_workEndTimeProxy.Value);

            var response =
                await BlazePolicyService.CreatePolicyAsync(_createPolicyRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_createPolicyRequestDto));
            }
        }

    }
}
