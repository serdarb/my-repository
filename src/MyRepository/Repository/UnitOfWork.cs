using System.Collections.Generic;
using System.Transactions;

namespace MyRepository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private List<IRepository> _repositories;

        public List<IRepository> Repositories
        {
            get { return _repositories; }
        }

        public UnitOfWork(params IRepository[] repositories)
        {
            _repositories = new List<IRepository>();

            foreach (var repository in repositories)
            {
                _repositories.Add(repository);
            }
        }

        public void Commit()
        {
            using (var tran = new TransactionScope())
            {
                foreach (var repository in _repositories)
                {
                    repository.SaveChanges();
                }

                tran.Complete();
            }
        }
    }
}