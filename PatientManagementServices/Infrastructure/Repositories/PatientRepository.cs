using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Domain.Interfaces.IRepository;
using PatientManagementServices.Infrastructure.Data;

namespace PatientManagementServices.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient, long>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement any additional methods specific to patients if needed
    }
}
