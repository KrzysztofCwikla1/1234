using System.Text.RegularExpressions;

namespace EShop.Application.Services
{
    public class CreditCardValidationResult
    {
        public bool IsValid { get; set; }
        public string CardType { get; set; }
    }

    public class CreditCardService
    {
        public CreditCardValidationResult Validate(string cardNumber)
        {
            string normalized = Normalize(cardNumber);

            if (normalized.Length < 13 || normalized.Length > 19 || !normalized.All(char.IsDigit))
                return new CreditCardValidationResult { IsValid = false };

            bool isValid = ValidateWithLuhn(normalized);
            string cardType = isValid ? GetCardType(normalized) : null;

            return new CreditCardValidationResult
            {
                IsValid = isValid,
                CardType = cardType
            };
        }

        public string GetCardType(string cardNumber)
        {
            string number = Normalize(cardNumber);

            if (Regex.IsMatch(number, @"^4(\d{12}|\d{15}|\d{18})$")) return "Visa";
            if (Regex.IsMatch(number, @"^(5[1-5]\d{14}|2(2[2-9][1-9]|2[3-9]\d{2}|[3-6]\d{3}|7([01]\d{2}|20\d))\d{10})$")) return "MasterCard";
            if (Regex.IsMatch(number, @"^3[47]\d{13}$")) return "American Express";
            if (Regex.IsMatch(number, @"^(6011\d{12}|65\d{14}|64[4-9]\d{13}|622(1[2-9][6-9]|[2-8]\d{2}|9([01]\d|2[0-5]))\d{10})$")) return "Discover";
            if (Regex.IsMatch(number, @"^(352[89]|35[3-8]\d)\d{12}$")) return "JCB";
            if (Regex.IsMatch(number, @"^3(0[0-5]|[68]\d)\d{11}$")) return "Diners Club";
            if (Regex.IsMatch(number, @"^(50|5[6-9]|6\d)\d{10,17}$")) return "Maestro";

            return "Unknown";
        }

        private bool ValidateWithLuhn(string number)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = number[i] - '0';
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }

        private string Normalize(string cardNumber)
        {
            return cardNumber.Replace(" ", "").Replace("-", "");
        }
    }
}
