namespace EKZ.Models;

public class RepairSummary
{
    public int ID { get; set; }
    public int RequestID { get; set; }
    public int ServiceID { get; set; }
    public string ServiceName { get; set; }
    public string ClientName { get; set; }
    public decimal Cost { get; set; }
    public DateTime Date { get; set; }
}