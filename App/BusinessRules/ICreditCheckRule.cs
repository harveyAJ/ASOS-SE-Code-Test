using App.Models;

namespace App.BusinessRules
{
    public interface ICreditCheckRule
    {
        CreditStatus Apply(Customer customer);
    }
}