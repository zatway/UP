using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EKZ.Models;

public class Request
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }

    [Required]
    public int ClientID { get; set; }

    [ForeignKey(nameof(ClientID))]
    public Client Client { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = null!;
}