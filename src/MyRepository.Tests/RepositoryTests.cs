using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

using MyRepository.Entity;
using MyRepository.Repository;
using MyRepository.Tests.Helper;

namespace MyRepository.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            TestDataHelper.DeleteAllUsers();
        }

        [Test]
        public void repository_should_insert_entity()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();

            //act
            var newEntity = sut.Insert(entity);
            var isSaved = sut.SaveChanges();

            //assert
            Assert.IsTrue(isSaved);
            Assert.Greater(newEntity.Id, 0);
        }

        [Test]
        public void repository_should_update_entity()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.SaveChanges();

            var updatedName = Guid.NewGuid().ToString();

            //act
            entity.Name = updatedName;
            var newEntity = sut.Update(entity);
            var isSaved = sut.SaveChanges();

            //assert
            Assert.IsTrue(isSaved);
            Assert.AreEqual(newEntity.Name, updatedName);
        }

        [Test]
        public void repository_should_delete_entity_by_id()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.SaveChanges();

            //act
            sut.Delete(entity.Id);
            var isSaved = sut.SaveChanges();

            //assert
            Assert.IsTrue(isSaved);

            var deletedEntity = sut.SelectOne(x => x.Id == entity.Id);
            Assert.IsNull(deletedEntity);
        }

        [Test]
        public void repository_should_delete_entity()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.SaveChanges();

            //act
            sut.Delete(x => x.Id == entity.Id);
            var isSaved = sut.SaveChanges();

            //assert
            Assert.IsTrue(isSaved);

            var deletedEntity = sut.SelectOne(x => x.Id == entity.Id);
            Assert.IsNull(deletedEntity);
        }

        [Test]
        public void repository_should_delete_entities()
        {
            //arrange
            var entity = TestDataHelper.GetAdminUser();
            var entity2 = TestDataHelper.GetAdminUser();

            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.Insert(entity2);
            sut.SaveChanges();

            //act
            sut.Delete(x => x.IsAdmin);
            var isSaved = sut.SaveChanges();

            //assert
            Assert.IsTrue(isSaved);

            var deletedEntity = sut.SelectOne(x => x.Id == entity.Id);
            var deletedEntity2 = sut.SelectOne(x => x.Id == entity2.Id);
            Assert.IsNull(deletedEntity);
            Assert.IsNull(deletedEntity2);
        }

        [Test]
        public void repository_should_select_entity_by_id()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.SaveChanges();

            //act
            var selected = sut.SelectOne(entity.Id);

            //assert
            Assert.IsNotNull(selected);
            Assert.AreEqual(selected.Id, selected.Id);
            Assert.AreEqual(selected.Name, selected.Name);
        }

        [Test]
        public void repository_should_select_entity()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.SaveChanges();

            //act
            var selected = sut.SelectOne(x => x.Id == entity.Id);

            //assert
            Assert.IsNotNull(selected);
            Assert.AreEqual(selected.Id, selected.Id);
            Assert.AreEqual(selected.Name, selected.Name);
        }

        [Test]
        public void repository_should_select_entities()
        {
            //arrange
            var entity = TestDataHelper.GetAdminUser();
            var entity2 = TestDataHelper.GetAdminUser();

            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.Insert(entity2);
            sut.SaveChanges();

            //act
            var items = sut.SelectAll(x => x.ImageUrl == entity.ImageUrl && x.IsAdmin);

            //assert
            Assert.AreEqual(items.Count(), 2);

            foreach (var item in items)
            {
                Assert.IsTrue(item.IsAdmin);
                Assert.AreEqual(item.ImageUrl, entity.ImageUrl);
            }
        }

        [Test]
        public void repository_should_get_sum()
        {
            //arrange
            var sut = new Repository<User>();

            TestDataHelper.Add2User();

            var userIdsSum = sut.SelectAll().Sum(x => x.Id);

            //act
            var sum = sut.Sum(x => x.Id);

            //assert
            Assert.AreEqual(sum, userIdsSum);
        }

        [Test]
        public void repository_should_get_count()
        {
            //arrange
            Expression<Func<User, bool>> expression = x => x.IsActive;

            var sut = new Repository<User>();
            var colCount = sut.SelectAll(expression).Count();

            //act
            var count = sut.Count(expression);

            //assert
            Assert.AreEqual(count, colCount);
        }

        [Test]
        public void repository_should_check_if_any_exists()
        {
            //arrange
            Expression<Func<User, bool>> expression = x => x.IsActive;

            var sut = new Repository<User>();
            var colAny = sut.SelectAll(expression).Any();

            //act
            var any = sut.Any(expression);

            //assert
            Assert.AreEqual(any, colAny);
        }

        [Test]
        public void repository_should_bulk_insert_entities_slow_way()
        {
            //arrange
            const string imgurl = "image2.png";
            var entity = TestDataHelper.GetUserWithImage(imgurl);
            var entity2 = TestDataHelper.GetUserWithImage(imgurl);

            var users = new List<User> { entity, entity2 };
            var sut = new Repository<User>();

            //act
            sut.BulkInsertSlow(users);
            sut.SaveChanges();

            //assert
            var items = sut.SelectAll(x => x.ImageUrl == imgurl);
            Assert.AreEqual(2, items.Count());

            foreach (var item in items)
            {
                Assert.AreEqual(item.ImageUrl, imgurl);
            }
        }

        [Test]
        public void repository_should_bulk_insert_entities()
        {
            //arrange
            const string imgurl = "image3.png";
            var entity = TestDataHelper.GetUserWithImage(imgurl);
            var entity2 = TestDataHelper.GetUserWithImage(imgurl);

            var users = new List<User> { entity, entity2 };
            var sut = new Repository<User>();

            //act
            sut.BulkInsert(users);

            //assert
            var items = sut.SelectAll(x => x.ImageUrl == imgurl);
            Assert.AreEqual(2, items.Count());

            foreach (var item in items)
            {
                Assert.AreEqual(item.ImageUrl, imgurl);
            }
        }

        [Test]
        public void repository_should_not_insert_if_save_changes_not_called()
        {
            //arrange
            var entity = TestDataHelper.GetUser();
            var sut = new Repository<User>();

            //act
            var newEntity = sut.Insert(entity);

            //assert
            Assert.AreEqual(newEntity.Id, 0);

            var user = sut.SelectOne(x => x.Email == entity.Email);
            Assert.IsNull(user);
        }

        [Test]
        public void repository_should_not_update_if_save_changes_not_called()
        {
            //arrange
            var name = Guid.NewGuid().ToString();
            var updatedName = Guid.NewGuid().ToString();
            var entity = TestDataHelper.GetUserWithName(name);
            var sut = new Repository<User>();
            sut.Insert(entity);
            sut.SaveChanges();

            //act
            entity.Name = updatedName;
            sut.Update(entity);

            //assert
            var user = sut.SelectOne(entity.Id, true);
            Assert.IsNotNull(user);

            Assert.AreEqual(name, user.Name);
            Assert.AreNotEqual(updatedName, user.Name);
        }
    }
}
