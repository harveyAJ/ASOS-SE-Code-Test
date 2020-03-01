namespace App.Models
{
    public class CreditStatus
    {
        public bool Failed { get; set; } = false;

        public bool HasCreditLimit { get; set; }

        public int CreditLimit { get; set; }
    }
}