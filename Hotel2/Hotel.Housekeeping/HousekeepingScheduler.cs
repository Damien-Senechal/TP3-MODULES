namespace Hotel.Housekeeping;

using Hotel.Housekeeping.Contracts;

internal class HousekeepingScheduler : IHousekeepingService
{
    private readonly IHousekeepingReservationRepository _reservationRepo;
    private readonly ICleaningPolicy _cleaningPolicy;
    private readonly ICleaningNotifier _notifier;

    public HousekeepingScheduler(
        IHousekeepingReservationRepository reservationRepo,
        ICleaningPolicy cleaningPolicy,
        ICleaningNotifier notifier)
    {
        _reservationRepo = reservationRepo;
        _cleaningPolicy = cleaningPolicy;
        _notifier = notifier;
    }

    public List<CleaningTaskDto> GetSchedule(DateTime date)
    {
        var reservations = _reservationRepo.GetByDateRange(date, date.AddDays(1));
        var allTasks = new List<CleaningTaskDto>();

        foreach (var reservation in reservations)
        {
            var tasks = _cleaningPolicy.GenerateTasks(reservation);
            var todayTasks = tasks.Where(t => t.Date.Date == date.Date).ToList();
            allTasks.AddRange(todayTasks);
        }

        return allTasks;
    }

    public void NotifyHousekeeping(DateTime date)
    {
        var tasks = GetSchedule(date);
        if (tasks.Count > 0)
            _notifier.NotifyNewTasks(tasks);
    }
}
