using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Infrastructure.Repositories;
using System.Numerics;

namespace PatientManagementServices.Domain.Interfaces.IRepository
{
    public interface IDiseaseRepository : IGenericRepository<Disease, long>
    {
        // Add any specific methods related to patients if needed
        // ...
    }
}
