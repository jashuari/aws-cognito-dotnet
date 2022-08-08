using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cognitocoreapi.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [JsonProperty("email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [JsonProperty("password")]
        public string? Password { get; set; }

    }
}