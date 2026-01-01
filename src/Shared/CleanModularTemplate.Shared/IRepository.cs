using Ardalis.Specification;

namespace CleanModularTemplate.Shared;


public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}

