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
    public class EfCorePostCommentRepositoryTests
    {
        private List<PostComment> _data;
        private ApplicationDbContext _context;
        private EfCorePostCommentRepository _repository;

        [SetUp]
        public void Setup()
        {
            _data = new List<PostComment>();
            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.PostComments.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCorePostCommentRepository(_context);
        }

        [Test]
        public void Insert_PostCommentUserFullNameIsNull_ThrowNullReferenceExceptionWithUserFullNameMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = null,
                Email = "Email",
                Body = "Body",
                Ip = "Ip",
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Insert(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.UserFullName));
        }

        [Test]
        public void Insert_PostCommentEmailIsNull_ThrowNullReferenceExceptionWithEmailMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = "UserFullName",
                Email = null,
                Body = "Body",
                Ip = "Ip",
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Insert(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.Email));
        }

        [Test]
        public void Insert_PostCommentBodyIsNull_ThrowNullReferenceExceptionWithBodyMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = "UserFullName",
                Email = "Email",
                Body = null,
                Ip = "Ip",
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Insert(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.Body));
        }

        [Test]
        public void Insert_PostCommentIpIsNull_ThrowNullReferenceExceptionWithIpMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = "UserFullName",
                Email = "Email",
                Body = "Body",
                Ip = null,
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Insert(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.Ip));
        }

        [Test]
        public void Update_PostCommentUserFullNameIsNull_ThrowNullReferenceExceptionWithUserFullNameMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = null,
                Email = "Email",
                Body = "Body",
                Ip = "Ip",
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Update(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.UserFullName));
        }

        [Test]
        public void Update_PostCommentEmailIsNull_ThrowNullReferenceExceptionWithEmailMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = "UserFullName",
                Email = null,
                Body = "Body",
                Ip = "Ip",
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Update(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.Email));
        }

        [Test]
        public void Update_PostCommentBodyIsNull_ThrowNullReferenceExceptionWithBodyMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = "UserFullName",
                Email = "Email",
                Body = null,
                Ip = "Ip",
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Update(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.Body));
        }

        [Test]
        public void Update_PostCommentIpIsNull_ThrowNullReferenceExceptionWithIpMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = "UserId",
                PostId = 1,
                UserFullName = "UserFullName",
                Email = "Email",
                Body = "Body",
                Ip = null,
                Status = PostCommentStatus.Rejected,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now,
                IsEdited = false
            };

            _repository.Invoking(x => x.Update(postComment))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(PostComment.Ip));
        }
    }
}
