using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private Mock<FakeUserManager> _fakeUserManager;
        private Mock<FakeSignInManger> _fakeSignInManager;

        [SetUp]
        public void Setup()
        {
            _data = new List<User>();
            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.Users.AddRange(_data);
            _context.SaveChanges();

            _fakeUserManager = new Mock<FakeUserManager>();
            _fakeSignInManager = new Mock<FakeSignInManger>();

            _repository = new EfCoreUserRepository(
                _fakeUserManager.Object, _fakeSignInManager.Object, new PasswordHasher<User>());
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

        [Test]
        public async Task UpdateRole_UpdateRoleFailed_ReturnFailedIdentityResult()
        {
            var user = new User()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Biography = "Biography",
                IsActive = false,
                FilesPathGuid = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _fakeUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            _fakeUserManager
                .Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await _repository.UpdateRole(user, It.IsAny<string>());

            result.Should()
                .BeOfType<IdentityResult>()
                .Which.Succeeded.Should().BeFalse();
        }
    }
}
