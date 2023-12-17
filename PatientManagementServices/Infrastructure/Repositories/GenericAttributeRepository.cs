using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Domain.Interfaces.IRepository;
using PatientManagementServices.Infrastructure.Data;

namespace PatientManagementServices.Infrastructure.Repositories
{
    public class GenericAttributeRepository : GenericRepository<GenericAttribute, long>, IGenericAttributeRepository
    {
        public GenericAttributeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
