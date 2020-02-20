using System.Collections.Generic;

namespace Ferrum.Core.Domain
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
    }
}
