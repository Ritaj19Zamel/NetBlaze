using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.SharedResources;

namespace NetBlaze.Ui.Client.Dialogs.Attendence
{
    public class ViolationDecisionDialogBase : ComponentBase
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public bool IsApproved { get; set; }

        protected string _clarification = string.Empty;

        protected string _title =>
            IsApproved ? Messages.Approve : Messages.Reject;

        protected void Submit()
        {
            MudDialog.Close(DialogResult.Ok(_clarification));
        }

        protected void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
