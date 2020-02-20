using Ferrum.Core.Utils;
using System;

namespace Ferrum.Core.Domain
{
    public class UserAccount
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserSecret { get; set; }  
        public string Salt { get; set; }
        public int ClientId { get; set; }

        public static UserAccount CreateNewUser()
        {
            var result = new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserSecret = PasswordUtils.GeneratePassword(),
                Salt = PasswordUtils.GenerateSalt()
            };

            return result;
        }
    }
}
