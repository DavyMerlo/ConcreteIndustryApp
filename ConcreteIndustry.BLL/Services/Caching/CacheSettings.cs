using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.Services.Caching
{
    public static class CacheSettings
    {
        public static readonly DateTimeOffset CacheExpirationTime = DateTimeOffset.Now.AddMinutes(5);

        public static class Key
        {
            public const string AppUser = "appuser";
            public const string AppUsers = "appusers";
            public const string Address = "address";
            public const string Addresses = "addresses";
            public const string Project = "project";
            public const string Projects = "projects";
            public const string ProjectsPaginated = "ProjectsPaginated";
            public const string Material = "material";
            public const string Materials = "materials";
            public const string Order = "order";
            public const string Orders = "orders";
            public const string Client = "client";
            public const string Clients = "clients";
            public const string ConcreteMix = "concretemix";
            public const string ConcreteMixes = "concretemixes";
        }
    }
}
