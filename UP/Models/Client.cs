using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Address { get; set; } = null!;

    public ICollection<Request>? Requests { get; set; }
    public ICollection<Sale>? Sales { get; set; }
}