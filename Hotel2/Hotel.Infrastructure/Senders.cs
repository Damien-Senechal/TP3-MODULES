namespace Hotel.Infrastructure;

using Hotel.Booking.Contracts;
using Hotel.Housekeeping.Contracts;

public class EmailSender : IConfirmationSender
{
    public void SendBookingConfirmation(string email, string guestName, string reservationId,
        DateTime checkIn, DateTime checkOut, string roomId)
    {
        Console.WriteLine($"  [EMAIL] To: {email}");
        Console.WriteLine($"    Booking confirmed for {guestName}");
        Console.WriteLine($"    Reservation: {reservationId} | Room: {roomId}");
        Console.WriteLine($"    {checkIn:d} → {checkOut:d}");
    }
}

public class SmsSender : ICleaningNotifier
{
    public void NotifyNewTasks(List<CleaningTaskDto> tasks)
    {
        foreach (var task in tasks)
            Console.WriteLine($"  [SMS] Housekeeping: {task.Type} for room {task.RoomId} on {task.Date:d}");
    }
}
