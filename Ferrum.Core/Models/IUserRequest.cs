namespace Ferrum.Core.Models
{
    public interface IUserRequest
    {
        string UserId { get; set; }
        string UserSecret { get; set; } 
    }
}
