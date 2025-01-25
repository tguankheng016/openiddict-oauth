using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OpenIddictOAuth.Web.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public const int MaxFirstNameLength = 64;

    public const int MaxLastNameLength = 64;

    [Required]
    [StringLength(MaxFirstNameLength)]
    public virtual string FirstName { get; set; } = "";

    [Required]
    [StringLength(MaxLastNameLength)]
    public virtual string LastName { get; set; } = "";
}