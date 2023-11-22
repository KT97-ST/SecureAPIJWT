using System.ComponentModel.DataAnnotations;

namespace SecureAPIJWT.Data
{
    public class Users
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

    }
}
