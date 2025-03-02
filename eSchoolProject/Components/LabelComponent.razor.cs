using Microsoft.AspNetCore.Components;

namespace eSchoolProject.Components
{
    public partial class LabelComponent
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}