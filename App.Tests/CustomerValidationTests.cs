using System;
using App.Models;
using App.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Tests
{
    [TestClass]
    public class CustomerValidationTests
    {
        [TestMethod]
        public void CustomerBelow21ShouldNotPassValidation()
        {
            var customer = new Customer
            {
                Firstname = "Tess",
                Surname = "d'Urbervilles",
                EmailAddress = "tess@urbervilles.co.uk",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Company = new Company() { Id = 1 }
            };

            var validator = new CustomerValidator();

            var result = validator.Validate(customer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CustomerWithBadEmailShouldNotPassValidation()
        {
            var customer = new Customer
            {
                Firstname = "Peekah",
                Surname = "Booh",
                EmailAddress = "peekahbooh.gmail.com",
                DateOfBirth = DateTime.Now.AddYears(-40),
                Company = new Company() { Id = 1 }
            };

            var validator = new CustomerValidator();

            var result = validator.Validate(customer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CustomerWithMissingNameShouldNotPassValidation()
        {
            var customer = new Customer
            {
                Surname = "Ronaldo",
                EmailAddress = "ronaldo@selecao.br",
                DateOfBirth = DateTime.Now.AddYears(-50),
                Company = new Company() { Id = 1 }
            };

            var validator = new CustomerValidator();

            var result = validator.Validate(customer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CustomerWithValidEntriesShouldPassValidation()
        {
            var customer = new Customer
            {
                Firstname = "Bob",
                Surname = "Smith",
                EmailAddress = "bob.smith@average.joe",
                DateOfBirth = DateTime.Now.AddYears(-50),
                Company = new Company() { Id = 1 }
            };

            var validator = new CustomerValidator();

            var result = validator.Validate(customer);

            Assert.IsTrue(result);
        }
    }
}