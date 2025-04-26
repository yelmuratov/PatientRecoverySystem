namespace PatientRecoverySystem.Application.Parameters
{
    public class PatientQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string? SortBy { get; set; } = "FullName"; // default sorting
        public string? SortDirection { get; set; } = "asc"; // default ascending
    }
}
