using System;
using App.BusinessRules;
using App.Models;
using App.Repository;
using App.Validation;

namespace App
{
    public class CustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly ICreditCheckRule _creditCheckRule;
        private readonly IValidator<Customer> _customerValidator;

        public CustomerService()
        {
            //I am having this constructor here to maintain backward compatibility with previous consumer of the service.
            //Another option (cleaner) would have been to wrap my new CustomerService (named differently) into the 
            //original CustomerService
            //Obviously, by having this default constructor, I am breaking the dependency injection principle...
            _customerRepository = new CustomerRepository();
            _companyRepository = new CompanyRepository();
            _creditCheckRule = new CreditCheckRule(new CustomerCreditServiceClient(), _companyRepository);
            _customerValidator = new CustomerValidator();
        }

        public CustomerService(IRepository<Customer> customerRepository, 
            IRepository<Company> companyRepository, 
            ICreditCheckRule creditCheckRule, 
            IValidator<Customer> customerValidator)
        {
            _customerRepository = customerRepository;
            _companyRepository = companyRepository;
            _creditCheckRule = creditCheckRule;
            _customerValidator = customerValidator;
        }

        public bool AddCustomer(string firstName, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            var customer = new Customer
            {
                Firstname = firstName,
                Surname = surname,
                EmailAddress = email,
                DateOfBirth = dateOfBirth
            };

            if (!_customerValidator.Validate(customer))
            {
                return false;
            }

            var company = _companyRepository.GetById(companyId);

            if (company == null)
            {
                return false;
            }

            customer.Company = company;

            var status = _creditCheckRule.Apply(customer);

            if (status.Failed)
            {
                return false;
            }

            customer.HasCreditLimit = status.HasCreditLimit;
            customer.CreditLimit = status.CreditLimit;

            try
            {
                _customerRepository.Add(customer);
            }
            catch
            {
                return false;
            }
            
            return true;
        }
    }
}