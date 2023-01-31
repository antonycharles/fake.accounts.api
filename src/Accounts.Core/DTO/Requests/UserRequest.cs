using System.ComponentModel.DataAnnotations;

namespace Accounts.Core.DTO.Requests;

public class UserRequest : LoginRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3 )]  
    public string Name { get; set; }
}