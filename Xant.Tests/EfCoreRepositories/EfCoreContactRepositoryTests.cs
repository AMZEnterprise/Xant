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
    public class EfCoreContactRepositoryTests
    {
        private List<Contact> _data;
        private ApplicationDbContext _context;
        private EfCoreContactRepository _repository;

        [SetUp]
        public void Setup()
        {
            _data = new List<Contact>();
            _context = InMemoryDatabaseUtility.GetInMemoryDatabaseContext();

            _context.Contacts.AddRange(_data);
            _context.SaveChanges();

            _repository = new EfCoreContactRepository(_context);
        }

        [Test]
        public void Insert_ContactUserFullNameIsNull_ThrowNullReferenceExceptionWithUserFullNameMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = null,
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = "Subject",
                Body = "Body",
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.UserFullName));
        }

        [Test]
        public void Insert_ContactEmailOrPhoneNumberIsNull_ThrowNullReferenceExceptionWithEmailOrPhoneNumberMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = null,
                Subject = "Subject",
                Body = "Body",
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.EmailOrPhoneNumber));
        }

        [Test]
        public void Insert_ContactSubjectIsNull_ThrowNullReferenceExceptionWithSubjectMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = null,
                Body = "Body",
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.Subject));
        }

        [Test]
        public void Insert_ContactBodyIsNull_ThrowNullReferenceExceptionWithBodyMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = "Subject",
                Body = null,
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.Body));
        }

        [Test]
        public void Insert_ContactIpIsNull_ThrowNullReferenceExceptionWithIpMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = "Subject",
                Body = "Body",
                Ip = null,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Insert(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.Ip));
        }

        [Test]
        public void Update_ContactUserFullNameIsNull_ThrowNullReferenceExceptionWithUserFullNameMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = null,
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = "Subject",
                Body = "Body",
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.UserFullName));
        }

        [Test]
        public void Update_ContactEmailOrPhoneNumberIsNull_ThrowNullReferenceExceptionWithEmailOrPhoneNumberMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = null,
                Subject = "Subject",
                Body = "Body",
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.EmailOrPhoneNumber));
        }

        [Test]
        public void Update_ContactSubjectIsNull_ThrowNullReferenceExceptionWithSubjectMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = null,
                Body = "Body",
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.Subject));
        }

        [Test]
        public void Update_ContactBodyIsNull_ThrowNullReferenceExceptionWithBodyMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = "Subject",
                Body = null,
                Ip = "Ip",
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.Body));
        }

        [Test]
        public void Update_ContactIpIsNull_ThrowNullReferenceExceptionWithIpMessage()
        {
            var contact = new Contact()
            {
                Id = 1,
                UserFullName = "UserFullName",
                EmailOrPhoneNumber = "EmailOrPhoneNumber",
                Subject = "Subject",
                Body = "Body",
                Ip = null,
                CreateDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            _repository.Invoking(x => x.Update(contact))
                .Should()
                .Throw<NullReferenceException>()
                .WithMessage(nameof(Contact.Ip));
        }
    }
}
