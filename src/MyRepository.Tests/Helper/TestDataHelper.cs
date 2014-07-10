using System;

using MyRepository.Entity;
using MyRepository.Repository;

namespace MyRepository.Tests.Helper
{
    public static class TestDataHelper
    {
        public static User GetUser()
        {
            return new User
            {
                Name = Guid.NewGuid().ToString(),
                Email = string.Format("{0}@test.com", Guid.NewGuid()),
                PasswordHash = Guid.NewGuid().ToString(),
                LastLoginAt = DateTime.Now
            };
        }

        public static User GetUserWithImage(string img)
        {
            var user = GetUser();
            user.ImageUrl = img;
            return user;
        }

        public static User GetUserWithName(string name)
        {
            var user = GetUser();
            user.Name = name;
            return user;
        }

        public static User GetAdminUser()
        {
            var user = GetUser();
            user.IsAdmin = true;
            user.ImageUrl = "admin.png";
            return user;
        }

        public static void Add2User()
        {
            var sut = new Repository<User>();
            var entity = GetUser();
            var entity2 = GetUser();

            sut.Insert(entity);
            sut.Insert(entity2);
            sut.SaveChanges();
        }

        public static void DeleteAllUsers()
        {
            var sut = new Repository<User>();
            sut.Delete(x => x.Id > 0);
            sut.SaveChanges();
        }


    }
}