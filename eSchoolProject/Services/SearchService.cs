using eSchoolProject.Services.IServices;

namespace eSchoolProject.Services
{
    public class SearchService : ISearchService
    {
        public IEnumerable<T> FilterList<T>(IEnumerable<T> list, string searchText, Func<T, string, bool> filterFunc)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return list ?? Enumerable.Empty<T>();

            return (list ?? Enumerable.Empty<T>()).Where(item => filterFunc(item, searchText));
        }
    }
}
