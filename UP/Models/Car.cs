using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Car
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Brand { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = null!;

    [Required]
    public int Year { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    public bool Availability { get; set; } = true;

    [Required]
    [MaxLength(50)]
    public string VIN { get; set; } = null!;
}
