namespace Web.Models.DTO
{
    public class AvailableDateAppointment
    {
        public int IdSchedule { get; set; }
        public int IdConsultory { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SlotDuration { get; set; }
        public int TotalSlots { get; set; }
        public int OccupiedSlots { get; set; }
        public int AvailableSlots { get; set; }
        public string DayOfWeek { get; set; }
    }
}
