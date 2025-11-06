using MyApp.Common;


namespace MyApp.Driver;

public class Database : IDatabase
{
    IDBDeliveryAdress IDatabase.DeliveryAdress => throw new NotImplementedException();

    IDBCatalogs IDatabase.Catalogs => throw new NotImplementedException();

    IDBClient IDatabase.Client => throw new NotImplementedException();

    IDBDescriction IDatabase.Descriction => throw new NotImplementedException();

    IDBPublication IDatabase.Publication => throw new NotImplementedException();

    IDBPublicationCatalogs IDatabase.PublicationCatalogs => throw new NotImplementedException();

    IDBServices IDatabase.Services => throw new NotImplementedException();

    IDBSubscriptions IDatabase.Subscriptions => throw new NotImplementedException();

    IDBSubscriptionsPrice IDatabase.SubscriptionsPrice => throw new NotImplementedException();

    IDBSubscriptionservices IDatabase.Subscriptionservices => throw new NotImplementedException();

    IDBTypeAderss IDatabase.TypeAderss => throw new NotImplementedException();

    IDBTypeDescriction IDatabase.TypeDescriction => throw new NotImplementedException();
}
