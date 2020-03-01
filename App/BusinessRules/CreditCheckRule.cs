using App.Models;
using App.Repository;

namespace App.BusinessRules
{
    public class CreditCheckRule : ICreditCheckRule
    {
        private readonly ICustomerCreditService _customerCreditService;
        private readonly IRepository<Company> _companyRepository;
        private readonly int _creditLimit;

        public CreditCheckRule(ICustomerCreditService customerCreditService, IRepository<Company> companyRepository, int creditLimit = 500)
        {
            _customerCreditService = customerCreditService;
            _companyRepository = companyRepository;
            _creditLimit = creditLimit;
        }

        public CreditStatus Apply(Customer customer)
        {
            var company = _companyRepository.GetById(customer.Company.Id);
            var creditStatus = new CreditStatus();

            if (company.Name == "VeryImportantClient")
            {
                // Skip credit check
                creditStatus.HasCreditLimit = false;
            }
            else if (company.Name == "ImportantClient")
            {
                // Do credit check and double credit limit
                creditStatus.HasCreditLimit = true;
                var creditLimit = _customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                creditLimit = creditLimit * 2;
                creditStatus.CreditLimit = creditLimit;
            }
            else
            {
                // Do credit check
                creditStatus.HasCreditLimit = true;
                var creditLimit = _customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                creditStatus.CreditLimit = creditLimit;
            }

            if (creditStatus.HasCreditLimit && creditStatus.CreditLimit < 500)
            {
                creditStatus.Failed = true;
            }

            return creditStatus;
        }
    }
}