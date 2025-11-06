namespace MyApp.Common;

public class SubscriptionsPrice
{
    public int SubscriptionsPriceID { get; set; }
    public int PublicationID { get; set; }
    public DateTime Date { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public float Price { get; set; }
}