namespace Hotel.Housekeeping.Contracts;

public interface IHousekeepingService
{
    List<CleaningTaskDto> GetSchedule(DateTime date);
    void NotifyHousekeeping(DateTime date);
}

public interface IHousekeepingReservationRepository
{
    List<HousekeepingReservationDto> GetByDateRange(DateTime from, DateTime to);
}

public interface ICleaningNotifier
{
    void NotifyNewTasks(List<CleaningTaskDto> tasks);
}

public class HousekeepingReservationDto
{
    public string Id { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
}

public class CleaningTaskDto
{
    public string RoomId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string ReservationId { get; set; } = string.Empty;
}
