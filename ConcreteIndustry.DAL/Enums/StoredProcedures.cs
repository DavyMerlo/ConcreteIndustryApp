using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.DAL.Enums
{
    public enum StoredProcedures
    {
        GetUserByEmail,
        RegisterUser,
        CheckClientExists,
        ViewMaterialsById,
        ViewMaterials,
        ViewProjects,
        ViewProjectsById,
        AddMaterial,
        AddProject,
        UpdateProject,
        DeleteProject,
        IsValidRefreshToken,
        ViewHashedPasswordByUserId
    }
}
