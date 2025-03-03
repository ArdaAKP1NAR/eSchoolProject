namespace eSchoolProject.Services.IServices
{
    public interface ITransactionService
    {
        Task ExecuteAsync(Func<Task> operation);
    }
}