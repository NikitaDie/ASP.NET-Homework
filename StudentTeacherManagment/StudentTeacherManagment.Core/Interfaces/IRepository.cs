namespace StudentTeacherManagment.Core.Interfaces
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>() where T : class;

        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}