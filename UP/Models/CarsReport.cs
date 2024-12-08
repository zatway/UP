namespace EKZ.Models;

public class CarsReport
{
    public int ID { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public bool Availability { get; set; }
    public int SoldCount { get; set; }
    public int RemainingStock { get; set; }
}