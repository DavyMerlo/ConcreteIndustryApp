namespace ConcreteIndustry.DAL.Constants
{
    public static class StoredProcedures
    {
        public const string GetUserByEmail = "GetUserByEmail";
        public const string RegisterUser = "RegisterUser";
        public const string CheckClientExists = "CheckClientExists";
        public const string ViewMaterialsById = "ViewMaterialsById";
        public const string ViewMaterials = "ViewMaterials";
        public const string ViewProjects = "ViewProjects";
        public const string ViewProjectsById = "ViewProjectsById";
        public const string AddMaterial = "AddMaterial";
        public const string AddProject = "AddProject";
        public const string UpdateProject = "UpdateProject";
        public const string DeleteProject = "DeleteProject";
        public const string IsValidRefreshToken = "IsValidRefreshToken";
        public const string ViewHashedPasswordByUserId = "ViewHashedPasswordByUserId";
        public const string ViewProjectsPaginated = "ViewProjectsPaginated";
    }
}
