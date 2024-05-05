namespace WebApplication2.Models;

public class Order
{
    public int IdOrder { get; set; }
    public int IdProduct { get; set; }
    public int Amount { get; set; }
    public DateTime DateTime { get; set; }
}