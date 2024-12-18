using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int UserRolesId { get; set; }
    
    [ForeignKey(nameof(UserRolesId))]
    public ICollection<UserRole> UserRoles { get; set; }

}