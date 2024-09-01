using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string firstName { get; set; } = string.Empty;

    [Required]
    public string lastName { get; set; } = string.Empty;

    [Required]
    public string email { get; set; } = string.Empty;

    // Identity validators will take care of this
    [Required]
    public string Password { get; set; } = string.Empty;

}
