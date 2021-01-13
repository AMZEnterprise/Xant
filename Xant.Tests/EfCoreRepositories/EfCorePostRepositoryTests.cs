using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Xant.Core.Domain;
using Xant.Persistence;
using Xant.Persistence.EfCoreRepositories;
using Xant.Tests.Utility;

namespace Xant.Tests.EfCoreRepositories
{
    [TestFixture]
    public class EfCorePostRepositoryTests
    {
        private List<Post> _data;
        private ApplicationDbContext _context;
        private EfCorePostRepository _repository;

        [SetUp]
        public void Setup()
        {
            _data = new List<Post>();
            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.Posts.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCorePostRepository(_context);
        }

        [Test]
        public void Insert_PostTitleIsNull_ThrowNullReferenceExceptionWithTitleMessage()
        {
            var post = new Post()
            {
                Id = 1,
                Title = null,
                Body = "Body",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                Tags = "Tags",
                PostCategoryId = 0,
                UserId = "UserId",
                IsCommentsOn = false,
                FilesPathGuid = Guid.NewGuid()
            };

            _repository.Invoking(x => x.Insert(post))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Post.Title));
        }

        [Test]
        public void Insert_PostBodyIsNull_ThrowNullReferenceExceptionWithBodyMessage()
        {
            var post = new Post()
            {
                Id = 1,
                Title = "Title",
                Body = null,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                Tags = "Tags",
                PostCategoryId = 0,
                UserId = "UserId",
                IsCommentsOn = false,
                FilesPathGuid = Guid.NewGuid()
            };

            _repository.Invoking(x => x.Insert(post))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Post.Body));
        }

        [Test]
        public void Update_PostTitleIsNull_ThrowNullReferenceExceptionWithTitleMessage()
        {
            var post = new Post()
            {
                Id = 1,
                Title = null,
                Body = "Body",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                Tags = "Tags",
                PostCategoryId = 0,
                UserId = "UserId",
                IsCommentsOn = false,
                FilesPathGuid = Guid.NewGuid()
            };

            _repository.Invoking(x => x.Update(post))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Post.Title));
        }

        [Test]
        public void Update_PostBodyIsNull_ThrowNullReferenceExceptionWithBodyMessage()
        {
            var post = new Post()
            {
                Id = 1,
                Title = "Title",
                Body = null,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                Tags = "Tags",
                PostCategoryId = 0,
                UserId = "UserId",
                IsCommentsOn = false,
                FilesPathGuid = Guid.NewGuid()
            };

            _repository.Invoking(x => x.Update(post))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Post.Body));
        }
    }
}
