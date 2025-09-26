namespace CMI.Domain.Ports
{
    public interface IQueryWrapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string resourceItemDescription)
            where T : class;

        Task<IEnumerable<T>> QueryAsync<T>(string resourceItemDescription, object parameters)
            where T : class;

        Task<IEnumerable<T>> QueryAsync<T>(string resourceItemDescription, object parameters, object[]? args)
            where T : class;

        Task<IEnumerable<dynamic>> QueryAsync(string resourceItemDescription, object[]? args);

        Task<T> QuerySingleAsync<T>(string resourceItemDescription, object parameters);

        Task<T> QuerySingleAsync<T>(string resourceItemDescription, object parameters, object[]? args);

        Task ExecuteAsync(string resourceItemDescription, object parameters);

        Task ExecuteAsync(string resourceItemDescription, object parameters, object[]? args);
    }
}
