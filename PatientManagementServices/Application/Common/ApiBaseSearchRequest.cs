namespace PatientManagementServices.Application.Common
{
    public class ApiBaseSearchRequest
    {
        #region Properties
        /// <summary>
        /// Gets a page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Gets a page size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets paging length. Number of records that the table can display in the current draw. 
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// Gets or sets Number of rowa that the table can display in the current draw. 
        /// </summary>
        public int TotalCount { get; set; } = 0;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Search { get; set; } = string.Empty;
        public long PatientId { get; set; }
        #endregion

        #region Ctor
        public ApiBaseSearchRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public ApiBaseSearchRequest(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
        #endregion
    }
}
