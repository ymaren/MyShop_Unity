namespace Core.Common.Dal
{
    public interface IRepositoryFactory
    {
        IGeneric_Repository<TEntity, TKey> CreateRepository<TEntity, TKey>() where TEntity : class;
    }
}
