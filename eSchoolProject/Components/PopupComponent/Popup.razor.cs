using Microsoft.AspNetCore.Components;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class Popup
    {
        [Parameter] public RenderFragment Body { get; set; } = default!;
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public bool IsVisible { get; set; }
        private async Task Close()
        {
            IsVisible = false;
            await PopupClosed.InvokeAsync();
        }
    }
}