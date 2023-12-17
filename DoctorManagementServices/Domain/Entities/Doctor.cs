using UserManagementServices.Domain;

namespace DoctorManagementServices.Domain.Entities
{
    public class Doctor : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public int CountryId{ get; set; }
        public int LanguageId { get; set; }
        public string  Gender { get; set; }
    }
}
