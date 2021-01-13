using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Persistence;
using Xant.Persistence.EfCoreRepositories;
using Xant.Tests.Utility;

namespace Xant.Tests.EfCoreRepositories
{
    [TestFixture]
    public class EfCorePostCategoryRepositoryTests
    {
        private List<PostCategory> _data;
        private ApplicationDbContext _context;
        private EfCorePostCategoryRepository _repository;

        [SetUp]
        public void Setup()
        {
            _data = new List<PostCategory>()
            {
                new PostCategory()
                {
                    Id = 1,
                    Title = "Title"
                }
            };

            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.PostCategories.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCorePostCategoryRepository(_context);
        }

        [Test]
        public void Insert_PostCategoryTitleIsNull_ThrowNullReferenceExceptionWithTitleMessage()
        {
            _repository.Invoking(x => x.Insert(new PostCategory()))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostCategory.Title));
        }

        [Test]
        public void Update_PostCategoryTitleIsNull_ThrowNullReferenceExceptionWithTitleMessage()
        {
            _repository.Invoking(x => x.Update(new PostCategory()))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostCategory.Title));
        }

        [Test]
        public async Task IsPostCategoryExists_PostCategoryExists_ReturnTrue()
        {
            var result = await _repository.IsPostCategoryExists(_data[0]);
            result.Should().BeTrue();
        }

        [Test]
        public async Task IsPostCategoryExists_PostCategoryDoesNotExists_ReturnFalse()
        {
            var result = await _repository.IsPostCategoryExists(
                new PostCategory() { Id = _data[0].Id++, Title = Guid.NewGuid().ToString() }
            );
            result.Should().BeFalse();
        }
    }
}
