namespace MyApp.Driver;

public class DDL
{
    public static readonly string[] Definition = {
@"CREATE DATABASE IF NOT EXISTS myapp DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `address` (
    `DeliveryAdressID` INT(11) NOT NULL AUTO_INCREMENT,
    `ParentID` INT(11) NOT NULL,
    `TypeAderssID` VARCHAR(100) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`DeliveryAdressID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `typeaddress` (
    `TypeAddressID` INT(11) NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`TypeAddressID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `client` (
    `ClientID` INT(11) NOT NULL AUTO_INCREMENT,
    `Fio` VARCHAR(250) NOT NULL,
    `Number` CHAR(11) NOT NULL,
    PRIMARY KEY (`ClientID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `publications` (
    `PublicationsID` INT(11) NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`PublicationsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `publicationscatalogs` (
    `PublicationscatalogsID` INT(11) NOT NULL AUTO_INCREMENT,
    `PublicationsID` INT(11) NOT NULL,
    `CatalogsID` INT(11) NOT NULL,
    PRIMARY KEY (PublicationscatalogsID),
    KEY `publications_catalogs_publications_FK` (PublicationsID),
    KEY `publications_catalogs_catalogs_FK` (CatalogsID),
    CONSTRAINT `publications_catalogs_publications_FK` FOREIGN KEY (`PublicationsID`) REFERENCES `publications` (`PublicationscatalogsID`),
    CONSTRAINT `publications_catalogs_catalogs_FK` FOREIGN KEY (CatalogsID) REFERENCES `catalog` (`PublicationscatalogsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `catalogs` (
    `CatalogsID` INT(11) NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(250) NOT NULL,
    `DateStart` DATE,
    `DateEnd` DATE,
    PRIMARY KEY (`CatalogsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `subscriptions` (
    `SubscriptionsID` INT(11) NOT NULL AUTO_INCREMENT,
    `ClientID` INT(11) NOT NULL,
    `PublicationID` INT(11) NOT NULL,
    `Date` DATETIME NOT NULL,
    `DateStart` DATE,
    `DateEnd` DATE,
    `Price` FLOAT NOT NULL,
    `DeliveryAdressID` INT(11) NOT NULL,
    PRIMARY KEY (SubscriptionsID),
    KEY `subscriptions_client_FK` (`ClientID`),
    KEY `subscriptions_publications_FK` (`PublicationID`),
    KEY `subscriptions_address_FK` (`DeliveryAdressID`),
    CONSTRAINT `subscriptions_client_FK` FOREIGN KEY (`ClientID`) REFERENCES `client`(`SubscriptionsID`),
    CONSTRAINT `subscriptions_publications_FK` FOREIGN KEY (`PublicationID`) REFERENCES `publications` (`SubscriptionsID`),
    CONSTRAINT `subscriptions_address_FK` FOREIGN KEY (`DeliveryAdressID`) REFERENCES `address` (`SubscriptionsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `description` (
    `DescriptionID` INT(11) NOT NULL AUTO_INCREMENT,
    `PublicationsID` INT(11) NOT NULL,
    `TypedescriptionID` INT(11) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (DescriptionID),
    KEY `description_publications_FK` (`PublicationsID`),
    KEY `description_type_description_FK` (`TypedescriptionID`),
    CONSTRAINT `description_publications_FK` FOREIGN KEY (`PublicationsID`) REFERENCES publications (`DescriptionID`),
    CONSTRAINT `description_type_description_FK` FOREIGN KEY (`TypedescriptionID`) REFERENCES type_description (`DescriptionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `typedescription` (
    `TypedescriptionID` INT(11) NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`TypedescriptionID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `subscriptionservices` (
    `SubscriptionservicesID` INT(11) NOT NULL AUTO_INCREMENT,
    `SubscriptionID` INT(11) NOT NULL,
    `ServicesID` INT(11) NOT NULL,
    PRIMARY KEY (`SubscriptionservicesID`),
    KEY `subscription_services_subscriptions_FK` (`SubscriptionID`),
    KEY `subscription_services_services_FK` (`ServicesID`),
    CONSTRAINT `subscription_services_subscriptions_FK` FOREIGN KEY (`SubscriptionID`) REFERENCES subscriptions (`SubscriptionservicesID`),
    CONSTRAINT `subscription_services_services_FK` FOREIGN KEY (`ServicesID`) REFERENCES services (`SubscriptionservicesID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `services` (
    `ServicesID` INT(11) NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(100) NOT NULL,
    PRIMARY KEY (`ServicesID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `subscriptionprices` (
    `SubscriptionpricesID` INT(11) NOT NULL AUTO_INCREMENT,
    `PublicationID` INT(11) NOT NULL,
    `Price` FLOAT NOT NULL,
    `StartDate` DATE NOT NULL,
    `StopDate` DATE,
    `TypesubscriptionsID` INT(11) NOT NULL,
    PRIMARY KEY (`SubscriptionpricesID`),
    KEY `subscription_prices_publications_FK` (`PublicationID`),
    KEY `subscription_prices_type_subscriptions_FK` (`TypesubscriptionsID`),
    CONSTRAINT `subscription_prices_publications_FK` FOREIGN KEY (`PublicationID`) REFERENCES `publications` (`SubscriptionpricesID`),
    CONSTRAINT `subscription_prices_type_subscriptions_FK` FOREIGN KEY (`TypesubscriptionsID`) REFERENCES `typesubscriptions` (`SubscriptionpricesID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

@use `myapp`;
CREATE TABLE `typesubscriptions` (
    `TypesubscriptionsID` INT(11) NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(250) NOT NULL,
    PRIMARY KEY (`TypesubscriptionsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;"
    };
}
