using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Sale
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public int CarID { get; set; }

    [ForeignKey(nameof(CarID))]
    public Car Car { get; set; } = null!;

    [Required]
    public int ClientID { get; set; }

    [ForeignKey(nameof(ClientID))]
    public Client Client { get; set; } = null!;

    [Required]
    public DateTime SaleDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    public int? EmployeeID { get; set; }

    [ForeignKey(nameof(EmployeeID))]
    public Employee? Employee { get; set; }
}