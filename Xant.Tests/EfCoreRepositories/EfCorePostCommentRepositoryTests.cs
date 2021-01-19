using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _data = new List<PostComment>()
            {
                new PostComment()
                {
                    Id = 1,
                    ParentId = null,
                    Status = PostCommentStatus.Accepted,
                    User = new User(),
                    Post = new Post(),
                    UserId = null,
                    UserFullName = "UserFullName",
                    Email = "Email",
                    Body = "Body",
                    Ip = "Ip"
                },
                new PostComment()
                {
                    Id = 2,
                    ParentId = 1,
                    Status = PostCommentStatus.Unclear,
                    User = new User(),
                    Post = new Post(),
                    UserId = null,
                    UserFullName = "UserFullName",
                    Email = "Email",
                    Body = "Body",
                    Ip = "Ip"
                },
                new PostComment()
                {
                    Id = 3,
                    ParentId = 1,
                    Status = PostCommentStatus.Accepted,
                    User = new User(),
                    Post = new Post(),
                    UserId = null,
                    UserFullName = "UserFullName",
                    Email = "Email",
                    Body = "Body",
                    Ip = "Ip"
                },
                new PostComment()
                {
                    Id = 4,
                    ParentId = 1,
                    Status = PostCommentStatus.Unclear,
                    User = new User(),
                    Post = new Post(),
                    UserId = null,
                    UserFullName = "UserFullName",
                    Email = "Email",
                    Body = "Body",
                    Ip = "Ip"
                },
                new PostComment()
                {
                    Id = 5,
                    ParentId = 1,
                    Status = PostCommentStatus.Rejected,
                    User = new User(),
                    Post = new Post(),
                    UserId = null,
                    UserFullName = "UserFullName",
                    Email = "Email",
                    Body = "Body",
                    Ip = "Ip"
                }
            };

            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.PostComments.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCorePostCommentRepository(_context);
        }

        [Test]
        public void Insert_PostCommentUserIdIsNullAndUserFullNameIsNull_ThrowNullReferenceExceptionWithUserFullNameMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = null,
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
        public void Insert_PostCommentUserIdIsNullAndEmailIsNull_ThrowNullReferenceExceptionWithEmailMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = null,
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
        public void Update_PostCommentUserIdIsNullAndUserFullNameIsNull_ThrowNullReferenceExceptionWithUserFullNameMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = null,
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
        public void Update_PostCommentUserIdIsNullAndEmailIsNull_ThrowNullReferenceExceptionWithEmailMessage()
        {
            var postComment = new PostComment()
            {
                Id = 1,
                ParentId = 0,
                UserId = null,
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

        [Test]
        public async Task UpdateChildrenStatus_StatusIsRejected_AllPostCommentChildrenHaveRejectedStatus()
        {
            await _repository.UpdateChildrenStatus(1, PostCommentStatus.Rejected);


            _data
                .Where(x => x.ParentId == 1)
                .All(x => x.Status == PostCommentStatus.Rejected)
                .Should()
                .BeTrue();
        }

        [Test]
        public async Task UpdateChildrenStatus_StatusIsUnclear_AllPostCommentChildrenHaveUnclearStatus()
        {
            await _repository.UpdateChildrenStatus(1, PostCommentStatus.Unclear);


            _data
                .Where(x => x.ParentId == 1)
                .All(x => x.Status == PostCommentStatus.Unclear)
                .Should()
                .BeTrue();
        }
    }
}
