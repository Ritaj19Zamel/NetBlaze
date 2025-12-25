namespace NetBlaze.Domain.Entities.Views
{
    public class AttendanceView
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; }
        public DateOnly AttendDate { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
    }
}
