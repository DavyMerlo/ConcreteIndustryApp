namespace ConcreteIndustry.DAL.Constants
{
    public static class Column
    {
        public static class Project
        {
            public const string ProjectID = "ProjectID";
            public const string Name = "Name";
            public const string Location = "Location";
            public const string ClientID = "ClientID";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string EstimatedVolume = "EstimatedVolume";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class UserToken
        {
            public const string TokenID = "TokenID";
            public const string UserID = "UserID";
            public const string Token = "Token";
            public const string Expired = "Expired";
            public const string Revoked = "Revoked";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class RefreshToken
        {
            public const string RefreshTokenID = "RefreshTokenID";
            public const string UserID = "UserID";
            public const string RefreshTokenHash = "RefreshTokenHash";
            public const string Expired = "Expired";
            public const string Revoked = "Revoked";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class Address
        {
            public const string AddressID = "AddressID";
            public const string Street = "Street";
            public const string HouseNumber = "HouseNumber";
            public const string BoxNumber = "BoxNumber";
            public const string District = "District";
            public const string Country = "Country";
            public const string PostalCode = "PostalCode";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class Order
        {
            public const string OrderID = "OrderID";
            public const string ProjectID = "ProjectID";
            public const string ClientID = "ClientID";
            public const string ConcreteMixID = "ConcreteMixID";
            public const string Quantity = "Quantity";
            public const string OrderDate = "OrderDate";
            public const string DeliveryDate = "DeliveryDate";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class Material
        {
            public const string MaterialID = "MaterialID";
            public const string Name = "Name";
            public const string Quantity = "Quantity";
            public const string PricePerTon = "PricePerTon";
            public const string SupplierID = "SupplierID";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class ConcreteMix
        {
            public const string ConcreteMixID = "ConcreteMixID";
            public const string Name = "Name";
            public const string StrengthClass = "StrengthClass";
            public const string MaxAggregateSize = "MaxAggregateSize";
            public const string WaterCementRatio = "WaterCementRatio";
            public const string Application = "Application";
            public const string PricePerM3 = "PricePerM3";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class Supplier
        {
            public const string SupplierID = "SupplierID";
            public const string Name = "Name";
            public const string ContactPerson = "ContactPerson";
            public const string PhoneNumber = "PhoneNumber";
            public const string Email = "Email";
            public const string AddressID = "AddressID";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class Client
        {
            public const string ClientID = "ClientID";
            public const string CompanyName = "CompanyName";
            public const string ContactPerson = "ContactPerson";
            public const string PhoneNumber = "PhoneNumber";
            public const string Email = "Email";
            public const string AddressID = "AddressID";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
        }

        public static class AppUser
        {
            public const string UserID = "UserID";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string UserName = "UserName";
            public const string Email = "Email";
            public const string HashedPassword = "HashedPassword";
            public const string Role = "Role";
            public const string CreatedAt = "CreatedAt";
            public const string UpdatedAt = "UpdatedAt";
            public const string DeletedAt = "DeletedAt";
            public const string LastLoginAt = "LastLoginAt";
        }

    }
}
