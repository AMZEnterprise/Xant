using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Xant.Core.Domain;
using Xant.Persistence;
using Xant.Persistence.EfCoreRepositories;
using Xant.Tests.Mocks;
using Xant.Tests.Utility;

namespace Xant.Tests.EfCoreRepositories
{
    [TestFixture]
    public class EfCoreUserRepositoryTests
    {
        private List<User> _data;
        private ApplicationDbContext _context;
        private EfCoreUserRepository _repository;

        [SetUp]
        public void Setup()
        {
            _data = new List<User>();
            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.Users.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCoreUserRepository(new FakeUserManager(), new FakeSignInManger(), new PasswordHasher<User>());
        }

        [Test]
        public void Insert_UserFirstNameIsNull_ThrowNullReferenceExceptionWithFirstNameMessage()
        {
            var user = new User()
            {
                FirstName = null,
                LastName = "LastName",
                Biography = "Biography",
                IsActive = false,
                FilesPathGuid = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(user, It.IsAny<string>()))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(User.FirstName));
        }

        [Test]
        public void Insert_UserLastNameIsNull_ThrowNullReferenceExceptionWithLastNameMessage()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = null,
                Biography = "Biography",
                IsActive = false,
                FilesPathGuid = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(user, It.IsAny<string>()))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(User.LastName));
        }

        [Test]
        public void Update_UserFirstNameIsNull_ThrowNullReferenceExceptionWithFirstNameMessage()
        {
            var user = new User()
            {
                FirstName = null,
                LastName = "LastName",
                Biography = "Biography",
                IsActive = false,
                FilesPathGuid = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(user))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(User.FirstName));
        }

        [Test]
        public void Update_UserLastNameIsNull_ThrowNullReferenceExceptionWithLastNameMessage()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = null,
                Biography = "Biography",
                IsActive = false,
                FilesPathGuid = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(user))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(User.LastName));
        }
    }
}
