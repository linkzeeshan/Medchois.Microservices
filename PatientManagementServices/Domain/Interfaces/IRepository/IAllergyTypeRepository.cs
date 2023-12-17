using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Infrastructure.Repositories;
using System.Numerics;

namespace PatientManagementServices.Domain.Interfaces.IRepository
{
    public interface IAllergyTypeRepository : IGenericRepository<AllergyType, long>
    {
        // Add any specific methods related to patients if needed
        // ...
    }
}
