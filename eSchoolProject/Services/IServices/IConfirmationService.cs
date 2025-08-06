namespace eSchoolProject.Services.IServices
{
    public interface IConfirmationService
    {
        void ShowConfirmSnackbar(string message, Action onConfirm);
    }
}