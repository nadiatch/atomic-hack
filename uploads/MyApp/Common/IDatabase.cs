
namespace MyApp.Common
{
    public interface IDatabase
    {
        IDBDeliveryAdress DeliveryAdress { get; }
        IDBCatalogs Catalogs { get; }
        IDBClient Client { get; }
        IDBDescriction Descriction { get; }
        IDBPublication Publication { get; }
        IDBPublicationCatalogs PublicationCatalogs { get; }
        IDBServices Services { get; }
        IDBSubscriptions Subscriptions { get; }
        IDBSubscriptionsPrice SubscriptionsPrice { get; }
        IDBSubscriptionservices Subscriptionservices { get; }
        IDBTypeAderss TypeAderss { get; }
        IDBTypeDescriction TypeDescriction{ get;}

    } }