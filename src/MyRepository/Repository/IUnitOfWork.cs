using System.Collections.Generic;

namespace MyRepository.Repository
{
    public interface IUnitOfWork
    {
        List<IRepository> Repositories { get; }

        void Commit();
    }
}