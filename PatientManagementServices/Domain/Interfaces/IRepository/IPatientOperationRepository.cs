using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Infrastructure.Repositories;
using System.Numerics;

namespace PatientManagementServices.Domain.Interfaces.IRepository
{
    public interface IPatientOperationRepository : IGenericRepository<PatientOperation, long>
    {
        // Add any specific methods related to patients if needed
        // ...
    }
}
