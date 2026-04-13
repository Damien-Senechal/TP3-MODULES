namespace Hotel.Housekeeping;

using Hotel.Housekeeping.Contracts;

internal interface ICleaningPolicy
{
    List<CleaningTaskDto> GenerateTasks(HousekeepingReservationDto reservation);
}

internal class StandardCleaningPolicy : ICleaningPolicy
{
    public List<CleaningTaskDto> GenerateTasks(HousekeepingReservationDto reservation)
    {
        var tasks = new List<CleaningTaskDto>();

        var current = reservation.CheckIn.AddDays(3);
        while (current < reservation.CheckOut)
        {
            tasks.Add(new CleaningTaskDto
            {
                RoomId = reservation.RoomId,
                Date = current,
                Type = "LinenChange",
                ReservationId = reservation.Id
            });
            current = current.AddDays(3);
        }

        tasks.Add(new CleaningTaskDto
        {
            RoomId = reservation.RoomId,
            Date = reservation.CheckOut,
            Type = "Departure",
            ReservationId = reservation.Id
        });

        return tasks;
    }
}

internal class VipCleaningPolicy : ICleaningPolicy
{
    public List<CleaningTaskDto> GenerateTasks(HousekeepingReservationDto reservation)
    {
        var tasks = new List<CleaningTaskDto>();

        for (var day = reservation.CheckIn.AddDays(1); day < reservation.CheckOut; day = day.AddDays(1))
        {
            tasks.Add(new CleaningTaskDto
            {
                RoomId = reservation.RoomId,
                Date = day,
                Type = "VipCleaning",
                ReservationId = reservation.Id
            });
        }

        tasks.Add(new CleaningTaskDto
        {
            RoomId = reservation.RoomId,
            Date = reservation.CheckOut,
            Type = "Departure",
            ReservationId = reservation.Id
        });

        return tasks;
    }
}
