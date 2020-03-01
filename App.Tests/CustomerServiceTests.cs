using System;
using App.BusinessRules;
using App.Models;
using App.Repository;
using App.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace App.Tests
{
    [TestClass]
    public class CustomerServiceTests
    {
        [TestMethod]
        public void ValidCustomerSuccessfullyAddedToRepo()
        {
            var customer = new Customer
            {
                Firstname = "Alice",
                Surname = "Wonderland",
                EmailAddress = "alice.wonderland@carroll.oxford",
                Company = new Company { Id = 1, Name = "L Carroll Ltd"},
                DateOfBirth = DateTime.Now.AddYears(-25)
            };

            var customerRepositoryMock = new Mock<IRepository<Customer>>();

            var companyMock = new Mock<Company>();
            companyMock.Object.Id = 1;
            companyMock.Object.Name = "L Carroll Ltd";

            var companyRepositoryMock = new Mock<IRepository<Company>>();
            companyRepositoryMock.Setup(x => x.GetById(1)).Returns(companyMock.Object);

            var creditServiceMock = new Mock<ICustomerCreditService>();
            creditServiceMock.Setup(x => x.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth)).Returns(800);
            var creditCheckRule = new CreditCheckRule(creditServiceMock.Object, companyRepositoryMock.Object);

            var customerValidator = new CustomerValidator();

            var customerService = new CustomerService(customerRepositoryMock.Object, companyRepositoryMock.Object, creditCheckRule, customerValidator);

            Assert.IsNotNull(customerService);
            var result = customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InvalidCustomerNotAddedToRepo()
        {

        }
    }
}