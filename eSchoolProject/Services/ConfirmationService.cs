using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Services
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly ISnackbar _snackbar;

        public ConfirmationService(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        public void ShowConfirmSnackbar(string message, Action onConfirm)
        {
            var messageContent = new RenderFragment(builder =>
            {
                int seq = 0;
                builder.AddContent(seq++, message);
                builder.OpenElement(seq++, "button");
                builder.AddAttribute(seq++, "class", "mud-button mud-button-text mud-snackbar-action");
                builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () =>
                {
                    onConfirm.Invoke();
                    _snackbar.Clear(); // Snackbar’ı kapat
                }));
                builder.AddContent(seq++, "Yes");
                builder.CloseElement();

                builder.OpenElement(seq++, "button");
                builder.AddAttribute(seq++, "class", "mud-button mud-button-text mud-snackbar-action");
                builder.AddAttribute(seq++, "onclick", EventCallback.Factory.Create(this, () => _snackbar.Clear()));
                builder.AddContent(seq++, "No");
                builder.CloseElement();
            });

            _snackbar.Add(messageContent, Severity.Info, config =>
            {
                config.ShowCloseIcon = false;
                config.RequireInteraction = true; // Snackbar’ı elle kapatmak gerek
                config.VisibleStateDuration = 10000; // 10 sn görünür kalır
            });
        }
    }
}
