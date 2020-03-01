using System;
using System.Net.Mail;
using App.Models;

namespace App.Validation
{
    public class CustomerValidator : IValidator<Customer>
    {
        public bool Validate(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Firstname) || string.IsNullOrEmpty(customer.Surname))
            {
                return false;
            }

            var dateOfBirth = customer.DateOfBirth;
            var now = DateTime.Now;
            var age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month ||
                (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age < 21)
            {
                return false;
            }

            try
            {
                var m = new MailAddress(customer.EmailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}