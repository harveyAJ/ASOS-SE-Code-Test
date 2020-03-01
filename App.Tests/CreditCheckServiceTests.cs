using System;
using App.BusinessRules;
using App.Models;
using App.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace App.Tests
{
    /// <summary>
    /// Summary description for CreditCheckServiceCheck
    /// </summary>
    [TestClass]
    public class CreditCheckServiceTests
    {
        [TestMethod]
        public void CustomerWithCreditLimitBelowThresholdShouldFailCreditCheck()
        {
            var customer = new Customer
            {
                Firstname = "Jerome",
                Surname = "Kerviel",
                DateOfBirth = new DateTime(1977, 1, 11),
                EmailAddress = "jejekerviel@socgen.fr",
                Company = new Company() { Id = 7, Name = "SocGen" }
            };

            var companyMock = new Mock<Company>();
            companyMock.Object.Id = 7;
            companyMock.Object.Name = "SocGen";

            var companyRepositoryMock = new Mock<IRepository<Company>>();
            companyRepositoryMock.Setup(m => m.GetById(7)).Returns(companyMock.Object);

            var creditServiceMock = new Mock<ICustomerCreditService>();
            creditServiceMock.Setup(m => m.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth)).Returns(499);

            var creditCheckService = new CreditCheckRule(creditServiceMock.Object, companyRepositoryMock.Object);

            var result = creditCheckService.Apply(customer);

            Assert.IsTrue(result.Failed);
            Assert.IsTrue(result.HasCreditLimit);
            Assert.AreEqual(499, result.CreditLimit);
        }

        [TestMethod]
        public void CustomerWithCreditLimitAboveThresholdShouldSucceedCreditCheck()
        {
            var customer = new Customer
            {
                Firstname = "John",
                Surname = "Dough",
                DateOfBirth = new DateTime(1977, 1, 11),
                EmailAddress = "john.dough@golddigger.com",
                Company = new Company() { Id = 1 }
            };

            var companyMock = new Mock<Company>();
            companyMock.Object.Id = 1;
            companyMock.Object.Name = "Moolah Corp.";

            var companyRepositoryMock = new Mock<IRepository<Company>>();
            companyRepositoryMock.Setup(m => m.GetById(1)).Returns(companyMock.Object);

            var creditServiceMock = new Mock<ICustomerCreditService>();
            creditServiceMock.Setup(m => m.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth)).Returns(10000);

            var creditCheckService = new CreditCheckRule(creditServiceMock.Object, companyRepositoryMock.Object);

            var result = creditCheckService.Apply(customer);

            Assert.IsFalse(result.Failed);
            Assert.IsTrue(result.HasCreditLimit);
            Assert.AreEqual(10000, result.CreditLimit);
        }

        [TestMethod]
        public void CustomerWorkingForVeryImportantClientShouldHaveNoCreditLimit()
        {
            var customer = new Customer
            {
                Firstname = "John",
                Surname = "Dough",
                DateOfBirth = new DateTime(1977, 1, 11),
                EmailAddress = "john.dough@golddigger.com",
                Company = new Company() { Id = 2 }
            };

            var companyMock = new Mock<Company>();
            companyMock.Object.Id = 2;
            companyMock.Object.Name = "VeryImportantClient";

            var companyRepositoryMock = new Mock<IRepository<Company>>();
            companyRepositoryMock.Setup(m => m.GetById(2)).Returns(companyMock.Object);

            var creditServiceMock = new Mock<ICustomerCreditService>();
            creditServiceMock.Setup(m => m.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth)).Returns(10000);

            var creditCheckService = new CreditCheckRule(creditServiceMock.Object, companyRepositoryMock.Object);

            var result = creditCheckService.Apply(customer);

            Assert.IsFalse(result.Failed);
            Assert.IsFalse(result.HasCreditLimit);
        }

        [TestMethod]
        public void CustomerWorkingForImportantClientShouldHaveDoubledCreditLimit()
        {
            var customer = new Customer
            {
                Firstname = "John",
                Surname = "Dough",
                DateOfBirth = new DateTime(1977, 1, 11),
                EmailAddress = "john.dough@golddigger.com",
                Company = new Company() { Id = 5 }
            };

            var companyMock = new Mock<Company>();
            companyMock.Object.Id = 5;
            companyMock.Object.Name = "ImportantClient";

            var companyRepositoryMock = new Mock<IRepository<Company>>();
            companyRepositoryMock.Setup(m => m.GetById(5)).Returns(companyMock.Object);

            var creditServiceMock = new Mock<ICustomerCreditService>();
            creditServiceMock.Setup(m => m.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth)).Returns(300);

            var creditCheckService = new CreditCheckRule(creditServiceMock.Object, companyRepositoryMock.Object);

            var result = creditCheckService.Apply(customer);

            Assert.IsFalse(result.Failed);
            Assert.IsTrue(result.HasCreditLimit);
            Assert.AreEqual(600, result.CreditLimit);
        }
    }
}
