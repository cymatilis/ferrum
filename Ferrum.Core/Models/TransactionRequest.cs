using Ferrum.Core.Enums.Serializable;
using Ferrum.Core.Structs;
using Ferrum.Core.Validation.CardDate;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ferrum.Core.Models
{
    public class TransactionRequest: IUserRequest
    {
        public string UserId { get; set; }

        public string UserSecret { get; set; }

        public Guid TransactionId { get; set; }
    }
}
