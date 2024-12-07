using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Inventory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PricePerUnit { get; set; }
}