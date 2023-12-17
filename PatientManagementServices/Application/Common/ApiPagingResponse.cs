namespace PatientManagementServices.Application.Common
{
    public class ApiPagingResponse<T> : ApiResponse<T>
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        
    }
}
