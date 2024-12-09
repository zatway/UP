using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public int UserRolesId { get; set; }
    
    [ForeignKey(nameof(UserRolesId))]
    public ICollection<UserRole> UserRoles { get; set; }
}