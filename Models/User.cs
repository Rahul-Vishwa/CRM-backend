using System.ComponentModel.DataAnnotations;

namespace Artico.Models
{
    public record UsersPost
    {
        [StringLength(100, MinimumLength = 3)]
        public required string username { get; set; }
        [StringLength(255, MinimumLength = 5)]
        public required string email { get; set; }
        [StringLength(100, MinimumLength = 5)]
        public required string password { get; set; }
    }

    public record LoginModel
    {
        public required string email { get; set; }
        public required string password { get; set; }
    }

    public record UserModel
    {
        public required string username { get; set; }
        public required string email { get; set; }
        public required string token { get; set; }
    }
}
