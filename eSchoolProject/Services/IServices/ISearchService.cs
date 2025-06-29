namespace eSchoolProject.Services.IServices
{
    public interface ISearchService
    {
        IEnumerable<T> FilterList<T>(IEnumerable<T> list, string searchText, Func<T, string, bool> filterFunc);
    }
}