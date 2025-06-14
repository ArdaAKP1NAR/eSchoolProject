using Microsoft.AspNetCore.Components;

namespace eSchoolProject.Components.Component
{
    public partial class CheckboxComponent<TValueType> : ComponentBase
    {
        [Parameter] public string Label { get; set; } = default!;
        [Parameter] public string id { get; set; } = default!;
        [Parameter] public string Name { get; set; } = default!;
        [Parameter] public TValueType Checked { get; set; } = default!;
        private bool internalChecked;
        [Parameter] public EventCallback<TValueType> CheckedChanged { get; set; } = default!;
        protected override async Task OnParametersSetAsync()
        {
            if (typeof(TValueType) == typeof(int) || typeof(TValueType) == typeof(short))
            {
                var value = (int)(object)Checked!;
                if (value == 1)
                {
                    internalChecked = true;
                }
                else if (value == 0)
                {
                    internalChecked = false;
                }
                else
                {
                    throw new InvalidDataException("Deger 1 yada 0 olmali");
                }
            }
            else if (typeof(TValueType) == typeof(bool))
            {
                internalChecked = (bool)(object)Checked!;
            }

            await base.OnParametersSetAsync();
        }
        private async Task onValueChanged()
        {
            TValueType valueType = default!;

            if (typeof(TValueType) == typeof(int) || typeof(TValueType) == typeof(short))
            {
                if (internalChecked)
                {
                    valueType = (TValueType)(object)1;
                }
                else
                {
                    valueType = (TValueType)(object)0;
                }
            }
            else if (typeof(TValueType) == typeof(bool))
            {
                valueType = (TValueType)(object)internalChecked;
            }

            await CheckedChanged.InvokeAsync(valueType);
        }
    }
}