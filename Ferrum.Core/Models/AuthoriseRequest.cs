using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Structs;
using Ferrum.Core.Validation.CardDate;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ferrum.Core.Models
{
    public class AuthoriseRequest: IUserRequest
    {
        [CreditCard]
        public string CardNumber { get; set; }
        
        [CardDate(CardDateType.ExpiryDate)]
        public string ExpiryDate { get; set; }
        
        [Range(0,9999, ErrorMessage = "Security Code must be between 000 and 9999.")]
        public int SecurityCode { get; set; }
        
        [Required]
        public string AccountHolder { get; set; }
        
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency Code must be 3 characters long.")]
        public string CurrencyCode { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        public string UserId { get; set; }

        public string UserSecret { get; set; }
    }
}
