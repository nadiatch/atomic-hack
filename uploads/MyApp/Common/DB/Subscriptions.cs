namespace MyApp.Common;

public class Subscriptions
{
    public int SubscriptionsID { get; set; }
    public int ClientID { get; set; }
    public int PublicationID { get; set; }
    public DateTime Date { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public float Price { get; set; }
    public int DeliveryAdressID { get; set; }
}