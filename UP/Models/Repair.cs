using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Repair
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public int RequestID { get; set; }

    [ForeignKey(nameof(RequestID))]
    public Request Request { get; set; } = null!;

    [Required]
    public int ServiceID { get; set; }

    [ForeignKey(nameof(ServiceID))]
    public Service Service { get; set; } = null!;

    public int? EmployeeID { get; set; }

    [ForeignKey(nameof(EmployeeID))]
    public Employee? Employee { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Cost { get; set; }
}