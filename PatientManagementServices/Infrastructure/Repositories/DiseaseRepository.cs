using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Infrastructure.Data;
using PatientManagementServices.Infrastructure.Repositories;
using System.Numerics;

namespace PatientManagementServices.Domain.Interfaces.IRepository
{
    public class DiseaseRepository : GenericRepository<Disease, long>, IDiseaseRepository
    {
        public DiseaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement any additional methods specific to patients if needed
    }
}
