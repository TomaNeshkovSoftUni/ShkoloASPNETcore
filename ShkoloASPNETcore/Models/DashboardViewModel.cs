namespace ShkoloASPNETcore.Web.Models
{
    public class DashboardViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int GradesCount { get; set; }
        public double AbsencesCount { get; set; }

        public int FeedbackCount { get; set; }
        public int? SchoolRank { get; set; }
        public bool IsStaff { get; set; }
    }
}