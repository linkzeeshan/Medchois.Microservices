﻿using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Infrastructure.Data;
using PatientManagementServices.Infrastructure.Repositories;
using System.Numerics;

namespace PatientManagementServices.Domain.Interfaces.IRepository
{
    public class PatientOperationRepository : GenericRepository<PatientOperation, long>, IPatientOperationRepository
    {
        public PatientOperationRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement any additional methods specific to patients if needed
    }
}
