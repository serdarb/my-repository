using System.Linq;

using NUnit.Framework;

using MyRepository.Data;
using MyRepository.Entity;
using MyRepository.Repository;
using MyRepository.Tests.Helper;

namespace MyRepository.Tests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            TestDataHelper.DeleteAllUsers();
        }

        [Test]
        public void unit_of_work_should_commit_transaction()
        {
            //arrange
            const string imageUrl = "uow.png";
            var entity = TestDataHelper.GetUserWithImage(imageUrl);
            var entity2 = TestDataHelper.GetUserWithImage(imageUrl);

            var dbContext = new MyDbContext();
            var repo = new Repository<User>(dbContext);
            var repo2 = new Repository<User>(dbContext);

            var sut = new UnitOfWork(repo, repo2);

            //act
            repo.Insert(entity);
            repo2.Insert(entity2);

            sut.Commit();

            //assert
            var repo3 = new Repository<User>();
            var items = repo3.SelectAll(x => x.ImageUrl == imageUrl).ToList();

            Assert.AreEqual(2, items.Count);
        }

        [Test]
        public void unit_of_work_should_not_save_if_commit_not_called()
        {
            //arrange
            const string imageUrl = "uow.png";
            var entity = TestDataHelper.GetUserWithImage(imageUrl);
            var entity2 = TestDataHelper.GetUserWithImage(imageUrl);

            var dbContext = new MyDbContext();
            var repo = new Repository<User>(dbContext);
            var repo2 = new Repository<User>(dbContext);

            var sut = new UnitOfWork(repo, repo2);

            //act
            repo.Insert(entity);
            repo2.Insert(entity2);
            
            //assert
            var repo3 = new Repository<User>();
            var items = repo3.SelectAll(x => x.ImageUrl == imageUrl).ToList();

            Assert.AreNotEqual(2, items.Count);
        }
    }
}